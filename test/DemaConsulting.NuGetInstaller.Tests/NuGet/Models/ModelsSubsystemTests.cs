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
///     Subsystem tests for the NuGet Models subsystem covering PackageEntry data model workflows.
/// </summary>
[TestClass]
public class ModelsSubsystemTests
{
    /// <summary>
    ///     Test that the Models subsystem represents package metadata correctly through the
    ///     PackagesConfigReader integration.
    /// </summary>
    [TestMethod]
    public void ModelsSubsystem_PackageEntryWorkflow_ValidEntries_RepresentsPackageMetadata()
    {
        // Arrange: create a packages.config with entries exercising all PackageEntry fields
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

            // Act: read packages through the reader to produce PackageEntry instances
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: verify PackageEntry data model represents all fields correctly
            Assert.HasCount(2, packages);

            Assert.AreEqual("PackageA", packages[0].Id);
            Assert.AreEqual("1.0.0", packages[0].Version);
            Assert.IsNull(packages[0].TargetFramework,
                "TargetFramework should be null when not specified");

            Assert.AreEqual("PackageB", packages[1].Id);
            Assert.AreEqual("2.0.0", packages[1].Version);
            Assert.AreEqual("net8.0", packages[1].TargetFramework,
                "TargetFramework should be preserved when specified");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    ///     Test that the Models subsystem returns an empty list for a self-closing packages element.
    /// </summary>
    [TestMethod]
    public void ModelsSubsystem_ReadPackages_EmptyPackagesElement_ReturnsEmptyList()
    {
        // Arrange: create a packages.config with a self-closing packages element
        var tempFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFile, "<packages/>");

            // Act: read packages through the reader
            var packages = PackagesConfigReader.Read(tempFile);

            // Assert: empty packages element returns an empty list
            Assert.IsEmpty(packages, "Self-closing <packages/> element should return an empty list");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
