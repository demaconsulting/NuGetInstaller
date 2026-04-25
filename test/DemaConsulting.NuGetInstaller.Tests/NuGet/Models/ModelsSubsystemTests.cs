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

using DemaConsulting.NuGetInstaller.NuGet.Models;

namespace DemaConsulting.NuGetInstaller.Tests.NuGet.Models;

/// <summary>
///     Subsystem tests for the NuGet Models subsystem covering PackageEntry data model.
/// </summary>
[TestClass]
public class ModelsSubsystemTests
{
    /// <summary>
    ///     Test that PackageEntry stores all properties correctly when constructed with all fields.
    /// </summary>
    [TestMethod]
    public void ModelsSubsystem_PackageEntry_AllProperties_StoresCorrectly()
    {
        // Arrange: construct a PackageEntry with all fields populated
        var entry = new PackageEntry
        {
            Id = "MyPackage",
            Version = "1.2.3",
            TargetFramework = "net8.0"
        };

        // Assert: verify all properties are stored and accessible
        Assert.AreEqual("MyPackage", entry.Id);
        Assert.AreEqual("1.2.3", entry.Version);
        Assert.AreEqual("net8.0", entry.TargetFramework);
    }

    /// <summary>
    ///     Test that PackageEntry TargetFramework is null when not set.
    /// </summary>
    [TestMethod]
    public void ModelsSubsystem_PackageEntry_OptionalTargetFramework_IsNullWhenNotSet()
    {
        // Arrange: construct a PackageEntry without TargetFramework
        var entry = new PackageEntry
        {
            Id = "AnotherPackage",
            Version = "2.0.0"
        };

        // Assert: TargetFramework is null when not explicitly set
        Assert.IsNull(entry.TargetFramework,
            "TargetFramework should be null when not specified");
    }
}
