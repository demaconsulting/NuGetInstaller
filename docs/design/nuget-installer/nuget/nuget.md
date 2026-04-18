# NuGet Subsystem

The `NuGet` subsystem provides the package management functionality for the NuGet Installer.
It is responsible for reading package configuration, resolving packages from the NuGet global
cache, and extracting them into the output directory.

## Overview

The `NuGet` subsystem implements the core value proposition of the tool: installing NuGet
packages from a `packages.config` file into a local directory. It owns the entire pipeline
from XML parsing through cache resolution to ZIP extraction. All other subsystems interact
with `NuGet` only through the `PackageInstaller` orchestrator.

## Units and Child Subsystems

The `NuGet` subsystem contains the following child subsystem and units:

| Item                   | Type      | Responsibility                          |
|------------------------|-----------|-----------------------------------------|
| `Models`               | Subsystem | Package data models (`NuGet/Models/`).  |
| `PackagesConfigReader` | Unit      | XML parser for packages.config files.   |
| `PackageExtractor`     | Unit      | ZIP extraction wrapper for .nupkg.      |
| `PackageInstaller`     | Unit      | Parallel install orchestrator.          |

## Interfaces

The `NuGet` subsystem exposes the following interfaces to the rest of the tool:

| Interface                        | Direction | Description                                                           |
|----------------------------------|-----------|-----------------------------------------------------------------------|
| `PackagesConfigReader.Read`      | Outbound  | Parses a packages.config file and returns a list of `PackageEntry`.   |
| `PackageInstaller.InstallAsync`  | Outbound  | Installs all packages into the output directory.                      |
| `PackageExtractor.Extract`       | Internal  | Extracts a single .nupkg; used by `PackageInstaller`.                 |

## Interactions

| Dependency     | Direction | Purpose                                                               |
|----------------|-----------|-----------------------------------------------------------------------|
| `Context`      | Uses      | Output channel for status messages during installation.               |
| `NuGetCache`   | Uses      | OTS library for resolving packages in the NuGet global package cache. |
| `PathHelpers`  | Uses      | Safe path combination for constructing file paths.                    |

## Error Handling

| Component              | Exception                    | Condition                                                      |
|------------------------|------------------------------|----------------------------------------------------------------|
| `PackagesConfigReader` | `InvalidOperationException`  | File not found or required attribute (`id`/`version`) missing. |
| `PackagesConfigReader` | `System.Xml.XmlException`    | File contains malformed XML.                                   |
| `PackageExtractor`     | `InvalidOperationException`  | Zip-slip entry detected or total extracted size exceeds 1 GB.  |
| `PackageInstaller`     | Propagates from above        | Any exception from `PackageExtractor` or `NuGetCache`.         |

## Security Algorithms

### Zip-Slip Defense

Before writing each archive entry, `PackageExtractor` resolves the canonical destination
path via `Path.GetFullPath` and verifies it starts with the canonical destination folder
(with a trailing separator). Entries that would escape the destination folder are rejected
with `InvalidOperationException`.

### Zip-Bomb Defense

`PackageExtractor` tracks the total number of decompressed bytes written during extraction.
If the running total exceeds 1 GB (`1_073_741_824` bytes), extraction is aborted with
`InvalidOperationException`.

### Parallel Installation

`PackageInstaller` installs packages using `Task.WhenAll`, so multiple packages are cached
and extracted concurrently. Each package is processed independently with no shared mutable
state between tasks.
