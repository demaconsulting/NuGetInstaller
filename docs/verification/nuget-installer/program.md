## Program Unit Verification

### Verification Strategy

The Program unit is verified using MSTest unit tests in `ProgramTests.cs`. Tests call the
`Program.Run` static method directly, supplying a `Context` instance whose console output is
captured via a `StringWriter`. Temporary directories and fixture `packages.config` files are
created on disk where the install path requires real files. No external network access is
required because all package resolution uses the local NuGet cache.

### Test Scenarios

#### Version Display Scenario

Tests verify that `--version` and `-v` flags cause `Run` to print the version string and return
exit code 0. The `Program.Version` property is also tested independently to confirm it is
populated from the assembly attributes.

Test methods:

- `Program_Run_WithVersionFlag_DisplaysVersionOnly` — asserts `--version` prints the version
  and returns 0
- `Program_Run_WithShortVersionFlag_DisplaysVersion` — asserts `-v` produces the same result
- `Program_Version_ReturnsNonEmptyString` — asserts the `Version` property is non-empty

#### Help Display Scenario

Tests verify that `--help`, `-h`, and `-?` flags all cause `Run` to print usage information
and return exit code 0.

Test methods:

- `Program_Run_WithHelpFlag_DisplaysUsageInformation` — asserts `--help` prints usage text
- `Program_Run_WithShortHelpFlag_QuestionMark_DisplaysUsageInformation` — asserts `-?` prints
  usage text
- `Program_Run_WithShortHelpFlag_H_DisplaysUsageInformation` — asserts `-h` prints usage text

#### Validate Scenario

Tests verify that the `--validate` flag causes `Run` to invoke the self-validation framework
and return exit code 0.

Test method:

- `Program_Run_WithValidateFlag_RunsValidation` — asserts validation runs and exits cleanly

#### Error Exit Code Scenario

Tests verify that a non-zero exit code is returned when a required file is absent.

Test method:

- `Program_Run_WithMissingPackagesConfig_ReturnsNonZeroExitCode` — passes a path to a
  non-existent `packages.config` and asserts the exit code is non-zero

#### Package Install Scenario

Tests verify that the default execution path reads the package configuration and extracts
packages into the output directory, and that the banner is displayed when no arguments are
provided.

Test methods:

- `Program_Run_WithValidPackagesConfig_InstallsPackages` — asserts packages are extracted when
  a well-formed `packages.config` is supplied
- `Program_Run_NoArguments_DisplaysBanner` — asserts the banner is shown when no arguments are
  provided

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Program-Version` | `Program_Run_WithVersionFlag_DisplaysVersionOnly`, `Program_Run_WithShortVersionFlag_DisplaysVersion`, `Program_Version_ReturnsNonEmptyString` |
| `NuGetInstaller-Program-Help` | `Program_Run_WithHelpFlag_DisplaysUsageInformation`, `Program_Run_WithShortHelpFlag_QuestionMark_DisplaysUsageInformation`, `Program_Run_WithShortHelpFlag_H_DisplaysUsageInformation` |
| `NuGetInstaller-Program-Validate` | `Program_Run_WithValidateFlag_RunsValidation` |
| `NuGetInstaller-Program-ExitCode` | `Program_Run_WithMissingPackagesConfig_ReturnsNonZeroExitCode` |
| `NuGetInstaller-Program-Install` | `Program_Run_WithValidPackagesConfig_InstallsPackages`, `Program_Run_NoArguments_DisplaysBanner` |
