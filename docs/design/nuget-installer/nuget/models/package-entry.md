### PackageEntry

![NuGet Models Structure](NuGetModelsView.svg)

The `PackageEntry` class is a data model representing a single package entry from a
packages.config file.

#### Purpose

`PackageEntry` has a single responsibility: to carry a package's identifier, version, and
optional target framework as an immutable data record between `PackagesConfigReader` (which
constructs instances while parsing `packages.config`) and `PackageInstaller` (which reads
`Id` and `Version` to resolve and extract the package). It uses `required` init-only
properties to enforce that every entry has an `Id` and `Version`. The class is declared
`internal sealed`: `internal` confines it to assembly-internal consumers only, keeping it
out of the public API; `sealed` prevents inheritance, which is appropriate for an immutable
data carrier.

#### Data Model

| Field             | Type      | Description                                              |
|-------------------|-----------|----------------------------------------------------------|
| `Id`              | `string`  | NuGet package identifier (required).                     |
| `Version`         | `string`  | NuGet package version string (required).                 |
| `TargetFramework` | `string?` | Target framework from packages.config entry (optional).  |

#### Key Methods

N/A - `PackageEntry` is a data-only class with `required` and optional init-only properties
and no methods of its own.

#### Error Handling

N/A - `PackageEntry` performs no validation, parsing, or I/O; it only stores the values
supplied at construction. The C# compiler enforces that `Id` and `Version` are supplied
(via the `required` modifier) at every construction site, so there is no runtime error
condition for this unit to detect or handle.

#### Dependencies

`PackageEntry` has no dependencies on other units, subsystems, OTS items, or shared
packages. It uses only .NET base class library types (`string`).

#### Callers

`PackageEntry` is consumed by `PackagesConfigReader` (which creates instances while parsing
`packages.config`) and `PackageInstaller` (which reads `Id` and `Version` to resolve and
extract each package).
