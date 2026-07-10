## Models Subsystem Verification

### Verification Strategy

The Models subsystem is verified using xUnit unit tests in `ModelsSubsystemTests.cs`. Tests
construct `PackageEntry` instances directly and assert on the property values. No mocking is
required because the subsystem contains only data model classes with no external dependencies.

### Test Environment

Tests run in the standard xUnit test runner with no additional environment setup, external
services, or configuration files. `PackageEntry` instances are constructed entirely in memory.

### Acceptance Criteria

A subsystem test passes when a constructed `PackageEntry` instance returns exactly the values
supplied for `Id`, `Version`, and (when supplied) `TargetFramework`, and returns `null` for
`TargetFramework` when it was not supplied.

### Test Scenarios

#### Package Entry Properties Scenario

Tests verify that all properties of `PackageEntry` store and return the expected values. One
test sets all properties, including the optional `TargetFramework`. A second test omits the
optional property and verifies it is null.

Test methods:

- `ModelsSubsystem_PackageEntry_AllProperties_StoresCorrectly` — asserts that `Id`, `Version`,
  and `TargetFramework` all return the values they were initialized with
- `ModelsSubsystem_PackageEntry_OptionalTargetFramework_IsNullWhenNotSet` — asserts that
  `TargetFramework` is null when not supplied

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Models-PackageEntry` | `ModelsSubsystem_PackageEntry_AllProperties_StoresCorrectly`, `ModelsSubsystem_PackageEntry_OptionalTargetFramework_IsNullWhenNotSet` |
