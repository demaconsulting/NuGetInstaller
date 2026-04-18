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
    ///     Maximum number of bytes allowed to be extracted from a single archive (1 GB).
    ///     Extraction exceeding this limit is rejected as a potential zip-bomb.
    /// </summary>
    private const long MaxExtractedBytes = 1L * 1024 * 1024 * 1024;

    /// <summary>
    ///     Buffer size used when copying decompressed entry data to disk (80 KiB).
    /// </summary>
    private const int CopyBufferSize = 81920;

    /// <summary>
    ///     Extracts all entries from a .nupkg file into the specified destination folder.
    /// </summary>
    /// <param name="nupkgPath">Path to the .nupkg file.</param>
    /// <param name="destFolder">Destination folder for extraction.</param>
    /// <returns><see langword="true"/> if extraction was performed; <see langword="false"/> if the destination folder already exists (skipped).</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when an entry would escape the destination folder (zip-slip), or when the
    ///     total extracted size exceeds 1 GB (zip-bomb).
    /// </exception>
    public static bool Extract(string nupkgPath, string destFolder)
    {
        // If destination already exists, skip extraction
        if (Directory.Exists(destFolder))
        {
            return false;
        }

        // Canonicalise once with a guaranteed trailing separator for prefix matching
        var canonicalDestFolder = Path.GetFullPath(destFolder);
        if (!canonicalDestFolder.EndsWith(Path.DirectorySeparatorChar))
        {
            canonicalDestFolder += Path.DirectorySeparatorChar;
        }

        // Extract each zip entry individually, tracking total bytes to defend against zip-bombs
        using var archive = ZipFile.OpenRead(nupkgPath);
        var totalExtractedBytes = 0L;
        var buffer = new byte[CopyBufferSize];

        foreach (var entry in archive.Entries)
        {
            // Skip directory entries (entries with no name component after the last separator)
            if (string.IsNullOrEmpty(entry.Name))
            {
                continue;
            }

            // Resolve and validate destination path to defend against zip-slip
            var destPath = Path.GetFullPath(Path.Combine(destFolder, entry.FullName));
            if (!destPath.StartsWith(canonicalDestFolder, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Extraction of '{nupkgPath}' aborted: entry '{entry.FullName}' would escape the destination folder (zip-slip).");
            }

            // destPath is a child of canonicalDestFolder so GetDirectoryName cannot return null
            var destDir = Path.GetDirectoryName(destPath)!;
            Directory.CreateDirectory(destDir);

            using var entryStream = entry.Open();
            using var destStream = File.Create(destPath);

            int bytesRead;
            while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalExtractedBytes += bytesRead;
                if (totalExtractedBytes > MaxExtractedBytes)
                {
                    throw new InvalidOperationException(
                        $"Extraction of '{nupkgPath}' aborted: total extracted size exceeded {MaxExtractedBytes:N0} bytes (potential zip-bomb).");
                }

                destStream.Write(buffer, 0, bytesRead);
            }
        }

        return true;
    }
}
