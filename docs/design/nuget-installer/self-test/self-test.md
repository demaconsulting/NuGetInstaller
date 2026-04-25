# SelfTest Subsystem

The `SelfTest` subsystem provides the self-validation framework for the NuGet Installer.
It runs a built-in suite of tests to demonstrate the tool is functioning correctly in the
deployment environment.

## Overview

The `SelfTest` subsystem is invoked when the user passes `--validate` on the command line.
It exercises the tool's own capabilities and reports a pass/fail summary. It can also write
test results to a file in TRX or JUnit XML format for integration with CI/CD pipelines.

## Units

The `SelfTest` subsystem contains the following software unit:

| Unit         | File                     | Responsibility                                     |
|--------------|--------------------------|----------------------------------------------------|
| `Validation` | `SelfTest/Validation.cs` | Orchestrating and executing self-validation tests. |

## Interfaces

The `SelfTest` subsystem exposes the following interface to the rest of the tool:

| Interface        | Direction | Description                                                           |
|------------------|-----------|-----------------------------------------------------------------------|
| `Validation.Run` | Outbound  | Runs all self-validation tests, prints a summary, and writes results. |

## Self-Tests

The `SelfTest` subsystem executes three built-in tests covering the tool's core behaviors:

| Test Name                          | Behavior Verified                                     |
|------------------------------------|-------------------------------------------------------|
| `NuGetInstaller_VersionDisplay`    | `--version` outputs a valid version string.           |
| `NuGetInstaller_HelpDisplay`       | `--help` outputs usage and options information.       |
| `NuGetInstaller_InstallPackage`    | Installs a known package from a packages.config file. |

Each test runner creates a temporary directory via the private `TemporaryDirectory` helper (an
`IDisposable` wrapper that deletes the directory on disposal). Results are accumulated into a
`DemaConsulting.TestResults.TestResults` object. The output file format is determined by the
extension of `context.ResultsFile`: `.trx` produces TRX (MSTest) format; `.xml` produces JUnit
XML format.

## Error Handling

Individual test methods catch all exceptions from the `try` block and record the exception
message as the test failure reason in the shared `TestResults` object before continuing to the
next test. This broad catch strategy is intentional: the self-validation framework must remain
resilient to unexpected failures in any single test so that the remaining tests still execute
and produce evidence.

## Interactions

| Dependency                   | Direction | Purpose                                                      |
|------------------------------|-----------|--------------------------------------------------------------|
| `Context`                    | Uses      | Output channel for header lines, test summaries, and errors. |
| `Program`                    | Uses      | `Program.Run` is called internally to exercise the tool.     |
| `PathHelpers`                | Uses      | `SafePathCombine` for constructing log file paths in tests.  |
| `PackagesConfigReader`       | Uses      | Reads packages.config in the install-package self-test.      |
| `PackageInstaller`           | Uses      | Installs packages in the install-package self-test.          |
| `DemaConsulting.TestResults` | Uses      | Result model and TRX/JUnit serialization.                    |
