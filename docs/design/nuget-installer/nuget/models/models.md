# Models Subsystem

The `Models` subsystem provides the data models used by the NuGet subsystem to represent
package metadata throughout the installation pipeline.

## Overview

The `Models` subsystem contains immutable data classes that carry package information between
the configuration reader and the installer. It uses `required` init-only properties to enforce
that every entry has the minimum required metadata (Id and Version).

## Units

The `Models` subsystem contains the following software unit:

| Unit           | File                           | Responsibility                             |
|----------------|--------------------------------|--------------------------------------------|
| `PackageEntry` | `NuGet/Models/PackageEntry.cs` | Data model for a single package entry.     |

## Interfaces

The `Models` subsystem exposes the following interface to the rest of the tool:

| Interface      | Direction | Description                                                     |
|----------------|-----------|-----------------------------------------------------------------|
| `PackageEntry` | Outbound  | Immutable data class consumed by reader and installer units.    |

## Error Handling

The `Models` subsystem contains only immutable data classes with no executable logic.
Error handling is not applicable at this subsystem level.

## Interactions

The `Models` subsystem has no dependencies on other tool units or subsystems. It uses only
.NET base class library types. It is consumed by `PackagesConfigReader` (which creates
instances) and `PackageInstaller` (which reads `Id` and `Version`).
