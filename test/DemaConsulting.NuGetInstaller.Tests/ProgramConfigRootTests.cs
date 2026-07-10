// Copyright (c) DEMA Consulting
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.IO.Compression;
using DemaConsulting.NuGetInstaller.Cli;

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     End-to-end tests proving that <see cref="Program.Run"/> derives a <c>configRoot</c> from
///     the <c>packages.config</c> location and forwards it through to NuGet settings discovery,
///     so a project/repo-local <c>nuget.config</c> is found the same way <c>dotnet restore</c>
///     finds it - regardless of the process's actual current working directory.
/// </summary>
/// <remarks>
///     This is a regression test for GitHub issue #37: <c>NuGetCache.EnsureCachedAsync</c>
///     previously ignored any repo-local <c>nuget.config</c> because no root directory was
///     forwarded to <c>Settings.LoadDefaultSettings</c>. Each test here builds an isolated
///     temporary "repository" containing a <c>nuget.config</c> at its root (defining a local
///     folder package source and a private global packages folder, so no real machine state
///     is touched) and a nested <c>packages.config</c> in a subdirectory - mirroring the
///     <c>HiArc.LaunchCode.HardwareSim</c> layout where <c>nuget.config</c> lives several
///     directories above the project's <c>packages.config</c>.
/// </remarks>
public class ProgramConfigRootTests
{
    /// <summary>
    ///     Test that <c>Program.Run</c> installs a package from a source defined only in a
    ///     repo-local <c>nuget.config</c> located in an ancestor of the <c>packages.config</c>
    ///     directory, proving the derived <c>configRoot</c> is honored end-to-end.
    /// </summary>
    [Fact]
    public void Program_Run_WithRepoLocalNugetConfigAboveNestedPackagesConfig_InstallsFromLocalSource()
    {
        // Arrange - build an isolated "repository" layout:
        //   repoRoot/
        //     nuget.config              <- defines the only source that can resolve the package
        //     feed/{id}.{version}.nupkg <- the package content
        //     project/packages.config   <- references the package; several levels of walk-up
        //                                  from here reach nuget.config, matching the real
        //                                  HardwareSim repo layout
        //     packages/                 <- installer output directory
        //     global-packages/          <- private global packages folder (keeps this test
        //                                  from touching the real machine-wide NuGet cache)
        var repoRoot = Path.Combine(Path.GetTempPath(), $"nuget_installer_config_root_test_{Guid.NewGuid():N}");
        var feedDir = Path.Combine(repoRoot, "feed");
        var projectDir = Path.Combine(repoRoot, "project");
        var outputDir = Path.Combine(repoRoot, "packages");
        var globalPackagesDir = Path.Combine(repoRoot, "global-packages");
        var packageId = $"Test.ConfigRootDiscovery.{Guid.NewGuid():N}";
        const string version = "1.0.0";

        try
        {
            Directory.CreateDirectory(feedDir);
            Directory.CreateDirectory(projectDir);
            Directory.CreateDirectory(globalPackagesDir);

            // Write the package into the local folder feed
            File.WriteAllBytes(
                Path.Combine(feedDir, $"{packageId}.{version}.nupkg"),
                CreateMinimalNupkgBytes(packageId, version));

            // Write a repo-local nuget.config at the repo root defining the local feed as the
            // only source and a private global packages folder
            File.WriteAllText(Path.Combine(repoRoot, "nuget.config"), $"""
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                  <config>
                    <add key="globalPackagesFolder" value="{globalPackagesDir}" />
                  </config>
                  <packageSources>
                    <clear />
                    <add key="repo-local-feed" value="{feedDir}" />
                  </packageSources>
                </configuration>
                """);

            // Write packages.config nested below the repo root, matching how HardwareSim's
            // packages.config sits several directories below its repo-root nuget.config
            var configPath = Path.Combine(projectDir, "packages.config");
            File.WriteAllText(configPath, $"""
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="{packageId}" version="{version}" />
                </packages>
                """);

            var originalOut = Console.Out;
            try
            {
                using var outWriter = new StringWriter();
                Console.SetOut(outWriter);
                using var context = Context.Create([configPath, "-o", outputDir]);

                // Act - run the program; the process's actual working directory is unrelated to
                // repoRoot, so success can only be explained by Program deriving configRoot from
                // the packages.config location and NuGetCache honoring it
                Program.Run(context);

                // Assert - installation succeeded and the package was extracted
                Assert.Equal(0, context.ExitCode);
                var expectedFolder = Path.Combine(outputDir, $"{packageId}.{version}");
                Assert.True(
                    Directory.Exists(expectedFolder),
                    $"Expected package folder to exist at: {expectedFolder}");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        finally
        {
            if (Directory.Exists(repoRoot))
            {
                Directory.Delete(repoRoot, recursive: true);
            }
        }
    }

    /// <summary>
    ///     Test that <c>Program.Run</c> fails to resolve a package whose only source is defined
    ///     in a <c>nuget.config</c> that does not exist anywhere above the <c>packages.config</c>
    ///     directory, proving the previous test's success is due to genuine discovery of the
    ///     repo-local config rather than an unrelated fallback.
    /// </summary>
    [Fact]
    public void Program_Run_WithoutRepoLocalNugetConfig_PackageNotFound_ThrowsInvalidOperationException()
    {
        // Arrange - same nested packages.config as above, but with no nuget.config anywhere in
        // the temp tree, so the package (which does not exist on any real source) cannot resolve
        var repoRoot = Path.Combine(Path.GetTempPath(), $"nuget_installer_config_root_test_{Guid.NewGuid():N}");
        var projectDir = Path.Combine(repoRoot, "project");
        var outputDir = Path.Combine(repoRoot, "packages");
        var packageId = $"Test.ConfigRootDiscovery.{Guid.NewGuid():N}";
        const string version = "1.0.0";

        try
        {
            Directory.CreateDirectory(projectDir);

            var configPath = Path.Combine(projectDir, "packages.config");
            File.WriteAllText(configPath, $"""
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="{packageId}" version="{version}" />
                </packages>
                """);

            var originalOut = Console.Out;
            try
            {
                using var outWriter = new StringWriter();
                Console.SetOut(outWriter);
                using var context = Context.Create([configPath, "-o", outputDir]);

                // Act & Assert - with no repo-local source defining this package, resolution must
                // fail; NuGetCache.EnsureCachedAsync surfaces this as InvalidOperationException,
                // which Program.Run (called directly, bypassing Main's try/catch) propagates
                Assert.Throws<InvalidOperationException>(() => Program.Run(context));
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        finally
        {
            if (Directory.Exists(repoRoot))
            {
                Directory.Delete(repoRoot, recursive: true);
            }
        }
    }

    /// <summary>
    ///     Builds a minimal valid .nupkg byte array for the given package identity using only
    ///     <see cref="ZipArchive"/> (no NuGet SDK package-building types are referenced by this
    ///     test project). NuGet's local-folder feed resolution only requires a top-level
    ///     <c>*.nuspec</c> entry containing valid package metadata; full OPC parts
    ///     (<c>[Content_Types].xml</c>, relationship parts) are not required for reading.
    /// </summary>
    /// <param name="packageId">The NuGet package identifier.</param>
    /// <param name="version">The package version string.</param>
    /// <returns>A byte array containing a minimal .nupkg archive.</returns>
    private static byte[] CreateMinimalNupkgBytes(string packageId, string version)
    {
        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            // Top-level nuspec entry - required by NuGet's package reader to identify the package
            var nuspecEntry = archive.CreateEntry($"{packageId}.nuspec");
            using (var entryStream = nuspecEntry.Open())
            using (var writer = new StreamWriter(entryStream))
            {
                writer.Write($"""
                    <?xml version="1.0" encoding="utf-8"?>
                    <package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">
                      <metadata>
                        <id>{packageId}</id>
                        <version>{version}</version>
                        <authors>Test</authors>
                        <description>Minimal test package for configRoot discovery tests.</description>
                      </metadata>
                    </package>
                    """);
            }

            // A placeholder content file so the extracted package folder is non-empty
            var contentEntry = archive.CreateEntry("lib/net8.0/_placeholder.txt");
            using (var entryStream = contentEntry.Open())
            using (var writer = new StreamWriter(entryStream))
            {
                writer.Write("placeholder");
            }
        }

        return stream.ToArray();
    }
}
