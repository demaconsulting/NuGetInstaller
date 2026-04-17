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

namespace DemaConsulting.NuGetInstaller.NuGet;

/// <summary>
///     Extracts NuGet package (.nupkg) contents to a destination folder.
/// </summary>
internal static class PackageExtractor
{
    /// <summary>
    ///     Extracts all entries from a .nupkg file into the specified destination folder.
    /// </summary>
    /// <param name="nupkgPath">Path to the .nupkg file.</param>
    /// <param name="destFolder">Destination folder for extraction.</param>
    /// <returns><see langword="true"/> if extraction was performed; <see langword="false"/> if the destination folder already exists (skipped).</returns>
    public static bool Extract(string nupkgPath, string destFolder)
    {
        // If destination already exists, skip extraction
        if (Directory.Exists(destFolder))
        {
            return false;
        }

        // Extract all zip entries into the destination folder
        ZipFile.ExtractToDirectory(nupkgPath, destFolder);
        return true;
    }
}
