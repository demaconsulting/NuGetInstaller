### Validation Unit Verification

#### Verification Strategy

The Validation unit is verified using xUnit unit tests in `ValidationTests.cs`. Tests call
`Validation.Run` with a silent `Context` and optional temporary result file paths, then assert
on the exit code, the presence of expected summary text in the context output buffer, and the
content of any generated result files. All temporary files and directories are cleaned up in
test teardown.

#### Test Scenarios

##### Run Scenario

Tests verify that `Validation.Run` completes, prints a summary, and leaves the exit code at 0.

Test methods:

- `Validation_Run_WithSilentContext_PrintsSummary` — asserts summary text appears in captured
  output
- `Validation_Run_WithSilentContext_ExitCodeIsZero` — asserts exit code is 0 after a
  successful run

##### Version Test Scenario

Tests verify that the self-validation suite exercises the version display functionality.

Test method:

- `Validation_Run_WithSilentContext_PrintsSummary` — the printed summary includes the version
  test result

##### Help Test Scenario

Tests verify that the self-validation suite exercises the help display functionality.

Test method:

- `Validation_Run_WithSilentContext_PrintsSummary` — the printed summary includes the help
  test result

##### TRX Results Scenario

Tests verify that a `.trx` result file is written when a path with a `.trx` extension is
supplied.

Test method:

- `Validation_Run_WithTrxResultsFile_WritesTrxFile` — asserts the file exists and contains
  expected TRX XML content

##### JUnit Results Scenario

Tests verify that a JUnit XML result file is written when a path with an `.xml` extension is
supplied.

Test method:

- `Validation_Run_WithXmlResultsFile_WritesXmlFile` — asserts the file exists and contains
  expected JUnit XML content

##### Install Package Test Scenario

Tests verify that the self-validation suite includes a test that installs a known package.

Test method:

- `Validation_Run_WithSilentContext_PrintsInstallPackageTestResult` — asserts the install
  package test result appears in the captured summary output

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Validation-Run` | `Validation_Run_WithSilentContext_PrintsSummary`, `Validation_Run_WithSilentContext_ExitCodeIsZero` |
| `NuGetInstaller-Validation-VersionTest` | `Validation_Run_WithSilentContext_PrintsSummary` |
| `NuGetInstaller-Validation-HelpTest` | `Validation_Run_WithSilentContext_PrintsSummary` |
| `NuGetInstaller-Validation-TrxResults` | `Validation_Run_WithTrxResultsFile_WritesTrxFile` |
| `NuGetInstaller-Validation-JUnitResults` | `Validation_Run_WithXmlResultsFile_WritesXmlFile` |
| `NuGetInstaller-Validation-InstallPackage` | `Validation_Run_WithSilentContext_PrintsInstallPackageTestResult` |
