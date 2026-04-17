# Utilities Subsystem

<!-- TODO: This is an example design section for the Utilities subsystem. Replace with your own subsystem design. -->

The `Utilities` subsystem provides shared utility functions for the NuGet Installer.
It supplies reusable, independently testable helpers that are consumed by other subsystems.

## Overview

<!-- TODO: Fill in for your project -->

The `Utilities` subsystem contains general-purpose helpers that do not belong to any
specific feature subsystem. Its primary responsibility is safe file-path manipulation,
protecting callers from path-traversal vulnerabilities when constructing paths from
external inputs.

## Units

<!-- TODO: Fill in for your project -->

The `Utilities` subsystem contains the following software unit:

| Unit          | File                       | Responsibility                              |
|---------------|----------------------------|---------------------------------------------|
| `PathHelpers` | `Utilities/PathHelpers.cs` | Safe path combination and traversal checks. |

## Interfaces

<!-- TODO: Fill in for your project -->

The `Utilities` subsystem exposes the following interface to the rest of the tool:

| Interface                     | Direction | Description                                                |
|-------------------------------|-----------|------------------------------------------------------------|
| `PathHelpers.SafePathCombine` | Outbound  | Combines two path segments, rejecting traversal sequences. |

## Interactions

<!-- TODO: Fill in for your project -->

`PathHelpers` has no dependencies on other tool units or subsystems. It uses only .NET base
class library types (`Path`, `ArgumentNullException`).
