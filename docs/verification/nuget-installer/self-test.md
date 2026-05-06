## SelfTest Subsystem Verification

### Verification Strategy

The SelfTest subsystem is verified using MSTest integration tests in
`SelfTestSubsystemTests.cs`. Tests invoke `Validation.Run` through a silent `Context` and
assert on the exit code, console output, and (where applicable) the content of TRX or JUnit
XML result files written to a temporary directory.

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
that both TRX and JUnit files are written correctly when both paths are provided.

Test methods:

- `SelfTestSubsystem_ValidationWorkflow_WithJUnitFile_GeneratesResults` — asserts the JUnit
  XML file exists and contains expected XML elements
- `SelfTestSubsystem_ValidationWorkflow_WithBothResultFiles_GeneratesBothResults` — asserts
  both result files are generated correctly when both paths are specified

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-SelfTest-Qualification` | `SelfTestSubsystem_ValidationWorkflow_NoResultFiles_CompletesSuccessfully` |
| `NuGetInstaller-SelfTest-TrxOutput` | `SelfTestSubsystem_ValidationWorkflow_WithTrxFile_GeneratesResults` |
| `NuGetInstaller-SelfTest-JUnitOutput` | `SelfTestSubsystem_ValidationWorkflow_WithJUnitFile_GeneratesResults`, `SelfTestSubsystem_ValidationWorkflow_WithBothResultFiles_GeneratesBothResults` |
