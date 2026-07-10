### PackageEntry Unit Verification

#### Verification Strategy

The `PackageEntry` unit is verified using xUnit unit tests in `ModelsSubsystemTests.cs`.
Tests construct `PackageEntry` instances directly and assert on the stored property values. No
dependencies require mocking.

Note: `PackageEntry` is a data model class with no methods of its own. Its tests share the
`ModelsSubsystemTests.cs` file with other Models subsystem tests.

#### Test Environment

Tests run in the standard xUnit test runner with no additional environment setup, external
services, or configuration files. `PackageEntry` instances are constructed entirely in memory.

#### Acceptance Criteria

A unit test passes when a constructed `PackageEntry` instance returns exactly the values
supplied for `Id`, `Version`, and (when supplied) `TargetFramework`, and returns `null` for
`TargetFramework` when it was not supplied.

#### Test Scenarios

##### Data Model Scenario

Tests verify that `PackageEntry` stores its `Id`, `Version`, and optional `TargetFramework`
properties correctly.

Test methods:

- `ModelsSubsystem_PackageEntry_AllProperties_StoresCorrectly` — constructs a `PackageEntry`
  with all three properties and asserts each is returned correctly
- `ModelsSubsystem_PackageEntry_OptionalTargetFramework_IsNullWhenNotSet` — constructs a
  `PackageEntry` without `TargetFramework` and asserts the property is null

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-PackageEntry-DataModel` | `ModelsSubsystem_PackageEntry_AllProperties_StoresCorrectly`, `ModelsSubsystem_PackageEntry_OptionalTargetFramework_IsNullWhenNotSet` |
