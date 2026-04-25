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
using DemaConsulting.NuGetInstaller.NuGet;

namespace DemaConsulting.NuGetInstaller.Tests.NuGet;

/// <summary>
///     Subsystem tests for the NuGet subsystem covering PackagesConfigReader, PackageExtractor,
///     and PackageInstaller integration workflows.
/// </summary>
[TestClass]
public class NuGetSubsystemTests
{
    /// <summary>
    ///     Test that the NuGet subsystem reads a packages.config and installs packages end-to-end.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ReadAndInstallWorkflow_ValidConfig_ReturnsAndInstallsPackages()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_subsystem_test_{Guid.NewGuid()}");

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

            // Act: read config and install packages using the subsystem pipeline
            var packages = PackagesConfigReader.Read(configPath);
            using var context = Context.Create(["--silent"]);
            PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: false)
                .GetAwaiter().GetResult();

            // Assert: verify packages were read and installed
            Assert.HasCount(1, packages);
            Assert.AreEqual("DemaConsulting.NuGet.Caching", packages[0].Id);
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                "Package should be extracted to output directory");
            Assert.IsNotEmpty(Directory.GetFileSystemEntries(expectedFolder),
                "Extracted folder should contain files");
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
    ///     Test that the NuGet subsystem config reading workflow returns correct entries.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ConfigReadingWorkflow_ValidPackagesConfig_ReturnsEntries()
    {
        // Arrange: create a packages.config with multiple entries
        var tempFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="PackageA" version="1.0.0" />
                  <package id="PackageB" version="2.0.0" targetFramework="net8.0" />
                </packages>
                """);

            // Act: read packages through the subsystem reader
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: verify all entries are returned with correct data
            Assert.HasCount(2, packages);
            Assert.AreEqual("PackageA", packages[0].Id);
            Assert.AreEqual("1.0.0", packages[0].Version);
            Assert.AreEqual("PackageB", packages[1].Id);
            Assert.AreEqual("2.0.0", packages[1].Version);
            Assert.AreEqual("net8.0", packages[1].TargetFramework);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that the NuGet subsystem extraction workflow extracts to new directory.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ExtractionWorkflow_ValidPackage_ExtractsToDirectory()
    {
        // Arrange: create a temporary zip file simulating a .nupkg
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_subsystem_test_{Guid.NewGuid()}");
        var sourceDir = Path.Combine(tempDir, "source");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "content.txt"), "package content");

            var zipPath = Path.Combine(tempDir, "test.nupkg");
            ZipFile.CreateFromDirectory(sourceDir, zipPath);

            // Act: extract the package through the subsystem extractor
            var result = PackageExtractor.Extract(zipPath, destFolder);

            // Assert: verify extraction occurred and content is present
            Assert.IsTrue(result, "Extract should return true for new destination");
            Assert.IsTrue(Directory.Exists(destFolder), "Destination folder should exist");
            Assert.IsTrue(File.Exists(Path.Combine(destFolder, "content.txt")),
                "Extracted content should be present");
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
    ///     Test that the NuGet subsystem extraction workflow skips existing destinations.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ExtractionWorkflow_ExistingDestination_SkipsExtraction()
    {
        // Arrange: create a temporary zip file and pre-existing destination
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_subsystem_test_{Guid.NewGuid()}");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(destFolder);
            var zipPath = Path.Combine(tempDir, "test.nupkg");
            ZipFile.CreateFromDirectory(destFolder, zipPath);

            // Act: attempt extraction to existing destination
            var result = PackageExtractor.Extract(zipPath, destFolder);

            // Assert: verify extraction was skipped
            Assert.IsFalse(result, "Extract should return false for existing destination");
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
    ///     Test that the NuGet subsystem installs packages using {Id}/ folder naming when excludeVersion is true.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ReadAndInstallWorkflow_ExcludeVersion_UsesFlatFolderNaming()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_subsystem_test_{Guid.NewGuid()}");

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

            // Act: read config and install packages using excludeVersion: true
            var packages = PackagesConfigReader.Read(configPath);
            using var context = Context.Create(["--silent"]);
            PackageInstaller.InstallAsync(context, packages, tempDir, excludeVersion: true)
                .GetAwaiter().GetResult();

            // Assert: folder uses {Id}/ naming, not {Id}.{Version}/
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                "Version-less package folder should exist when excludeVersion is true");
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

    /// <summary>
    ///     Test that the NuGet subsystem throws InvalidOperationException when packages.config does not exist.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_MissingFileWorkflow_NonexistentConfig_ThrowsInvalidOperationException()
    {
        // Arrange: use a path to a non-existent file
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.config");

        // Act & Assert: verify the subsystem throws InvalidOperationException
        var exception = Assert.ThrowsExactly<InvalidOperationException>(
            () => PackagesConfigReader.Read(nonExistentPath));
        Assert.Contains("not found", exception.Message);
    }

    /// <summary>
    ///     Test that the NuGet subsystem rejects a zip entry that would escape the destination folder (zip-slip).
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ExtractionWorkflow_ZipSlipEntry_ThrowsInvalidOperationException()
    {
        // Arrange: build a zip archive containing a zip-slip entry
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_zipslip_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var zipPath = Path.Combine(tempDir, "malicious.nupkg");
            var destFolder = Path.Combine(tempDir, "output");

            // Create a zip with a path-traversal entry name
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("../../escaped.txt");
                using var stream = entry.Open();
                stream.WriteByte(0);
            }

            // Act & Assert: extraction must throw InvalidOperationException for the zip-slip entry
            Assert.ThrowsExactly<InvalidOperationException>(
                () => PackageExtractor.Extract(zipPath, destFolder));
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
    ///     Test that the NuGet subsystem aborts extraction when decompressed bytes exceed 1 GB (zip-bomb).
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_ExtractionWorkflow_ZipBombExceedsLimit_ThrowsInvalidOperationException()
    {
        // Arrange: create a zip with entries whose declared uncompressed sizes total more than 1 GB.
        // We use many small entries that together claim to exceed the limit.
        // The actual data is sparse (all zeros), which compresses extremely well,
        // so the zip file itself is tiny; the size check fires on decompressed bytes written.
        var tempDir = Path.Combine(Path.GetTempPath(), $"nuget_zipbomb_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var zipPath = Path.Combine(tempDir, "bomb.nupkg");
            var destFolder = Path.Combine(tempDir, "output");

            // Build a zip with 1100 entries × 1 MiB each = ~1.1 GiB total uncompressed
            const int entryCount = 1100;
            const int entrySize = 1024 * 1024; // 1 MiB
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var data = new byte[entrySize];
                for (var i = 0; i < entryCount; i++)
                {
                    var entry = archive.CreateEntry($"entry{i:D4}.bin",
                        CompressionLevel.Optimal);
                    using var stream = entry.Open();
                    stream.Write(data, 0, data.Length);
                }
            }

            // Act & Assert: extraction must throw InvalidOperationException for zip-bomb
            Assert.ThrowsExactly<InvalidOperationException>(
                () => PackageExtractor.Extract(zipPath, destFolder));
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
    ///     Test that the NuGet subsystem throws XmlException when packages.config contains malformed XML.
    /// </summary>
    [TestMethod]
    public void NuGetSubsystem_MalformedXmlWorkflow_InvalidXml_ThrowsXmlException()
    {
        // Arrange: write a malformed XML file
        var tempFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFile, "<packages><package id=\"A\" version=\"1.0\"</packages>");

            // Act & Assert: malformed XML must throw XmlException
            Assert.ThrowsExactly<System.Xml.XmlException>(
                () => PackagesConfigReader.Read(tempFile));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
