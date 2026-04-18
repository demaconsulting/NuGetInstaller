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

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     Unit tests for the PackageInstaller class.
/// </summary>
[TestClass]
public class PackageInstallerTests
{
    /// <summary>
    ///     Test that InstallAsync uses {Id}/ folder naming when excludeVersion is true.
    /// </summary>
    [TestMethod]
    public void PackageInstaller_InstallAsync_ExcludeVersion_UsesFlatFolderNaming()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"package_installer_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            File.WriteAllText(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """);

            // Read package entries from the config
            var packages = PackagesConfigReader.Read(configPath);

            // Act: install with excludeVersion: true
            using var context = Context.Create(["--silent"]);
            PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: true)
                .GetAwaiter().GetResult();

            // Assert: folder uses {Id}/ naming, not {Id}.{Version}/
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                $"Version-less package folder should exist at {expectedFolder}");
            Assert.IsNotEmpty(Directory.GetFileSystemEntries(expectedFolder),
                "Version-less package folder should contain extracted files");

            var versionedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.IsFalse(Directory.Exists(versionedFolder),
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
}
