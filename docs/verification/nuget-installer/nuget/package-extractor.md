### PackageExtractor Unit Verification

#### Verification Strategy

The `PackageExtractor` unit is verified using MSTest unit tests in `PackageExtractorTests.cs`.
Tests construct minimal `.zip` archive fixtures in memory and write them to temporary file
paths, then call `PackageExtractor.Extract` and assert on the return value, extracted file
content, or thrown exception type. All tests use temporary directories that are cleaned up in
test teardown.

#### Test Scenarios

##### Extraction Scenario

Tests verify that a valid `.nupkg` archive is extracted to the destination folder and that the
method returns `true`.

Test method:

- `PackageExtractor_Extract_NewDestination_ExtractsAndReturnsTrue` — asserts file contents are
  extracted correctly and the return value is `true`

##### Zip-Slip Protection Scenario

Tests verify that an archive entry whose path escapes the destination folder is rejected with
an `InvalidOperationException` before any file is written.

Test method:

- `PackageExtractor_Extract_ZipSlipEntry_ThrowsInvalidOperationException` — uses an entry
  with a `../` traversal sequence in its name

##### Skip Existing Scenario

Tests verify that extraction is skipped and `false` is returned when the destination folder
already exists.

Test method:

- `PackageExtractor_Extract_DestinationExists_ReturnsFalse` — pre-creates the destination
  directory and asserts `false` is returned without modifying it

##### Zip-Bomb Protection Scenario

Tests verify that an archive whose total decompressed content exceeds 1 GB is rejected with
an `InvalidOperationException`.

Test method:

- `PackageExtractor_Extract_ZipBombEntry_ThrowsInvalidOperationException` — uses an entry
  whose reported uncompressed size exceeds the 1 GB limit

##### Skip Directory Entries Scenario

Tests verify that archive entries that are directory markers (entries with an empty `Name`
component) are skipped and the remaining file entries are extracted successfully.

Test method:

- `PackageExtractor_Extract_ArchiveWithDirectoryEntries_SkipsDirectoriesAndReturnsTrue`

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-PackageExtractor-Extract` | `PackageExtractor_Extract_NewDestination_ExtractsAndReturnsTrue` |
| `NuGetInstaller-PackageExtractor-ZipSlip` | `PackageExtractor_Extract_ZipSlipEntry_ThrowsInvalidOperationException` |
| `NuGetInstaller-PackageExtractor-SkipExisting` | `PackageExtractor_Extract_DestinationExists_ReturnsFalse` |
| `NuGetInstaller-PackageExtractor-ZipBomb` | `PackageExtractor_Extract_ZipBombEntry_ThrowsInvalidOperationException` |
| `NuGetInstaller-PackageExtractor-SkipDirectoryEntries` | `PackageExtractor_Extract_ArchiveWithDirectoryEntries_SkipsDirectoriesAndReturnsTrue` |
