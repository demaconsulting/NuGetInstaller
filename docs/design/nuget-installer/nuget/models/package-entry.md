# PackageEntry

The `PackageEntry` class is a data model representing a single package entry from a
packages.config file.

## Overview

`PackageEntry` is a simple immutable record-style class that carries the package identifier,
version, and optional target framework between the configuration reader and the installer.
It uses `required` init-only properties to enforce that every entry has an `Id` and `Version`.

## Data Model

| Field             | Type      | Description                                              |
|-------------------|-----------|----------------------------------------------------------|
| `Id`              | `string`  | NuGet package identifier (required).                     |
| `Version`         | `string`  | NuGet package version string (required).                 |
| `TargetFramework` | `string?` | Target framework from packages.config entry (optional).  |

## Methods

`PackageEntry` has no methods. It is a data-only class with init-only properties.

## Interactions

`PackageEntry` has no dependencies on other units. It is consumed by `PackagesConfigReader`
(which creates instances) and `PackageInstaller` (which reads `Id` and `Version`).
