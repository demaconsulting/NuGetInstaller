### Context Unit Verification

#### Verification Approach

The Context unit is verified using xUnit unit tests in `ContextTests.cs`. Each test
constructs a `Context` from a specific string array of command-line arguments and asserts
on the resulting property values, console output behavior, and log file behavior. Tests
that verify error output replace the standard output and error streams with `StringWriter`
instances before constructing the `Context`.

No external dependencies are required. Log file tests write to temporary file paths and
clean up in test teardown.

#### Test Environment

Tests run under the standard xUnit test runner with no external services. Log file tests
create files under the OS temporary directory and delete them during test teardown. Console
`Out` and `Error` are redirected to `StringWriter` instances for the duration of each test
and restored afterward. No network access is required.

#### Acceptance Criteria

A unit test passes when the `Context` property values, thrown exceptions, console output,
and log file contents match the behavior documented for the corresponding command-line
argument combination. Unit verification is considered complete when every recognized flag,
its documented aliases, and each malformed-argument path have at least one passing test.

#### Test Scenarios

##### Argument Parsing Scenarios

Tests verify that each recognized flag and positional argument is parsed correctly into the
corresponding `Context` property. Boundary values include zero arguments (default context),
each flag in isolation, combined flags, and the full set of new arguments together.

Test methods:

- `Context_Create_NoArguments_ReturnsDefaultContext`
- `Context_Create_VersionFlag_SetsVersionTrue`
- `Context_Create_ShortVersionFlag_SetsVersionTrue`
- `Context_Create_HelpFlag_SetsHelpTrue`
- `Context_Create_ShortHelpFlag_H_SetsHelpTrue`
- `Context_Create_ShortHelpFlag_Question_SetsHelpTrue`
- `Context_Create_SilentFlag_SetsSilentTrue`
- `Context_Create_ValidateFlag_SetsValidateTrue`
- `Context_Create_ResultsFlag_SetsResultsFile`
- `Context_Create_LogFlag_OpensLogFile`
- `Context_Create_ResultAliasFlag_SetsResultsFile`
- `Context_Create_DepthFlag_SetsHeadingDepth`
- `Context_Create_NoDepthFlag_ReturnsDefaultHeadingDepth`
- `Context_Create_PositionalArgument_SetsPackagesConfigFile`
- `Context_Create_ExcludeVersionShortFlag_SetsExcludeVersionTrue`
- `Context_Create_ExcludeVersionLongFlag_SetsExcludeVersionTrue`
- `Context_Create_OutputDirectoryShortFlag_SetsOutputDirectory`
- `Context_Create_OutputDirectoryLongFlag_SetsOutputDirectory`
- `Context_Create_AllNewArguments_SetsAllProperties`

##### Invalid Argument Scenarios

Tests verify that unrecognized or malformed arguments throw an `ArgumentException` with a
descriptive message. Each test targets a distinct error path: unknown flag, flag with missing
value, and depth flag with an invalid, zero, or out-of-range integer value.

Test methods:

- `Context_Create_UnknownArgument_ThrowsArgumentException`
- `Context_Create_LogFlag_WithoutValue_ThrowsArgumentException`
- `Context_Create_ResultsFlag_WithoutValue_ThrowsArgumentException`
- `Context_Create_ResultAliasFlag_WithoutValue_ThrowsArgumentException`
- `Context_Create_DepthFlag_WithoutValue_ThrowsArgumentException`
- `Context_Create_DepthFlag_NonIntegerValue_ThrowsArgumentException`
- `Context_Create_DepthFlag_ZeroValue_ThrowsArgumentException`
- `Context_Create_DepthFlag_ExceedsMaxValue_ThrowsArgumentException`
- `Context_Create_OutputDirectoryFlag_WithoutValue_ThrowsArgumentException`

##### Console Output Scenarios

Tests verify that `WriteLine` writes to the console when silent is false and suppresses
output when silent is true.

Test methods:

- `Context_WriteLine_NotSilent_WritesToConsole`
- `Context_WriteLine_Silent_DoesNotWriteToConsole`

##### Log File Output Scenarios

Tests verify that `Context` opens a log file when `--log` is supplied and that `WriteError`
also writes to the log file.

Test methods:

- `Context_Create_LogFlag_OpensLogFile`
- `Context_WriteError_WritesToLogFile`

##### Silent Mode Scenarios

Tests verify that `--silent` sets the `Silent` property to `true` and that both `WriteLine`
and `WriteError` suppress console output when silent is enabled.

Test methods:

- `Context_Create_SilentFlag_SetsSilentTrue`
- `Context_WriteLine_Silent_DoesNotWriteToConsole`
- `Context_WriteError_Silent_DoesNotWriteToConsole`

##### Error Output Scenarios

Tests verify that `WriteError` writes to the console when silent is false and sets the error
exit code.

Test methods:

- `Context_WriteError_NotSilent_WritesToConsole`
- `Context_WriteError_SetsErrorExitCode`

##### Exit Code Scenario

Tests verify that calling `WriteError` causes `ExitCode` to become non-zero.

Test method:

- `Context_WriteError_SetsErrorExitCode`

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Context-ArgumentParsing` | `Context_Create_NoArguments_ReturnsDefaultContext`, `Context_Create_VersionFlag_SetsVersionTrue`, `Context_Create_ShortVersionFlag_SetsVersionTrue`, `Context_Create_HelpFlag_SetsHelpTrue`, `Context_Create_ShortHelpFlag_H_SetsHelpTrue`, `Context_Create_ShortHelpFlag_Question_SetsHelpTrue`, `Context_Create_SilentFlag_SetsSilentTrue`, `Context_Create_ValidateFlag_SetsValidateTrue`, `Context_Create_ResultsFlag_SetsResultsFile`, `Context_Create_LogFlag_OpensLogFile`, `Context_Create_ResultAliasFlag_SetsResultsFile`, `Context_Create_DepthFlag_SetsHeadingDepth`, `Context_Create_NoDepthFlag_ReturnsDefaultHeadingDepth`, `Context_Create_PositionalArgument_SetsPackagesConfigFile`, `Context_Create_ExcludeVersionShortFlag_SetsExcludeVersionTrue`, `Context_Create_ExcludeVersionLongFlag_SetsExcludeVersionTrue`, `Context_Create_OutputDirectoryShortFlag_SetsOutputDirectory`, `Context_Create_OutputDirectoryLongFlag_SetsOutputDirectory`, `Context_Create_AllNewArguments_SetsAllProperties` |
| `NuGetInstaller-Context-InvalidArgs` | `Context_Create_UnknownArgument_ThrowsArgumentException`, `Context_Create_LogFlag_WithoutValue_ThrowsArgumentException`, `Context_Create_ResultsFlag_WithoutValue_ThrowsArgumentException`, `Context_Create_ResultAliasFlag_WithoutValue_ThrowsArgumentException`, `Context_Create_DepthFlag_WithoutValue_ThrowsArgumentException`, `Context_Create_DepthFlag_NonIntegerValue_ThrowsArgumentException`, `Context_Create_DepthFlag_ZeroValue_ThrowsArgumentException`, `Context_Create_DepthFlag_ExceedsMaxValue_ThrowsArgumentException`, `Context_Create_OutputDirectoryFlag_WithoutValue_ThrowsArgumentException` |
| `NuGetInstaller-Context-ConsoleOutput` | `Context_WriteLine_NotSilent_WritesToConsole`, `Context_WriteLine_Silent_DoesNotWriteToConsole` |
| `NuGetInstaller-Context-LogFileOutput` | `Context_Create_LogFlag_OpensLogFile`, `Context_WriteError_WritesToLogFile` |
| `NuGetInstaller-Context-Silent` | `Context_Create_SilentFlag_SetsSilentTrue`, `Context_WriteLine_Silent_DoesNotWriteToConsole`, `Context_WriteError_Silent_DoesNotWriteToConsole` |
| `NuGetInstaller-Context-ErrorOutput` | `Context_WriteError_NotSilent_WritesToConsole`, `Context_WriteError_SetsErrorExitCode` |
| `NuGetInstaller-Context-ExitCode` | `Context_WriteError_SetsErrorExitCode` |
