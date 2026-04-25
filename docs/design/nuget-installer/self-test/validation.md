# Validation

The `Validation` class provides the self-validation framework for the NuGet Installer.
It runs a suite of internal tests that demonstrate the tool is functioning correctly in the
deployment environment.

## Overview

`Validation.Run` prints a header, executes each test, accumulates results into a
`DemaConsulting.TestResults.TestResults` object, prints a summary, and optionally writes
a results file in TRX or JUnit XML format.

## Data Model

`Validation` holds no instance state. All state is local to `Run` and the private test methods.

## Methods

### Run(Context context)

Orchestrates the validation sequence:

1. Calls `PrintValidationHeader` to emit a Markdown heading and a table with tool and
   environment metadata. The heading level is controlled by `context.HeadingDepth`
   (default `1`, producing a `#` heading; `--depth 2` produces `##`, etc.).
2. Constructs a `TestResults` object named `"NuGet Installer Self-Validation"`.
3. Calls each test runner (`RunVersionTest`, `RunHelpTest`, `RunInstallPackageTest`).
4. Prints three aggregate summary lines: `Total Tests: N`, `Passed: N`, `Failed: N`.
   Failed count is written via `WriteError` (red) when non-zero; otherwise via `WriteLine`.
5. Calls `WriteResultsFile` if `context.ResultsFile` is set.

### RunVersionTest / RunHelpTest

Each of these test methods:

1. Creates a temporary directory via `TemporaryDirectory`.
2. Constructs a log-file path using `PathHelpers.SafePathCombine`.
3. Invokes `Program.Run` with the relevant arguments and captures the output log.
4. Validates the output against expected content.
5. Records pass or fail in the shared `TestResults`.

### RunInstallPackageTest

This test method exercises the package installation API directly:

1. Creates a temporary directory via `TemporaryDirectory`.
2. Writes a `packages.config` file referencing a known NuGet package.
3. Reads the package list via `PackagesConfigReader.Read`.
4. Installs the packages via `PackageInstaller.InstallAsync`.
5. Verifies the expected package folder exists and is non-empty.
6. Records pass or fail in the shared `TestResults`.

### WriteResultsFile(Context context, TestResults testResults)

Writes `testResults` to `context.ResultsFile`. The format is determined by the file extension:
`.trx` for TRX (MSTest), `.xml` for JUnit.

## Interactions

| Dependency              | Direction | Purpose                                         |
|-------------------------|-----------|-------------------------------------------------|
| `Context`               | Uses      | Output channel for header and summary lines.    |
| `Program`               | Uses      | `Program.Run` called to exercise the tool.      |
| `PathHelpers`           | Uses      | `SafePathCombine` for temp-dir file paths.      |
| `PackagesConfigReader`  | Uses      | Reads packages.config in install test.          |
| `PackageInstaller`      | Uses      | Installs packages in install test.              |
| `DemaConsulting.TestResults` | Uses | Result model, TRX and JUnit serialization.      |

## Error Handling

Exceptions thrown inside individual test methods (`RunVersionTest`, `RunHelpTest`,
`RunInstallPackageTest`) are caught by a try/catch block inside each runner. The exception
message is recorded as the test failure reason in the shared `TestResults` object, and
execution continues with the next test.
