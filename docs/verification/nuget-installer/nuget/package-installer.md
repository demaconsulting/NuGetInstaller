### PackageInstaller Unit Verification

#### Verification Strategy

The `PackageInstaller` unit is verified using xUnit unit tests in `PackageInstallerTests.cs`.
Tests call `PackageInstaller.InstallAsync` with a list of `PackageEntry` objects and an output
directory path, then assert on the resulting directory structure and the status messages written
to a silent `Context`. A real `.nupkg` archive from the local NuGet cache is used to exercise
extraction; no mocking is applied within the unit.

#### Test Environment

Tests require the local NuGet package cache (populated by `dotnet restore` during the build)
to contain the `DemaConsulting.NuGet.Caching` package used as the extraction fixture, and a
writable OS temporary directory for output folders. No network access is required at test
time because the package is resolved from the local cache. Tests run under the standard
xUnit test runner.

#### Acceptance Criteria

A unit test passes when `InstallAsync` produces the expected output-folder layout (versioned
`{Id}.{Version}/` or flat `{Id}/`, per the `excludeVersion` flag) containing the extracted
package contents, creates the output directory when it does not already exist, and writes the
expected skip status message to the `Context` for packages whose output folder already
exists.

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
