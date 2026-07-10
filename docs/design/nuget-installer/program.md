## Program

![NuGet Installer System Structure](NuGetInstallerView.svg)

The `Program` class is the main entry point for the NuGet Installer. It creates a `Context`
from command-line arguments, dispatches to the appropriate logic based on the flags, and returns
the exit code.

### Overview

`Program` owns the top-level execution flow. It delegates all argument interpretation to `Context`
and all validation logic to `Validation`. Its own responsibility is limited to reading the flags
that `Context` exposes and calling the correct handler.

### Data Model

`Program` holds no instance state. Its single static property is:

| Field     | Type     | Description                                                    |
|-----------|----------|----------------------------------------------------------------|
| `Version` | `string` | The tool version from `AssemblyInformationalVersionAttribute`. |

### Methods

#### Main(string[] args)

Entry point. Creates a `Context`, calls `Run`, and returns `context.ExitCode`.
Catches `ArgumentException` and `InvalidOperationException` — writes the message to stderr
and returns exit code 1. Catches unexpected exceptions — writes the message to stderr and
re-throws to generate event logs.

**Returns:** `int` — 0 for success, non-zero for failure.

#### PrintBanner(Context context)

Prints the tool name and version banner to the context output. Called by `Run` before
dispatching to `PrintHelp`, `Validation.Run`, or `RunToolLogic`. Not called when the
`Version` flag is set; `--version` returns early before reaching `PrintBanner`.

#### PrintHelp(Context context)

Prints the usage information and available options to the context output.

#### Run(Context context)

Inspects the flags on `context` and dispatches:

- `Version` flag → writes the version string directly via `context.WriteLine(Version)` and returns
  immediately without calling `PrintBanner`.
- Otherwise → calls `PrintBanner` first, then:
  - `Help` flag → calls `PrintHelp` to print usage information and returns.
  - `Validate` flag → calls `Validation.Run(context)`.
  - Otherwise → calls `RunToolLogic(context)` to install packages.

#### RunToolLogic(Context context)

Executes the package installation workflow:

1. Verifies `context.PackagesConfigFile` exists; reports error if not found.
2. Reads packages via `PackagesConfigReader.Read`.
3. Resolves the output directory from `context.OutputDirectory` (defaults to cwd).
4. Calls `PackageInstaller.InstallAsync` to install all packages, forwarding
   `context.ExcludeVersion` to control output folder naming (`{Id}.{Version}/` vs `{Id}/`).

#### Version (property)

Reads `AssemblyInformationalVersionAttribute` from the executing assembly, falling back to
`AssemblyVersion`, then `"0.0.0"`.

### Error Handling

| Condition                                          | Behavior                                                                                                                                                                                                                                                                |
|----------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `packages.config` file not found                   | Calls `context.WriteError`, which sets exit code to 1 and writes to stderr and the log file (if open); suppressed on stderr, but still recorded to the log file and exit code, when `--silent` is active — see the *Cli Subsystem Design*, `--silent` and error output. |
| `ArgumentException` or `InvalidOperationException` | Caught directly in `Main` via `Console.Error.WriteLine`; always written to stderr regardless of `--silent` (this path runs outside `Context`, either before a `Context` exists or independent of its output gating), and exits with code 1.                             |
| Unexpected exceptions                              | Caught directly in `Main` via `Console.Error.WriteLine`; always written to stderr regardless of `--silent`; re-thrown to generate event logs.                                                                                                                           |
| Unknown or malformed command-line argument         | `ArgumentException` caught in `Main`; `Error: <message>` always written to stderr, independent of `--silent`.                                                                                                                                                           |
| Log file cannot be opened                          | `InvalidOperationException` caught in `Main`; `Error: <message>` always written to stderr, independent of `--silent`.                                                                                                                                                   |

`Main`'s own catch blocks write directly to `Console.Error` and are never suppressed by
`--silent`; only errors reported through `Context.WriteError` (such as the missing
`packages.config` case) are subject to `--silent` suppression, consistent with the
`--silent` and error output behavior documented in the *Cli Subsystem Design*.

### Interactions

| Dependency             | Direction | Purpose                                          |
|------------------------|-----------|--------------------------------------------------|
| `Context`              | Uses      | Reads flags; calls `WriteLine`/`WriteError`      |
| `Validation`           | Uses      | Calls `Validation.Run` when validate flag is set |
| `PackagesConfigReader` | Uses      | Reads packages.config in `RunToolLogic`          |
| `PackageInstaller`     | Uses      | Installs packages in `RunToolLogic`              |
