# NuGet Installer

[![GitHub forks][badge-forks]][link-forks]
[![GitHub stars][badge-stars]][link-stars]
[![GitHub contributors][badge-contributors]][link-contributors]
[![License][badge-license]][link-license]
[![Build][badge-build]][link-build]
[![Quality Gate][badge-quality]][link-quality]
[![Security][badge-security]][link-security]
[![NuGet][badge-nuget]][link-nuget]

NuGet Installer is a cross-platform .NET tool that installs NuGet packages listed in a
`packages.config` file into a local output directory, mirroring the behavior of `nuget.exe install`.

## Use Case

NuGet packages are a convenient distribution mechanism for pre-built libraries, headers, native
binaries, and other assets — not just for .NET projects. NuGetInstaller lets any build system
consume NuGet packages by adding a simple pre-build step that resolves and extracts packages into
a local directory.

**Examples:**

- **CMake / Make / Meson** — install C/C++ headers and native libraries from NuGet, then
  reference `./packages/SomeLib/` in your build scripts
- **Python or shell build scripts** — provision platform-specific binaries or assets from a
  shared `packages.config` before invoking the main build
- **CI/CD pipelines** — reproduce the same package layout on Windows, Linux, and macOS agents
  from a single version-controlled `packages.config`

```bash
# Add as a pre-build step — works identically on Windows, Linux, and macOS
nuget-installer packages.config -o ./packages -x
```

## Features

- 📦 **Package Installation** — Install NuGet packages listed in a `packages.config` file
- 📁 **Output Directory** — Specify a custom output directory with `-o`/`-OutputDirectory`
- 🏷️ **Version-less Folders** — Optionally strip the version suffix from output folder names
- ⏭️ **Skip Existing** — Re-running skips packages whose output folder already exists
- ⚙️ **Standard CLI** — Shared options for help, version, validation, and logging
- ✅ **Self-Validation** — Built-in qualification tests with TRX and JUnit output
- 🌐 **Multi-Platform** — Runs on Windows, Linux, and macOS on .NET 8, 9, and 10
- 🛡️ **Continuous Compliance** — Compliance evidence generated automatically on every CI run

## Installation

Install the tool globally using the .NET CLI:

```bash
dotnet tool install -g DemaConsulting.NuGetInstaller
```

## Usage

```bash
# Install packages from packages.config into current directory
nuget-installer packages.config

# Install into a specific output directory
nuget-installer packages.config -o ./packages

# Use {Id}/ folder naming instead of {Id}.{Version}/
nuget-installer -x packages.config

# Display version
nuget-installer --version

# Display help
nuget-installer --help

# Run self-validation
nuget-installer --validate

# Save validation results
nuget-installer --validate --results results.trx

# Set heading depth for embedded validation output
nuget-installer --validate --depth 2

# Silent mode with logging
nuget-installer --silent --log output.log
```

## Command-Line Options

| Option                          | Description                                                  |
| ------------------------------- | ------------------------------------------------------------ |
| `[packages.config]`             | Path to packages.config (default: packages.config)           |
| `-x`, `-ExcludeVersion`         | Name output folder {Id}/ instead of {Id}.{Version}/          |
| `-o`, `-OutputDirectory <dir>`  | Output directory (default: current directory)                |
| `-v`, `--version`               | Display version information                                  |
| `-?`, `-h`, `--help`            | Display help message                                         |
| `--silent`                      | Suppress console output                                      |
| `--validate`                    | Run self-validation                                          |
| `--results <file>`              | Write validation results to file (TRX or JUnit format)       |
| `--depth <#>`                   | Set heading depth for markdown output (default: 1)           |
| `--log <file>`                  | Write output to log file                                     |

## Self Validation

Running self-validation produces a report containing the following information:

```text
# DEMA Consulting NuGet Installer

| Information         | Value                                              |
| :------------------ | :------------------------------------------------- |
| Tool Version        | <version>                                          |
| Machine Name        | <machine-name>                                     |
| OS Version          | <os-version>                                       |
| DotNet Runtime      | <dotnet-runtime-version>                           |
| Time Stamp          | <timestamp> UTC                                    |

✓ NuGetInstaller_VersionDisplay - Passed
✓ NuGetInstaller_HelpDisplay - Passed
✓ NuGetInstaller_InstallPackage - Passed

Total Tests: 3
Passed: 3
Failed: 0
```

Each test in the report proves:

- **`NuGetInstaller_VersionDisplay`** - `--version` outputs a valid version string.
- **`NuGetInstaller_HelpDisplay`** - `--help` outputs usage and options information.
- **`NuGetInstaller_InstallPackage`** - Installs a NuGet package from a packages.config file.

Use `--depth <#>` to control the heading level of the validation output (default: `1`).
This is useful when embedding validation output into a larger markdown document:

```bash
# Embed validation at heading level 2
nuget-installer --validate --depth 2
```

See the [User Guide][link-guide] for more details on the self-validation tests.

On validation failure the tool will exit with a non-zero exit code.

## Documentation

Generated documentation includes:

- **Build Notes**: Release information and changes
- **User Guide**: Comprehensive usage documentation
- **Code Quality Report**: CodeQL and SonarCloud analysis results
- **Requirements**: Functional and non-functional requirements
- **Requirements Justifications**: Detailed requirement rationale
- **Trace Matrix**: Requirements to test traceability

## Contributing

Contributions are welcome. See [CONTRIBUTING.md][link-contributing] for development setup information
and contribution guidelines.

## License

Copyright (c) DEMA Consulting. Licensed under the MIT License. See [LICENSE][link-license] for details.

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

<!-- Badge References -->
[badge-forks]: https://img.shields.io/github/forks/demaconsulting/NuGetInstaller?style=plastic
[badge-stars]: https://img.shields.io/github/stars/demaconsulting/NuGetInstaller?style=plastic
[badge-contributors]: https://img.shields.io/github/contributors/demaconsulting/NuGetInstaller?style=plastic
[badge-license]: https://img.shields.io/github/license/demaconsulting/NuGetInstaller?style=plastic
[badge-build]: https://img.shields.io/github/actions/workflow/status/demaconsulting/NuGetInstaller/build_on_push.yaml?style=plastic
[badge-quality]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_NuGetInstaller&metric=alert_status
[badge-security]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_NuGetInstaller&metric=security_rating
[badge-nuget]: https://img.shields.io/nuget/v/DemaConsulting.NuGetInstaller?style=plastic

<!-- Link References -->
[link-contributing]: https://github.com/demaconsulting/NuGetInstaller/blob/main/CONTRIBUTING.md
[link-forks]: https://github.com/demaconsulting/NuGetInstaller/network/members
[link-stars]: https://github.com/demaconsulting/NuGetInstaller/stargazers
[link-contributors]: https://github.com/demaconsulting/NuGetInstaller/graphs/contributors
[link-license]: https://github.com/demaconsulting/NuGetInstaller/blob/main/LICENSE
[link-build]: https://github.com/demaconsulting/NuGetInstaller/actions/workflows/build_on_push.yaml
[link-quality]: https://sonarcloud.io/dashboard?id=demaconsulting_NuGetInstaller
[link-security]: https://sonarcloud.io/dashboard?id=demaconsulting_NuGetInstaller
[link-nuget]: https://www.nuget.org/packages/DemaConsulting.NuGetInstaller
[link-guide]: https://github.com/demaconsulting/NuGetInstaller/blob/main/docs/user_guide/introduction.md
