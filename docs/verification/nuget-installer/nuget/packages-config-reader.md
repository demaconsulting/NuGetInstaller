### PackagesConfigReader Unit Verification

#### Verification Strategy

The `PackagesConfigReader` unit is verified using xUnit unit tests in
`PackagesConfigReaderTests.cs`. Each test writes a fixture XML file to a temporary directory,
calls `PackagesConfigReader.Read`, and asserts on the returned collection or the thrown
exception type. Tests cover valid single and multiple packages, an empty packages element,
and all documented error conditions.

#### Test Environment

Tests require a writable OS temporary directory (`Path.GetTempPath()`) in which fixture
`packages.config` files are written and subsequently deleted during teardown. No network
access or external services are required; tests run under the standard xUnit test runner.

#### Acceptance Criteria

A unit test passes when `Read` returns an `IReadOnlyList<PackageEntry>` containing the
expected entries (correct `Id`, `Version`, and `TargetFramework` values, in declaration
order) for valid input, or throws the documented exception type
(`InvalidOperationException` for a missing file or missing required attribute;
`XmlException` for malformed XML) for each documented error condition.

#### Test Scenarios

##### Parse Scenario

Tests verify correct parsing for the common cases: a single package, multiple packages, and
an empty packages element.

Test methods:

- `PackagesConfigReader_Read_SinglePackage_ReturnsSingleEntry` — asserts a file with one
  `<package>` element returns a list with one entry whose `Id` and `Version` are correct
- `PackagesConfigReader_Read_MultiplePackages_ReturnsAllEntries` — asserts a file with multiple
  `<package>` elements returns all entries in declaration order
- `PackagesConfigReader_Read_EmptyPackages_ReturnsEmptyList` — asserts a file with no
  `<package>` elements returns an empty list

##### File Not Found Scenario

Tests verify that a clear `InvalidOperationException` is thrown when the specified file does
not exist.

Test method:

- `PackagesConfigReader_Read_FileNotFound_ThrowsInvalidOperationException`

##### Missing Attributes Scenario

Tests verify that missing `id` or `version` attributes on a `<package>` element throw an
`InvalidOperationException`.

Test methods:

- `PackagesConfigReader_Read_MissingIdAttribute_ThrowsInvalidOperationException`
- `PackagesConfigReader_Read_MissingVersionAttribute_ThrowsInvalidOperationException`

##### Malformed XML Scenario

Tests verify that malformed XML throws an `XmlException`.

Test method:

- `PackagesConfigReader_Read_MalformedXml_ThrowsXmlException`

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-PackagesConfigReader-Parse` | `PackagesConfigReader_Read_SinglePackage_ReturnsSingleEntry`, `PackagesConfigReader_Read_MultiplePackages_ReturnsAllEntries`, `PackagesConfigReader_Read_EmptyPackages_ReturnsEmptyList` |
| `NuGetInstaller-PackagesConfigReader-FileNotFound` | `PackagesConfigReader_Read_FileNotFound_ThrowsInvalidOperationException` |
| `NuGetInstaller-PackagesConfigReader-MissingAttributes` | `PackagesConfigReader_Read_MissingIdAttribute_ThrowsInvalidOperationException`, `PackagesConfigReader_Read_MissingVersionAttribute_ThrowsInvalidOperationException` |
| `NuGetInstaller-PackagesConfigReader-MalformedXml` | `PackagesConfigReader_Read_MalformedXml_ThrowsXmlException` |
