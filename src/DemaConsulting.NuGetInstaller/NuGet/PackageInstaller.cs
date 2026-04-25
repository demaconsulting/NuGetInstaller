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

using DemaConsulting.NuGet.Caching;
using DemaConsulting.NuGetInstaller.Cli;
using DemaConsulting.NuGetInstaller.NuGet.Models;

namespace DemaConsulting.NuGetInstaller.NuGet;

/// <summary>
///     Orchestrates the installation of NuGet packages from a packages list into an output directory.
/// </summary>
internal static class PackageInstaller
{
    /// <summary>
    ///     Installs all specified packages into the output directory.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="packages">The list of packages to install.</param>
    /// <param name="outputDirectory">The output directory for package extraction.</param>
    /// <param name="excludeVersion">When <see langword="true"/>, use {Id}/ folder naming instead of {Id}.{Version}/.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="outputDirectory"/> is <see langword="null"/> or empty.</exception>
    public static async Task InstallAsync(
        Context context,
        IReadOnlyList<PackageEntry> packages,
        string outputDirectory,
        bool excludeVersion)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(outputDirectory);

        // Ensure output directory exists
        Directory.CreateDirectory(outputDirectory);

        // Install all packages in parallel
        await Task.WhenAll(packages.Select(entry =>
            InstallPackageAsync(context, entry, outputDirectory, excludeVersion))).ConfigureAwait(false);
    }

    /// <summary>
    ///     Installs a single package by caching it and extracting to the output directory.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="entry">The package entry to install.</param>
    /// <param name="outputDirectory">The output directory for package extraction.</param>
    /// <param name="excludeVersion">When <see langword="true"/>, use {Id}/ folder naming instead of {Id}.{Version}/.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task InstallPackageAsync(
        Context context,
        PackageEntry entry,
        string outputDirectory,
        bool excludeVersion)
    {
        // Ensure the package is in the local NuGet cache
        var cacheFolder = await NuGetCache.EnsureCachedAsync(entry.Id, entry.Version).ConfigureAwait(false);

        // Build paths - NuGet global package cache uses lowercase for filenames
        var nupkgPath = Path.Combine(cacheFolder, $"{entry.Id}.{entry.Version}.nupkg".ToLowerInvariant());

        var destFolder = excludeVersion
            ? Path.Combine(outputDirectory, entry.Id)
            : Path.Combine(outputDirectory, $"{entry.Id}.{entry.Version}");

        // Extract or skip
        var extracted = PackageExtractor.Extract(nupkgPath, destFolder);

        context.WriteLine(extracted
            ? $"Installed {entry.Id} {entry.Version}"
            : $"Skipping {entry.Id} {entry.Version} (already exists)");
    }
}
