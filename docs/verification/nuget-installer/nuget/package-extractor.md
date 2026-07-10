### PackageExtractor Unit Verification

#### Verification Strategy

The `PackageExtractor` unit is verified using xUnit unit tests in `PackageExtractorTests.cs`.
Tests build real `.zip`/`.nupkg` archives on disk using `System.IO.Compression.ZipArchive`
and `ZipFile`, written to temporary file paths under the OS temp directory, then call
`PackageExtractor.Extract` and assert on the return value, extracted file content, or thrown
exception type. The zip-bomb scenario writes an actual archive whose entry decompresses to
approximately 1.1 GiB on disk (exceeding the 1 GB limit), so this test creates a genuinely
large temporary file rather than a small or in-memory fixture. All tests use temporary
directories that are cleaned up in a `finally` block after each test.

#### Test Environment

Tests require a writable OS temporary directory (`Path.GetTempPath()`) with enough free disk
space to hold the zip-bomb fixture (in excess of 1 GB, plus the compressed archive itself).
No network access or external services are required. Tests run in the standard xUnit test
runner with no additional mocking.

#### Acceptance Criteria

A unit test passes when: `Extract` returns `true` and the expected file(s) exist in the
destination folder for valid archives; `Extract` returns `false` and leaves the destination
folder unmodified when it already exists; directory-marker entries are skipped without error;
and `Extract` throws `InvalidOperationException` — with a message containing `"zip-slip"` or
`"zip-bomb"` as appropriate — for a traversal entry or an oversized decompressed payload.

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
