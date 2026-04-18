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

using DemaConsulting.NuGetInstaller.NuGet;

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     Unit tests for the PackagesConfigReader class.
/// </summary>
[TestClass]
public class PackagesConfigReaderTests
{
    /// <summary>
    ///     Test that Read throws InvalidOperationException when file does not exist.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_FileNotFound_ThrowsInvalidOperationException()
    {
        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(
            () => PackagesConfigReader.Read("nonexistent.config"));
        Assert.Contains("not found", exception.Message);
    }

    /// <summary>
    ///     Test that Read parses a valid packages.config with one package.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_SinglePackage_ReturnsSingleEntry()
    {
        // Arrange: write a packages.config with one package
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="TestPackage" version="1.2.3" targetFramework="net8.0" />
                </packages>
                """);

            // Act: read the file
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: verify the single package entry
            Assert.HasCount(1, packages);
            Assert.AreEqual("TestPackage", packages[0].Id);
            Assert.AreEqual("1.2.3", packages[0].Version);
            Assert.AreEqual("net8.0", packages[0].TargetFramework);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that Read parses a valid packages.config with multiple packages.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_MultiplePackages_ReturnsAllEntries()
    {
        // Arrange: write a packages.config with multiple packages
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="PackageA" version="1.0.0" />
                  <package id="PackageB" version="2.0.0" targetFramework="net9.0" />
                </packages>
                """);

            // Act: read the file
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: verify both entries
            Assert.HasCount(2, packages);
            Assert.AreEqual("PackageA", packages[0].Id);
            Assert.AreEqual("1.0.0", packages[0].Version);
            Assert.IsNull(packages[0].TargetFramework);
            Assert.AreEqual("PackageB", packages[1].Id);
            Assert.AreEqual("2.0.0", packages[1].Version);
            Assert.AreEqual("net9.0", packages[1].TargetFramework);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that Read returns an empty list for an empty packages element.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_EmptyPackages_ReturnsEmptyList()
    {
        // Arrange: write a packages.config with empty packages element
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                </packages>
                """);

            // Act: read the file
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: verify empty result
            Assert.IsEmpty(packages);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that Read throws InvalidOperationException when package element is missing id attribute.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_MissingIdAttribute_ThrowsInvalidOperationException()
    {
        // Arrange: write a packages.config with missing id attribute
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package version="1.0.0" />
                </packages>
                """);

            // Act & Assert
            var exception = Assert.ThrowsExactly<InvalidOperationException>(
                () => PackagesConfigReader.Read(tempFile));
            Assert.Contains("id", exception.Message);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that Read throws InvalidOperationException when package element is missing version attribute.
    /// </summary>
    [TestMethod]
    public void PackagesConfigReader_Read_MissingVersionAttribute_ThrowsInvalidOperationException()
    {
        // Arrange: write a packages.config with missing version attribute
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="TestPackage" />
                </packages>
                """);

            // Act & Assert
            var exception = Assert.ThrowsExactly<InvalidOperationException>(
                () => PackagesConfigReader.Read(tempFile));
            Assert.Contains("version", exception.Message);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
