## Program Unit Verification

### Verification Strategy

The Program unit is verified using xUnit unit tests in `ProgramTests.cs`. Tests call the
`Program.Run` static method directly, or `Program.Main` for the `help` command, which is
answered before a `Context` exists; console output is captured by redirecting `Console.Out`
to a `StringWriter` before creating the `Context`. Temporary directories and fixture
`packages.config` files are created on disk where the install path requires real files. No
external network access is required because all package resolution uses the local NuGet cache.

### Test Environment

Tests require a writable OS temporary directory for fixture `packages.config` files and
install output folders, and rely on the local NuGet package cache for extraction (no network
access is required). Tests that exercise the current-working-directory default path
temporarily change `Environment.CurrentDirectory` and restore it during teardown. Tests run
under the standard xUnit test runner with no additional mocking.

### Acceptance Criteria

A unit test passes when `Program.Run` returns the documented exit code (`0` for success
paths, non-zero when a required file is missing), prints the expected version, help, or
banner text to the captured console output for the corresponding flag, and — for install
scenarios — the expected packages are extracted to the output directory.

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

- `Program_Run_WithHelpFlag_DisplaysUsageInformation` — asserts `--help` prints usage text,
  including the `nuget-installer help [topic]` usage line, and ends with the pointer to the
  help command
- `Program_Run_WithShortHelpFlag_QuestionMark_DisplaysUsageInformation` — asserts `-?` prints
  usage text
- `Program_Run_WithShortHelpFlag_H_DisplaysUsageInformation` — asserts `-h` prints usage text

#### Help Command Scenario

Tests verify that `Program.Main` answers the `help` command before the options are parsed:
invoked with `["help"]`, which is not an option, it prints the help topic index without the
banner and returns exit code 0.

Test method:

- `Program_Main_WithHelpVerb_PrintsHelpIndex` — asserts the topic index is printed, the banner
  is not, and the exit code is 0

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
packages into the output directory, that the default no-flag path installs from a
`packages.config` file in the current working directory, and that the banner is displayed
when no arguments are provided.

Test methods:

- `Program_Run_WithValidPackagesConfig_InstallsPackages` — asserts packages are extracted when
  a well-formed `packages.config` is supplied
- `Program_Run_WithNoFlags_InstallsPackagesFromDefaultConfigInCurrentDirectory` — asserts
  packages are extracted when no flags are provided and a `packages.config` file exists in the
  current working directory, exercising the documented default execution path
- `Program_Run_NoArguments_DisplaysBanner` — asserts the banner is shown when no arguments are
  provided

### Coverage Summary

- `NuGetInstaller-Program-Version`:
  - `Program_Run_WithVersionFlag_DisplaysVersionOnly`
  - `Program_Run_WithShortVersionFlag_DisplaysVersion`
  - `Program_Version_ReturnsNonEmptyString`
- `NuGetInstaller-Program-Help`:
  - `Program_Run_WithHelpFlag_DisplaysUsageInformation`
  - `Program_Run_WithShortHelpFlag_QuestionMark_DisplaysUsageInformation`
  - `Program_Run_WithShortHelpFlag_H_DisplaysUsageInformation`
- `NuGetInstaller-Program-HelpCommand`:
  - `Program_Main_WithHelpVerb_PrintsHelpIndex`
- `NuGetInstaller-Program-HelpFooter`:
  - `Program_Run_WithHelpFlag_DisplaysUsageInformation`
- `NuGetInstaller-Program-Validate`:
  - `Program_Run_WithValidateFlag_RunsValidation`
- `NuGetInstaller-Program-ExitCode`:
  - `Program_Run_WithMissingPackagesConfig_ReturnsNonZeroExitCode`
- `NuGetInstaller-Program-Install`:
  - `Program_Run_WithValidPackagesConfig_InstallsPackages`
  - `Program_Run_WithNoFlags_InstallsPackagesFromDefaultConfigInCurrentDirectory`
  - `Program_Run_NoArguments_DisplaysBanner`
