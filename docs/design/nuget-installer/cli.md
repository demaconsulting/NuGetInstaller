## Cli Subsystem

![Cli Structure](CliView.svg)

The `Cli` subsystem provides the command-line interface for the NuGet Installer.
It is responsible for accepting user input from the command line and routing output to
the console and an optional log file.

### Overview

The `Cli` subsystem acts as the primary boundary between the user's shell invocation and
the tool's internal logic. It owns argument parsing, output formatting, and error tracking.
All other subsystems receive a `Context` object from the `Cli` subsystem to read parsed
flags and write output.

### Units

The `Cli` subsystem contains the following software unit:

| Unit      | File             | Responsibility                                    |
|-----------|------------------|---------------------------------------------------|
| `Context` | `Cli/Context.cs` | Argument parsing, output channels, and exit code. |

### Interfaces

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
| `Context.HeadingDepth`       | Outbound  | Heading depth for markdown output (default 1, valid range 1–6).        |
| `Context.Dispose`            | Outbound  | Releases resources held by the context (flushes and closes log file).  |

### Design

The `Cli` subsystem contains a single unit, `Context`, so internal collaboration is limited
to how `Context` is constructed and then consumed by the rest of the tool:

- `Program.Main` calls `Context.Create(args)` once at startup. `Create` parses the raw
  `string[]` argument array into a nested `ArgumentParser` helper type, validates each flag
  (raising `ArgumentException` for unknown flags or missing required values), and then
  copies the parsed values into an immutable `Context` instance using `init`-only properties.
- If `--log <file>` was supplied, `Create` opens the log file before returning the `Context`,
  so all subsequent output methods can write to both the console and the log file without
  re-checking whether logging is enabled.
- Once constructed, the single `Context` instance is passed by reference to every other
  subsystem (`Program`, `SelfTest`, `NuGet`) that needs to read parsed flags or produce
  output. No subsystem re-parses arguments or constructs its own `Context`.
- `Context.WriteLine` and `Context.WriteError` centralize all console and log output so that
  the `--silent` flag and log-file redirection are honored consistently regardless of which
  subsystem is writing. `WriteError` additionally sets the internal error flag consumed by
  `Context.ExitCode`.
- `Context` implements `IDisposable` so the log file handle opened during `Create` is
  flushed and closed deterministically via a `using` statement in `Program.Main`.

### Command-Line Flags

The following table maps each command-line flag to its corresponding `Context` property:

| Flag(s)                               | Context Property             | Notes                                    |
|---------------------------------------|------------------------------|------------------------------------------|
| `-v`, `--version`                     | `Context.Version`            | Boolean flag                             |
| `-?`, `-h`, `--help`                  | `Context.Help`               | Boolean flag                             |
| `--silent`                            | `Context.Silent`             | Boolean flag                             |
| `--validate`                          | `Context.Validate`           | Boolean flag                             |
| `--log <file>`                        | *(log file path)*            | Opened internally; not exposed directly  |
| `--results <file>`, `--result <file>` | `Context.ResultsFile`        | `--result` is an alias for `--results`   |
| `--depth <#>`                         | `Context.HeadingDepth`       | Integer 1–6, default 1                   |
| `-o <dir>`, `-OutputDirectory <dir>`  | `Context.OutputDirectory`    | Null means current working directory     |
| `-x`, `-ExcludeVersion`               | `Context.ExcludeVersion`     | Boolean flag                             |
| `[packages.config]`                   | `Context.PackagesConfigFile` | Positional; default `packages.config`    |

### Error Handling

When `Context.Create` encounters an unknown or malformed command-line argument it throws
`ArgumentException` with a descriptive message identifying the offending argument.
`Program.Main` catches this exception, writes `Error: <message>` to stderr, and returns
exit code 1.

Additional error cases handled by `Context.Create`:

- **Missing required value**: If a flag that requires a value (e.g., `--results`, `--log`,
  `--depth`, `-o`) is the last argument with no following value, `ArgumentException` is
  thrown with a message identifying the flag and what is required.
- **Log file open failure**: If the file path provided to `--log` cannot be created or
  opened (e.g., directory does not exist, insufficient permissions), `Context.Create`
  throws `InvalidOperationException`. `Program.Main` catches this and writes
  `Error: <message>` to stderr, returning exit code 1.
- **`--silent` and error output**: `--silent` suppresses all console output, including
  error messages written through `Context.WriteError`. When `--silent` is active,
  errors are recorded internally (setting `ExitCode` to 1) and written to the log file
  if one is open, but are not written to stderr. This is an intentional exception to the
  general rule that errors are written to stderr.

### Interactions

The `Cli` subsystem has no dependencies on other tool subsystems. It uses only .NET base
class library types. The `Program` unit at system level creates the `Context` and passes it
to all subsystems that need to produce output.
