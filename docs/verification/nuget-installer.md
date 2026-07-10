# NuGet Installer System Verification

## System Verification Strategy

The NuGet Installer is verified at the system level using integration tests that invoke the
published `dotnet` tool end-to-end. Tests are written using xUnit in `IntegrationTests.cs` and
exercise the complete tool — from command-line argument parsing through package extraction — in
a temporary directory. No mocking or stubbing is used at the system level; tests exercise the
actual binary on the actual file system.

## Test Environment

System integration tests run in the CI/CD pipeline on all three supported platforms (Windows,
Linux, and macOS) under all three supported .NET runtimes (.NET 8, .NET 9, .NET 10). Each test
creates a temporary working directory, writes a `packages.config` fixture, invokes the tool,
and asserts on the exit code, console output, file system state, or log files as appropriate.

The NuGet global cache on the CI runner supplies package archives for extraction tests.

## Acceptance Criteria

A system test scenario passes when all of the following hold:

- The process exit code matches the expected value for the scenario (`0` for successful
  operations such as version display, help display, self-validation, and package installation;
  non-zero for error conditions such as unrecognized arguments, a missing `packages.config`
  file, or malformed configuration XML).
- Captured stdout/stderr contains the expected content for the scenario (for example, a
  non-empty version string, the usage banner, an `Error:`/`Unexpected error:` prefixed
  diagnostic message, or a package-installation confirmation), and contains no unexpected
  error text for scenarios that are expected to succeed.
- Any expected file system side effect occurs: the package output folder is created with a
  non-empty content set for successful installs, the specified log file is created and
  populated when `--log` is used, and the specified TRX/JUnit results file is created and
  contains well-formed XML with the expected root elements when `--results` is used.
- No unintended side effects occur: `--silent` produces no console output, `--skip-existing`
  behavior leaves pre-existing package folders untouched and reports the skip, and error
  scenarios do not leave partially-extracted package folders behind.

A system test scenario fails if any of the above conditions is not met. The overall system
verification passes only when every scenario in the sections below passes on all three
supported platforms (Windows, Linux, macOS) and all three supported .NET runtimes (.NET 8,
.NET 9, .NET 10).

## System Test Scenarios

### Version Display Scenario

Verifies that the tool prints version information and exits with code 0 when `--version` is
passed. The test captures stdout and asserts it contains a non-empty version string.

Test method: `NuGetInstaller_VersionFlag_FlagProvided_OutputsVersion`

### Help Display Scenario

Verifies that the tool prints usage information and exits with code 0 when `--help` is passed.
The test captures stdout and asserts it contains the expected option descriptions.

Test method: `NuGetInstaller_HelpFlag_FlagProvided_OutputsUsageInformation`

### Validate Scenario

Verifies that the tool runs its self-validation suite when `--validate` is passed. The test
asserts a zero exit code and that the summary output contains expected test result text.

Test method: `NuGetInstaller_ValidateFlag_FlagProvided_RunsValidation`

### Validate TRX Results Scenario

Verifies that the tool writes a TRX file when `--validate --results output.trx` is passed.
The test asserts the file exists and contains expected XML content.

Test method: `NuGetInstaller_ValidateWithResults_TrxExtension_GeneratesTrxFile`

### Validate JUnit Results Scenario

Verifies that the tool writes a JUnit XML file when `--validate --results output.xml` is
passed. The test asserts the file exists and contains expected XML content.

Test method: `NuGetInstaller_ValidateWithResults_XmlExtension_GeneratesJUnitFile`

### Silent Mode Scenario

Verifies that the tool produces no console output when `--silent` is passed. The test captures
stdout and asserts it is empty.

Test method: `NuGetInstaller_SilentFlag_FlagProvided_SuppressesOutput`

### Log File Scenario

Verifies that the tool writes its output to the specified log file when `--log` is passed.
The test asserts that the log file exists and contains expected content.

Test method: `NuGetInstaller_LogFlag_FlagProvided_WritesOutputToFile`

### Invalid Argument Exit Code Scenario

Verifies that the tool returns a non-zero exit code when an unrecognized argument is passed.

Test method: `NuGetInstaller_UnknownArgument_InvalidFlag_ReturnsNonZeroExitCode`

### Invalid Argument Error Message Scenario

Verifies that the tool reports a descriptive error message to stderr when an unrecognized
argument is passed.

Test method: `NuGetInstaller_UnknownArgument_InvalidFlag_ReportsErrorMessage`

### Output Directory Scenario

Verifies that packages are extracted to the directory specified by `--output-directory`. The
test asserts the expected package folder appears inside the specified path.

Test method: `NuGetInstaller_InstallPackages_ValidConfig_ExtractsPackageToOutputDirectory`

### Default Output Directory Scenario

Verifies that packages are installed to the current working directory when `--output-directory`
is not specified.

Test method: `NuGetInstaller_InstallPackages_DefaultOutputDirectory_InstallsToCurrentDirectory`

### Exclude Version Scenario

Verifies that packages use `{Id}/` folder naming (without the version segment) when
`--exclude-version` is passed.

Test method: `NuGetInstaller_InstallPackages_ExcludeVersionFlag_UsesFlatFolderNaming`

### Heading Depth Scenario

Verifies that the Markdown heading depth of validation output is adjusted when `--depth` is
passed.

Test method: `NuGetInstaller_ValidateDepth_DepthFlagProvided_AdjustsHeadingDepth`

### Skip Existing Scenario

Verifies that the tool skips packages whose output folder already exists and reports the skip.

Test method: `NuGetInstaller_InstallPackages_ExistingFolder_SkipsInstallation`

### Package Install Scenario

Verifies that the tool installs packages from a valid `packages.config` and extracts them into
the output directory.

Test method: `NuGetInstaller_InstallPackages_ValidConfig_ExtractsPackageToOutputDirectory`

### Missing Configuration File Scenario

Verifies that the tool reports a not-found error and returns a non-zero exit code when the
`packages.config` file specified on the command line does not exist. The test invokes the
published tool against a nonexistent file path and asserts on the exit code and stderr content.

Test method: `NuGetInstaller_InstallPackages_MissingConfigFile_ReportsErrorAndReturnsNonZeroExitCode`

### Malformed Configuration File Scenario

Verifies that the tool reports an error and returns a non-zero exit code when the
`packages.config` file contains malformed XML. The test writes an incomplete/invalid XML
fixture, invokes the published tool against it, and asserts on the exit code and stderr content.

Test method: `NuGetInstaller_InstallPackages_MalformedConfigFile_ReportsErrorAndReturnsNonZeroExitCode`

## Platform Test Scenarios

Platform requirements are verified by running the self-validation tests on each platform and
runtime. The CI pipeline runs the tool on Windows, Linux (Ubuntu), and macOS, and under
.NET 8, .NET 9, and .NET 10. The test names below include a source-filter prefix identifying
the required platform or runtime.

### Windows Platform Scenario

Test methods: `windows@NuGetInstaller_VersionDisplay`, `windows@NuGetInstaller_HelpDisplay`

### Linux Platform Scenario

Test methods: `ubuntu@NuGetInstaller_VersionDisplay`, `ubuntu@NuGetInstaller_HelpDisplay`

### macOS Platform Scenario

Test methods: `macos@NuGetInstaller_VersionDisplay`, `macos@NuGetInstaller_HelpDisplay`

### .NET 8 Runtime Scenario

Test methods: `dotnet8.x@NuGetInstaller_VersionDisplay`, `dotnet8.x@NuGetInstaller_HelpDisplay`

### .NET 9 Runtime Scenario

Test methods: `dotnet9.x@NuGetInstaller_VersionDisplay`, `dotnet9.x@NuGetInstaller_HelpDisplay`

### .NET 10 Runtime Scenario

Test methods: `dotnet10.x@NuGetInstaller_VersionDisplay`, `dotnet10.x@NuGetInstaller_HelpDisplay`

## Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-System-Version` | `NuGetInstaller_VersionFlag_FlagProvided_OutputsVersion` |
| `NuGetInstaller-System-Help` | `NuGetInstaller_HelpFlag_FlagProvided_OutputsUsageInformation` |
| `NuGetInstaller-System-Validate` | `NuGetInstaller_ValidateFlag_FlagProvided_RunsValidation`, `NuGetInstaller_ValidateWithResults_TrxExtension_GeneratesTrxFile`, `NuGetInstaller_ValidateWithResults_XmlExtension_GeneratesJUnitFile` |
| `NuGetInstaller-System-Silent` | `NuGetInstaller_SilentFlag_FlagProvided_SuppressesOutput` |
| `NuGetInstaller-System-Log` | `NuGetInstaller_LogFlag_FlagProvided_WritesOutputToFile` |
| `NuGetInstaller-System-ErrorHandling-ExitCode` | `NuGetInstaller_UnknownArgument_InvalidFlag_ReturnsNonZeroExitCode` |
| `NuGetInstaller-System-ErrorHandling-Report` | `NuGetInstaller_UnknownArgument_InvalidFlag_ReturnsNonZeroExitCode`, `NuGetInstaller_UnknownArgument_InvalidFlag_ReportsErrorMessage` |
| `NuGetInstaller-System-ErrorHandling-MissingFile` | `NuGetInstaller_InstallPackages_MissingConfigFile_ReportsErrorAndReturnsNonZeroExitCode` |
| `NuGetInstaller-System-ErrorHandling-MalformedXml` | `NuGetInstaller_InstallPackages_MalformedConfigFile_ReportsErrorAndReturnsNonZeroExitCode` |
| `NuGetInstaller-System-OutputDirectory` | `NuGetInstaller_InstallPackages_ValidConfig_ExtractsPackageToOutputDirectory`, `NuGetInstaller_InstallPackages_DefaultOutputDirectory_InstallsToCurrentDirectory` |
| `NuGetInstaller-System-ExcludeVersion` | `NuGetInstaller_InstallPackages_ExcludeVersionFlag_UsesFlatFolderNaming` |
| `NuGetInstaller-System-Depth` | `NuGetInstaller_ValidateDepth_DepthFlagProvided_AdjustsHeadingDepth` |
| `NuGetInstaller-System-SkipExisting` | `NuGetInstaller_InstallPackages_ExistingFolder_SkipsInstallation` |
| `NuGetInstaller-System-Install` | `NuGetInstaller_InstallPackages_ValidConfig_ExtractsPackageToOutputDirectory` |
| `NuGetInstaller-System-CrossPlatform` | `windows@NuGetInstaller_VersionDisplay`, `ubuntu@NuGetInstaller_VersionDisplay`, `macos@NuGetInstaller_VersionDisplay` |
| `NuGetInstaller-Platform-Windows` | `windows@NuGetInstaller_VersionDisplay`, `windows@NuGetInstaller_HelpDisplay` |
| `NuGetInstaller-Platform-Linux` | `ubuntu@NuGetInstaller_VersionDisplay`, `ubuntu@NuGetInstaller_HelpDisplay` |
| `NuGetInstaller-Platform-MacOS` | `macos@NuGetInstaller_VersionDisplay`, `macos@NuGetInstaller_HelpDisplay` |
| `NuGetInstaller-Platform-Net8` | `dotnet8.x@NuGetInstaller_VersionDisplay`, `dotnet8.x@NuGetInstaller_HelpDisplay` |
| `NuGetInstaller-Platform-Net9` | `dotnet9.x@NuGetInstaller_VersionDisplay`, `dotnet9.x@NuGetInstaller_HelpDisplay` |
| `NuGetInstaller-Platform-Net10` | `dotnet10.x@NuGetInstaller_VersionDisplay`, `dotnet10.x@NuGetInstaller_HelpDisplay` |
