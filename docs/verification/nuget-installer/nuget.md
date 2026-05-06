## NuGet Subsystem Verification

### Verification Strategy

The NuGet subsystem is verified using MSTest integration tests in `NuGetSubsystemTests.cs`.
Tests exercise the subsystem through complete workflows — reading a `packages.config`, resolving
packages from the local NuGet cache, and extracting them to a temporary output directory. No
internal components are mocked at the subsystem level; tests validate the combined behavior of
`PackagesConfigReader`, `PackageExtractor`, and `PackageInstaller` working together.

A silent `Context` (constructed with `--silent`) is used in all subsystem tests to suppress
console output without affecting functional behavior.

### Test Scenarios

#### Read and Install Workflow Scenario

Tests verify the complete read-and-install workflow: reading a valid `packages.config` and
extracting the declared packages to the output directory.

Test methods:

- `NuGetSubsystem_ReadAndInstallWorkflow_ValidConfig_ReturnsAndInstallsPackages` — asserts
  packages are extracted to the correct versioned folder
- `NuGetSubsystem_ConfigReadingWorkflow_ValidPackagesConfig_ReturnsEntries` — asserts the
  config reader returns the expected entries

#### Extraction Workflow Scenario

Tests verify that packages are extracted correctly to a new destination and that extraction is
skipped when the destination already exists.

Test methods:

- `NuGetSubsystem_ExtractionWorkflow_ValidPackage_ExtractsToDirectory` — asserts a package is
  extracted when the destination does not exist
- `NuGetSubsystem_ExtractionWorkflow_ExistingDestination_SkipsExtraction` — asserts extraction
  is skipped when the destination folder already exists

#### Exclude Version Scenario

Tests verify that version-less folder naming is used when the `excludeVersion` flag is set.

Test method:

- `NuGetSubsystem_ReadAndInstallWorkflow_ExcludeVersion_UsesFlatFolderNaming`

#### Missing File Scenario

Tests verify that an `InvalidOperationException` is thrown when the `packages.config` file
does not exist.

Test method:

- `NuGetSubsystem_MissingFileWorkflow_NonexistentConfig_ThrowsInvalidOperationException`

#### Malformed XML Scenario

Tests verify that an `XmlException` is thrown when the `packages.config` file contains
malformed XML.

Test method:

- `NuGetSubsystem_MalformedXmlWorkflow_InvalidXml_ThrowsXmlException`

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-NuGet-ReadConfig` | `NuGetSubsystem_ReadAndInstallWorkflow_ValidConfig_ReturnsAndInstallsPackages`, `NuGetSubsystem_ConfigReadingWorkflow_ValidPackagesConfig_ReturnsEntries` |
| `NuGetInstaller-NuGet-ExtractPackages` | `NuGetSubsystem_ReadAndInstallWorkflow_ValidConfig_ReturnsAndInstallsPackages`, `NuGetSubsystem_ExtractionWorkflow_ValidPackage_ExtractsToDirectory` |
| `NuGetInstaller-NuGet-SkipExisting` | `NuGetSubsystem_ExtractionWorkflow_ExistingDestination_SkipsExtraction` |
| `NuGetInstaller-NuGet-ExcludeVersion` | `NuGetSubsystem_ReadAndInstallWorkflow_ExcludeVersion_UsesFlatFolderNaming` |
| `NuGetInstaller-NuGet-MissingFile` | `NuGetSubsystem_MissingFileWorkflow_NonexistentConfig_ThrowsInvalidOperationException` |
| `NuGetInstaller-NuGet-MalformedXml` | `NuGetSubsystem_MalformedXmlWorkflow_InvalidXml_ThrowsXmlException` |
