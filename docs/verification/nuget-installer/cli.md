## Cli Subsystem Verification

### Verification Strategy

The Cli subsystem is verified using xUnit integration tests in `CliSubsystemTests.cs`. Each
test constructs a `Context` from a specific set of command-line arguments and then calls
`Program.Run`, asserting on the combined observable behavior: console output captured via a
`StringWriter`, exit code, and (where applicable) file system state. The tests operate at the
subsystem boundary — validating the interaction between argument parsing (`Context`) and
dispatch logic (`Program.Run`) — without mocking any internal components.

### Test Scenarios

#### Version Flow Scenario

Tests verify that version-related flags (`--version`, `-v`) cause `Context` to set the version
flag and `Program.Run` to print the version string and exit.

Test methods:

- `CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits` — uses `--version`
- `CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits_WithShortVFlag` — uses `-v`

#### Help Flow Scenario

Tests verify that help flags (`--help`, `-?`, `-h`) cause help text to be printed and the tool
to exit.

Test methods:

- `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits`
- `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortQuestionFlag`
- `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortHFlag`

#### Silent Flow Scenario

Tests verify that `--silent` suppresses all console output.

Test method:

- `CliSubsystem_SilentFlow_ContextAndProgram_SuppressesOutput`

#### Validate Flow Scenario

Tests verify that `--validate` triggers self-validation and that the tool exits cleanly.

Test method:

- `CliSubsystem_ValidateFlow_ContextAndProgram_RunsValidationAndExits`

#### Results Flow Scenario

Tests verify that `--results` and its alias `--result` cause the validation output to be
written to the specified file.

Test methods:

- `CliSubsystem_ResultsFlow_ContextAndProgram_WritesResultsFile`
- `CliSubsystem_ResultAliasFlow_ContextAndProgram_WritesResultsFile`

#### Depth Flow Scenario

Tests verify that `--depth` controls the Markdown heading depth of validation output.

Test method:

- `CliSubsystem_DepthFlow_ContextAndProgram_AdjustsHeadingDepth`

#### Log Flow Scenario

Tests verify that `--log` routes tool output to the specified log file.

Test method:

- `CliSubsystem_LogFlow_ContextAndProgram_WritesLogFile`

#### Error Output Scenario

Tests verify that error messages are written to stderr.

Test method:

- `CliSubsystem_ErrorOutput_ContextAndProgram_WritesErrorToStderr`

#### Invalid Args Scenario

Tests verify that unknown arguments cause a descriptive error and a non-zero exit code.

Test method:

- `CliSubsystem_InvalidArgs_ContextAndProgram_RejectsUnknownArgumentsAndExitsNonZero`

#### Packages Config Scenario

Tests verify that a positional argument sets the `packages.config` file path.

Test method:

- `CliSubsystem_PackagesConfigFlow_ContextAndProgram_AcceptsPositionalArgument`

#### Output Directory Scenario

Tests verify that `-o` and `-OutputDirectory` set the output directory.

Test methods:

- `CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory`
- `CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory_WithLongFlag`

#### Exclude Version Scenario

Tests verify that `-x` and `-ExcludeVersion` set the exclude-version flag.

Test methods:

- `CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion`
- `CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion_WithLongFlag`

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Cli-Context` | `CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits`, `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits`, `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortQuestionFlag`, `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortHFlag`, `CliSubsystem_ValidateFlow_ContextAndProgram_RunsValidationAndExits` |
| `NuGetInstaller-Cli-OutputManagement` | `CliSubsystem_SilentFlow_ContextAndProgram_SuppressesOutput`, `CliSubsystem_LogFlow_ContextAndProgram_WritesLogFile` |
| `NuGetInstaller-Cli-Version` | `CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits`, `CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits_WithShortVFlag` |
| `NuGetInstaller-Cli-Help` | `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits`, `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortQuestionFlag`, `CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortHFlag` |
| `NuGetInstaller-Cli-Silent` | `CliSubsystem_SilentFlow_ContextAndProgram_SuppressesOutput` |
| `NuGetInstaller-Cli-Validate` | `CliSubsystem_ValidateFlow_ContextAndProgram_RunsValidationAndExits` |
| `NuGetInstaller-Cli-Results` | `CliSubsystem_ResultsFlow_ContextAndProgram_WritesResultsFile` |
| `NuGetInstaller-Cli-ResultAlias` | `CliSubsystem_ResultAliasFlow_ContextAndProgram_WritesResultsFile` |
| `NuGetInstaller-Cli-Depth` | `CliSubsystem_DepthFlow_ContextAndProgram_AdjustsHeadingDepth` |
| `NuGetInstaller-Cli-Log` | `CliSubsystem_LogFlow_ContextAndProgram_WritesLogFile` |
| `NuGetInstaller-Cli-ErrorOutput` | `CliSubsystem_ErrorOutput_ContextAndProgram_WritesErrorToStderr` |
| `NuGetInstaller-Cli-InvalidArgs` | `CliSubsystem_InvalidArgs_ContextAndProgram_RejectsUnknownArgumentsAndExitsNonZero` |
| `NuGetInstaller-Cli-ExitCode` | `CliSubsystem_InvalidArgs_ContextAndProgram_RejectsUnknownArgumentsAndExitsNonZero` |
| `NuGetInstaller-Cli-PackagesConfig` | `CliSubsystem_PackagesConfigFlow_ContextAndProgram_AcceptsPositionalArgument` |
| `NuGetInstaller-Cli-OutputDirectory` | `CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory`, `CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory_WithLongFlag` |
| `NuGetInstaller-Cli-ExcludeVersion` | `CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion`, `CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion_WithLongFlag` |
