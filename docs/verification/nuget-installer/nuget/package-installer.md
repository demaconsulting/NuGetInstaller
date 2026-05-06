### PackageInstaller Unit Verification

#### Verification Strategy

The `PackageInstaller` unit is verified using xUnit unit tests in `PackageInstallerTests.cs`.
Tests call `PackageInstaller.InstallAsync` with a list of `PackageEntry` objects and an output
directory path, then assert on the resulting directory structure and the status messages written
to a silent `Context`. A real `.nupkg` archive from the local NuGet cache is used to exercise
extraction; no mocking is applied within the unit.

#### Test Scenarios

##### Default Naming Scenario

Tests verify that packages are extracted to `{Id}.{Version}/` folders by default.

Test method:

- `PackageInstaller_InstallAsync_DefaultNaming_ExtractsPackageToVersionedFolder` — asserts the
  expected `{Id}.{Version}` subfolder exists in the output directory after install

##### Exclude Version Naming Scenario

Tests verify that packages are extracted to `{Id}/` folders when the `excludeVersion` flag is
set.

Test method:

- `PackageInstaller_InstallAsync_ExcludeVersion_UsesFlatFolderNaming` — asserts the expected
  `{Id}` subfolder (without version) exists in the output directory

##### Create Output Directory Scenario

Tests verify that the output directory is created automatically when it does not exist.

Test method:

- `PackageInstaller_InstallAsync_EmptyPackages_CreatesOutputDirectoryAndSucceeds` — passes an
  empty package list and a path to a non-existent directory, then asserts the directory exists

##### Status Messages Scenario

Tests verify that a status message is written for each package that is skipped because its
output folder already exists.

Test method:

- `PackageInstaller_InstallAsync_AlreadyExtracted_SkipsInstallation` — pre-creates the
  expected output folder and asserts the skip message is written to the context

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-PackageInstaller-Install` | `PackageInstaller_InstallAsync_DefaultNaming_ExtractsPackageToVersionedFolder` |
| `NuGetInstaller-PackageInstaller-FolderNamingVersioned` | `PackageInstaller_InstallAsync_DefaultNaming_ExtractsPackageToVersionedFolder` |
| `NuGetInstaller-PackageInstaller-FolderNamingExcludeVersion` | `PackageInstaller_InstallAsync_ExcludeVersion_UsesFlatFolderNaming` |
| `NuGetInstaller-PackageInstaller-CreateOutputDirectory` | `PackageInstaller_InstallAsync_EmptyPackages_CreatesOutputDirectoryAndSucceeds` |
| `NuGetInstaller-PackageInstaller-StatusMessages` | `PackageInstaller_InstallAsync_AlreadyExtracted_SkipsInstallation` |
