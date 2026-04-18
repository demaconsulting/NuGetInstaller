# Cli Subsystem

The `Cli` subsystem provides the command-line interface for the NuGet Installer.
It is responsible for accepting user input from the command line and routing output to
the console and an optional log file.

## Overview

The `Cli` subsystem acts as the primary boundary between the user's shell invocation and
the tool's internal logic. It owns argument parsing, output formatting, and error tracking.
All other subsystems receive a `Context` object from the `Cli` subsystem to read parsed
flags and write output.

## Units

The `Cli` subsystem contains the following software unit:

| Unit      | File             | Responsibility                                    |
|-----------|------------------|---------------------------------------------------|
| `Context` | `Cli/Context.cs` | Argument parsing, output channels, and exit code. |

## Interfaces

The `Cli` subsystem exposes the following interface to the rest of the tool:

| Interface                    | Direction | Description                                                            |
|------------------------------|-----------|------------------------------------------------------------------------|
| `Context.Create`             | Outbound  | Factory method constructing a `Context` from `string[] args`.          |
| `Context.Version`            | Outbound  | Indicates whether the version flag was specified.                      |
| `Context.Help`               | Outbound  | Indicates whether the help flag was specified.                         |
| `Context.Silent`             | Outbound  | Indicates whether the silent flag was specified.                       |
| `Context.Validate`           | Outbound  | Indicates whether the validate flag was specified.                     |
| `Context.ResultsFile`        | Outbound  | The validation results file path (null when not specified).            |
| `Context.PackagesConfigFile` | Outbound  | Path to the packages.config file (default: "packages.config").         |
| `Context.OutputDirectory`    | Outbound  | Output directory for package installation (null means current dir).    |
| `Context.ExcludeVersion`     | Outbound  | Indicates whether to use {Id}/ naming instead of {Id}.{Version}/.      |
| `Context.WriteLine`          | Outbound  | Writes a message to console and optional log file.                     |
| `Context.WriteError`         | Outbound  | Writes an error to stderr and sets the error exit code.                |
| `Context.ExitCode`           | Outbound  | Returns 0 for success or 1 when errors have been reported.             |
| `Context.HeadingDepth`       | Outbound  | Heading depth for markdown output (default 1); supplied via `--depth`. |
| `Context.Dispose`            | Outbound  | Releases resources held by the context (flushes and closes log file).  |

## Error Handling

When `Context.Create` encounters an unknown or malformed command-line argument it throws
`ArgumentException` with a descriptive message identifying the offending argument.
`Program.Main` catches this exception, writes `Error: <message>` to stderr, and returns
exit code 1.

## Interactions

The `Cli` subsystem has no dependencies on other tool subsystems. It uses only .NET base
class library types. The `Program` unit at system level creates the `Context` and passes it
to all subsystems that need to produce output.
