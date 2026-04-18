# Utilities Subsystem

The `Utilities` subsystem provides shared utility functions for the NuGet Installer.
It supplies reusable, independently testable helpers that are consumed by other subsystems.

## Overview

The `Utilities` subsystem contains general-purpose helpers that do not belong to any
specific feature subsystem. Its primary responsibility is safe file-path manipulation,
protecting callers from path-traversal vulnerabilities when constructing paths from
external inputs.

## Units

The `Utilities` subsystem contains the following software unit:

| Unit          | File                       | Responsibility                              |
|---------------|----------------------------|---------------------------------------------|
| `PathHelpers` | `Utilities/PathHelpers.cs` | Safe path combination and traversal checks. |

## Interfaces

The `Utilities` subsystem exposes the following interface to the rest of the tool:

| Interface                     | Direction | Description                                                       |
|-------------------------------|-----------|-------------------------------------------------------------------|
| `PathHelpers.SafePathCombine` | Outbound  | Combines paths, rejecting null inputs and traversal sequences.    |

## Interactions

`PathHelpers` has no dependencies on other tool units or subsystems. It uses only .NET base
class library types (`Path`, `ArgumentNullException`).

## Error Handling

`SafePathCombine` throws the following exceptions:

| Exception                | Condition                                                                           |
|--------------------------|-------------------------------------------------------------------------------------|
| `ArgumentNullException`  | Either `basePath` or `relativePath` is `null`.                                      |
| `ArgumentException`      | The combined path would escape the base directory (path traversal detected).        |
| `NotSupportedException`  | A path component contains an unsupported character.                                 |
| `PathTooLongException`   | The resulting path exceeds the system's maximum path length.                        |

## Algorithm

Both paths are resolved to absolute form via `Path.GetFullPath` before using
`Path.GetRelativePath` for containment checking. This avoids simple prefix-matching
vulnerabilities where a path such as `/base-dir-sibling` could falsely appear to be
inside `/base-dir`.
