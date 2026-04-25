# PackageInstaller

The `PackageInstaller` class orchestrates the installation of NuGet packages from a
packages list into an output directory.

## Overview

`PackageInstaller` is a static class that coordinates the two-step install process:
first ensuring each package is present in the NuGet global package cache via
`NuGetCache.EnsureCachedAsync`, then extracting the cached `.nupkg` file to the output
directory via `PackageExtractor.Extract`. Packages are installed in parallel using
`Task.WhenAll`.

## Data Model

`PackageInstaller` holds no instance state. All state is local to the async methods.

## Methods

### InstallAsync(Context, IReadOnlyList\<PackageEntry\>, string, bool)

Installs all specified packages into the output directory.

**Parameters:**

| Parameter         | Type                          | Description                                          |
|-------------------|-------------------------------|------------------------------------------------------|
| `context`         | `Context`                     | Output channel for status messages.                  |
| `packages`        | `IReadOnlyList<PackageEntry>` | The list of packages to install.                     |
| `outputDirectory` | `string`                      | The output directory for package extraction.         |
| `excludeVersion`  | `bool`                        | Use `{Id}/` naming instead of `{Id}.{Version}/`.     |

**Algorithm:**

1. Create the output directory if it does not exist.
2. Install all packages in parallel via `Task.WhenAll`.

### InstallPackageAsync (private)

Installs a single package.

**Parameters:**

| Parameter         | Type           | Description                                      |
|-------------------|----------------|--------------------------------------------------|
| `context`         | `Context`      | Output channel for status messages.              |
| `entry`           | `PackageEntry` | The package entry to install.                    |
| `outputDirectory` | `string`       | The output directory for package extraction.     |
| `excludeVersion`  | `bool`         | Use `{Id}/` naming instead of `{Id}.{Version}/`. |

**Algorithm:**

1. Call `NuGetCache.EnsureCachedAsync(entry.Id, entry.Version)` to get the cache folder.
2. Construct the `.nupkg` path using lowercase (`ToLowerInvariant`) to match NuGet cache
   conventions.
3. Compute the destination folder: `{Id}/` if `excludeVersion`, else `{Id}.{Version}/`.
4. Call `PackageExtractor.Extract` to extract or skip.
5. Write a status message via `context.WriteLine`.

## Interactions

| Dependency         | Direction | Purpose                                                    |
|--------------------|-----------|------------------------------------------------------------|
| `Context`          | Uses      | Output channel for "Installed" / "Skipping" messages.      |
| `PackageEntry`     | Reads     | Reads `Id` and `Version` from each entry.                  |
| `PackageExtractor` | Uses      | Delegates ZIP extraction to `PackageExtractor.Extract`.    |
| `NuGetCache`       | Uses      | OTS library for resolving packages in the global cache.    |

## Error Handling

`InstallAsync` throws `ArgumentNullException` if `outputDirectory` is `null`, and
`ArgumentException` if `outputDirectory` is empty.

`InstallPackageAsync` does not catch exceptions. All exceptions thrown by
`NuGetCache.EnsureCachedAsync` or `PackageExtractor.Extract` propagate to the caller.
