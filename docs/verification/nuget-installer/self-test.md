## SelfTest Subsystem Verification

### Verification Strategy

The SelfTest subsystem is verified using xUnit integration tests in
`SelfTestSubsystemTests.cs`. Tests invoke `Validation.Run` through a silent `Context` (created
with the `--silent` flag, which suppresses console output) and assert on the exit code and
(where applicable) the content of TRX or JUnit XML result files written to a temporary
directory. Console output is not captured or asserted by these tests.

### Test Environment

Tests run under the standard xUnit runner with no external services. Each test that writes a
results file creates a uniquely named file under the OS temporary directory
(`Path.GetTempPath()`) and deletes it during teardown, so tests do not depend on or leave
behind fixed file-system state.

### Acceptance Criteria

A subsystem test passes when `Validation.Run` returns exit code `0` and — for scenarios that
request a results file — the expected result file exists and contains the expected root XML
element (`<TestRun` for TRX, `<testsuites` for JUnit).

### Test Scenarios

#### Qualification Scenario

Tests verify that the self-validation suite completes successfully with exit code 0 when no
result files are requested. This scenario establishes baseline qualification evidence for the
tool in the current deployment environment.

Test method:

- `SelfTestSubsystem_ValidationWorkflow_NoResultFiles_CompletesSuccessfully`

#### TRX Output Scenario

Tests verify that a TRX file is written when a `.trx` results path is supplied.

Test method:

- `SelfTestSubsystem_ValidationWorkflow_WithTrxFile_GeneratesResults` — asserts the TRX file
  exists and contains expected XML elements

#### JUnit Output Scenarios

Tests verify that a JUnit XML file is written when an `.xml` results path is supplied, and
that both the TRX and JUnit paths independently produce correct output when each is run as a
separate `Validation.Run` invocation.

Test methods:

- `SelfTestSubsystem_ValidationWorkflow_WithJUnitFile_GeneratesResults` — asserts the JUnit
  XML file exists and contains expected XML elements
- `SelfTestSubsystem_ValidationWorkflow_WithBothResultFiles_GeneratesBothResults` — runs
  `Validation.Run` twice, once with a `.trx` results path and once with an `.xml` results
  path, and asserts each run generates its own correctly formatted result file

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-SelfTest-Qualification` | `SelfTestSubsystem_ValidationWorkflow_NoResultFiles_CompletesSuccessfully` |
| `NuGetInstaller-SelfTest-TrxOutput` | `SelfTestSubsystem_ValidationWorkflow_WithTrxFile_GeneratesResults` |
| `NuGetInstaller-SelfTest-JUnitOutput` | `SelfTestSubsystem_ValidationWorkflow_WithJUnitFile_GeneratesResults`, `SelfTestSubsystem_ValidationWorkflow_WithBothResultFiles_GeneratesBothResults` |
