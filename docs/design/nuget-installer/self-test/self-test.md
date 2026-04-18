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

## Interactions

| Dependency             | Direction | Purpose                                                      |
|------------------------|-----------|--------------------------------------------------------------|
| `Context`              | Uses      | Output channel for header lines, test summaries, and errors. |
| `Program`              | Uses      | `Program.Run` is called internally to exercise the tool.     |
| `PathHelpers`          | Uses      | `SafePathCombine` for constructing log file paths in tests.  |
| `PackagesConfigReader` | Uses      | Reads packages.config in the install-package self-test.      |
| `PackageInstaller`     | Uses      | Installs packages in the install-package self-test.          |
