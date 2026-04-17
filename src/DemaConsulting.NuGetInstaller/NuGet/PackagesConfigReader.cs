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

using System.Xml.Linq;
using DemaConsulting.NuGetInstaller.NuGet.Models;

namespace DemaConsulting.NuGetInstaller.NuGet;

/// <summary>
///     Reads and parses packages.config XML files.
/// </summary>
internal static class PackagesConfigReader
{
    /// <summary>
    ///     Reads a packages.config file and returns the list of package entries.
    /// </summary>
    /// <param name="filePath">Path to the packages.config file.</param>
    /// <returns>A read-only list of package entries.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the file is not found or cannot be parsed.</exception>
    public static IReadOnlyList<PackageEntry> Read(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new InvalidOperationException($"packages.config not found: {filePath}");
        }

        var doc = XDocument.Load(filePath);
        var packages = doc.Root?.Elements("package") ?? [];

        return packages
            .Select(element => new PackageEntry
            {
                Id = element.Attribute("id")?.Value
                     ?? throw new InvalidOperationException("Package element missing 'id' attribute"),
                Version = element.Attribute("version")?.Value
                          ?? throw new InvalidOperationException("Package element missing 'version' attribute"),
                TargetFramework = element.Attribute("targetFramework")?.Value
            })
            .ToList()
            .AsReadOnly();
    }
}
