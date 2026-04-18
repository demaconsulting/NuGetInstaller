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
using DemaConsulting.NuGetInstaller.NuGet;

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     Unit tests for the PackageExtractor class.
/// </summary>
[TestClass]
public class PackageExtractorTests
{
    /// <summary>
    ///     Test that Extract returns false when destination folder already exists.
    /// </summary>
    [TestMethod]
    public void PackageExtractor_Extract_DestinationExists_ReturnsFalse()
    {
        // Arrange: create a temporary zip file and a destination folder
        var tempDir = Path.Combine(Path.GetTempPath(), $"extractor_test_{Guid.NewGuid()}");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(destFolder);
            var zipPath = Path.Combine(tempDir, "test.nupkg");
            ZipFile.CreateFromDirectory(destFolder, zipPath);

            // Act: extract to existing destination
            var result = PackageExtractor.Extract(zipPath, destFolder);

            // Assert: should return false (skipped)
            Assert.IsFalse(result);
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
    ///     Test that Extract returns true and extracts content when destination folder does not exist.
    /// </summary>
    [TestMethod]
    public void PackageExtractor_Extract_NewDestination_ExtractsAndReturnsTrue()
    {
        // Arrange: create a temporary zip file with a test file inside
        var tempDir = Path.Combine(Path.GetTempPath(), $"extractor_test_{Guid.NewGuid()}");
        var sourceDir = Path.Combine(tempDir, "source");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "test.txt"), "test content");

            var zipPath = Path.Combine(tempDir, "test.nupkg");
            ZipFile.CreateFromDirectory(sourceDir, zipPath);

            // Act: extract to new destination
            var result = PackageExtractor.Extract(zipPath, destFolder);

            // Assert: should return true (extracted) and contain the file
            Assert.IsTrue(result);
            Assert.IsTrue(Directory.Exists(destFolder));
            Assert.IsTrue(File.Exists(Path.Combine(destFolder, "test.txt")));
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
    ///     Test that Extract throws InvalidOperationException when a zip entry would escape the destination folder (zip-slip).
    /// </summary>
    [TestMethod]
    public void PackageExtractor_Extract_ZipSlipEntry_ThrowsInvalidOperationException()
    {
        // Arrange: create a zip containing a traversal entry (../evil.txt)
        var tempDir = Path.Combine(Path.GetTempPath(), $"extractor_test_{Guid.NewGuid()}");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(tempDir);
            var zipPath = Path.Combine(tempDir, "malicious.nupkg");

            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("../evil.txt");
                using var stream = entry.Open();
                using var writer = new StreamWriter(stream);
                writer.Write("malicious content");
            }

            // Act & Assert: extraction must be rejected with the zip-slip message
            var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
                PackageExtractor.Extract(zipPath, destFolder));
            Assert.Contains("zip-slip", exception.Message);
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
    ///     Test that Extract throws InvalidOperationException when the total decompressed size exceeds 1 GB.
    /// </summary>
    [TestMethod]
    public void PackageExtractor_Extract_ZipBombEntry_ThrowsInvalidOperationException()
    {
        // Arrange: create a zip archive whose entries total more than 1 GB uncompressed
        var tempDir = Path.Combine(Path.GetTempPath(), $"extractor_test_{Guid.NewGuid()}");
        var destFolder = Path.Combine(tempDir, "output");

        try
        {
            Directory.CreateDirectory(tempDir);
            var zipPath = Path.Combine(tempDir, "bomb.zip");

            using (var zipStream = new FileStream(zipPath, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("content/bomb.bin", CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                var buffer = new byte[1024 * 1024]; // 1 MB all zeros (compresses very well)
                long bytesWritten = 0;
                const long targetSize = 1_500_000_000L; // 1.5 GB — exceeds the 1 GB limit
                while (bytesWritten < targetSize)
                {
                    var toWrite = (int)Math.Min(buffer.Length, targetSize - bytesWritten);
                    entryStream.Write(buffer, 0, toWrite);
                    bytesWritten += toWrite;
                }
            }

            // Act & Assert: extraction must be aborted when the 1 GB limit is exceeded
            var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
                PackageExtractor.Extract(zipPath, destFolder));
            Assert.Contains("zip-bomb", exception.Message);
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
