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

using DemaConsulting.NuGetInstaller.Cli;
using DemaConsulting.NuGetInstaller.NuGet;

namespace DemaConsulting.NuGetInstaller.Tests.NuGet;

/// <summary>
///     Integration tests for the PackageInstaller class.
/// </summary>
public class PackageInstallerTests
{
    /// <summary>
    ///     Test that InstallAsync throws ArgumentNullException when outputDirectory is null.
    /// </summary>
    [Fact]
    public void PackageInstaller_InstallAsync_NullOutputDirectory_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = Context.Create(["--silent"]);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => PackageInstaller.InstallAsync(context, [], null!, excludeVersion: false)
                .GetAwaiter().GetResult());
    }

    /// <summary>
    ///     Test that InstallAsync creates the output directory and succeeds when the package list is empty.
    /// </summary>
    [Fact]
    public async Task PackageInstaller_InstallAsync_EmptyPackages_CreatesOutputDirectoryAndSucceeds()
    {
        // Arrange: use a non-existent output directory path
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");
        var outputDir = Path.Combine(tempDir, "output");

        try
        {
            // Act: install with an empty package list
            using var context = Context.Create(["--silent"]);
            await PackageInstaller.InstallAsync(context, [], outputDir, excludeVersion: false);

            // Assert: output directory should have been created
            Assert.True(Directory.Exists(outputDir),
                "Output directory should be created even when the package list is empty");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that InstallAsync installs packages and uses {Id}.{Version}/ folder naming by default.
    /// </summary>
    [Fact]
    public async Task PackageInstaller_InstallAsync_DefaultNaming_ExtractsPackageToVersionedFolder()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            await File.WriteAllTextAsync(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """, TestContext.Current.CancellationToken);

            // Read package entries from the config
            var packages = PackagesConfigReader.Read(configPath);

            // Act: install with default (versioned) naming
            using var context = Context.Create(["--silent"]);
            await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false);

            // Assert: folder uses {Id}.{Version}/ naming
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.True(Directory.Exists(expectedFolder),
                $"Versioned package folder should exist at {expectedFolder}");
            Assert.NotEmpty(Directory.GetFileSystemEntries(expectedFolder));
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that InstallAsync uses {Id}/ folder naming when excludeVersion is true.
    /// </summary>
    [Fact]
    public async Task PackageInstaller_InstallAsync_ExcludeVersion_UsesFlatFolderNaming()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            await File.WriteAllTextAsync(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """, TestContext.Current.CancellationToken);

            // Read package entries from the config
            var packages = PackagesConfigReader.Read(configPath);

            // Act: install with excludeVersion: true
            using var context = Context.Create(["--silent"]);
            await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: true);

            // Assert: folder uses {Id}/ naming, not {Id}.{Version}/
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching");
            Assert.True(Directory.Exists(expectedFolder),
                $"Version-less package folder should exist at {expectedFolder}");
            Assert.NotEmpty(Directory.GetFileSystemEntries(expectedFolder));

            var versionedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.False(Directory.Exists(versionedFolder),
                "Versioned package folder should not exist when excludeVersion is true");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that InstallAsync skips already-extracted packages without error.
    /// </summary>
    [Fact]
    public async Task PackageInstaller_InstallAsync_AlreadyExtracted_SkipsInstallation()
    {
        // Arrange: install a package to establish the initial state
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            await File.WriteAllTextAsync(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """, TestContext.Current.CancellationToken);

            var packages = PackagesConfigReader.Read(configPath);
            using var context = Context.Create(["--silent"]);

            await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false);

            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.True(Directory.Exists(expectedFolder), "Package should be installed after first call");

            // Act: install the same packages again (destination folder already exists)
            await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false);

            // Assert: folder still exists and installation did not throw
            Assert.True(Directory.Exists(expectedFolder),
                "Package folder should still exist after second install (skipped)");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that InstallAsync writes a status message via the context for newly installed
    ///     packages and a different status message when a package is already installed and skipped.
    /// </summary>
    [Fact]
    public async Task PackageInstaller_InstallAsync_WritesStatusMessageForInstalledAndSkippedPackages()
    {
        // Arrange: create a temporary directory with a packages.config file, and route the
        // context's status messages to a log file so they can be asserted.
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");
        var logFile = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}.log");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            await File.WriteAllTextAsync(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """, TestContext.Current.CancellationToken);

            var packages = PackagesConfigReader.Read(configPath);

            // Act: install the package for the first time, then again to trigger the skip path
            using (var context = Context.Create(["--silent", "--log", logFile]))
            {
                await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false);
                await PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false);
            }

            // Assert: the log file contains a status message for the install and the skip
            var logContent = await File.ReadAllTextAsync(logFile, TestContext.Current.CancellationToken);
            Assert.Contains("Installed DemaConsulting.NuGet.Caching 1.0.0", logContent);
            Assert.Contains("Skipping DemaConsulting.NuGet.Caching 1.0.0 (already exists)", logContent);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}
