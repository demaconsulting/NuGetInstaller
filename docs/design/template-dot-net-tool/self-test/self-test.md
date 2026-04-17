# SelfTest Subsystem

<!-- TODO: This is an example design section for the SelfTest subsystem. Replace with your own subsystem design. -->

The `SelfTest` subsystem provides the self-validation framework for the Template DotNet Tool.
It runs a built-in suite of tests to demonstrate the tool is functioning correctly in the
deployment environment.

## Overview

<!-- TODO: Fill in for your project -->

The `SelfTest` subsystem is invoked when the user passes `--validate` on the command line.
It exercises the tool's own capabilities and reports a pass/fail summary. It can also write
test results to a file in TRX or JUnit XML format for integration with CI/CD pipelines.

## Units

<!-- TODO: Fill in for your project -->

The `SelfTest` subsystem contains the following software unit:

| Unit         | File                     | Responsibility                                     |
|--------------|--------------------------|----------------------------------------------------|
| `Validation` | `SelfTest/Validation.cs` | Orchestrating and executing self-validation tests. |

## Interfaces

<!-- TODO: Fill in for your project -->

The `SelfTest` subsystem exposes the following interface to the rest of the tool:

| Interface        | Direction | Description                                                           |
|------------------|-----------|-----------------------------------------------------------------------|
| `Validation.Run` | Outbound  | Runs all self-validation tests, prints a summary, and writes results. |

## Interactions

<!-- TODO: Fill in for your project -->

| Dependency    | Direction | Purpose                                                      |
|---------------|-----------|--------------------------------------------------------------|
| `Context`     | Uses      | Output channel for header lines, test summaries, and errors. |
| `Program`     | Uses      | `Program.Run` is called internally to exercise the tool.     |
| `PathHelpers` | Uses      | `SafePathCombine` for constructing log file paths in tests.  |
