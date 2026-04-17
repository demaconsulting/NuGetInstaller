# Introduction

## Purpose

NuGet Installer is a demonstration project that showcases best practices for DEMA
Consulting DotNet Tools.

## Scope

This user guide covers:

- Installation instructions
- Usage examples for common tasks
- Command-line options reference
- Practical examples for various scenarios

# Continuous Compliance

This template follows the
[Continuous Compliance](https://github.com/demaconsulting/ContinuousCompliance) methodology, which ensures
compliance evidence is generated automatically on every CI run.

## Key Practices

- **Requirements Traceability**: Every requirement is linked to passing tests, and a trace matrix is
  auto-generated on each release
- **Linting Enforcement**: markdownlint, cspell, and yamllint are enforced before any build proceeds
- **Automated Audit Documentation**: Each release ships with generated requirements, justifications,
  trace matrix, and quality reports
- **CodeQL and SonarCloud**: Security and quality analysis runs on every build

# Installation

Install the tool globally using the .NET CLI:

```bash
dotnet tool install -g DemaConsulting.NuGetInstaller
```

# Usage

## Display Version

Display the tool version:

```bash
nuget-installer --version
```

## Display Help

Display usage information:

```bash
nuget-installer --help
```

## Self-Validation

Self-validation produces a report demonstrating that NuGet Installer is functioning
correctly. This is useful in regulated industries where tool validation evidence is required.

### Running Validation

To perform self-validation:

```bash
nuget-installer --validate
```

To save validation results to a file:

```bash
nuget-installer --validate --results results.trx
```

The results file format is determined by the file extension: `.trx` for TRX (MSTest) format,
or `.xml` for JUnit format.

### Heading Depth

Use `--depth <#>` to control the heading level of the validation output (default: `1`).
This is useful when embedding the validation report into a larger markdown document:

```bash
# Embed validation at heading level 2
nuget-installer --validate --depth 2
```

### Validation Report

The validation report contains the tool version, machine name, operating system version,
.NET runtime version, timestamp, and test results.

Example validation report:

```text
# DEMA Consulting NuGet Installer

| Information         | Value                                              |
| :------------------ | :------------------------------------------------- |
| Tool Version        | 1.0.0                                              |
| Machine Name        | BUILD-SERVER                                       |
| OS Version          | Ubuntu 22.04.3 LTS                                 |
| DotNet Runtime      | .NET 10.0.0                                        |
| Time Stamp          | 2024-01-15 10:30:00 UTC                            |

✓ TemplateTool_VersionDisplay - Passed
✓ TemplateTool_HelpDisplay - Passed

Total Tests: 2
Passed: 2
Failed: 0
```

### Validation Tests

Each test proves specific functionality works correctly:

- **`TemplateTool_VersionDisplay`** - `--version` outputs a valid version string.
- **`TemplateTool_HelpDisplay`** - `--help` outputs usage and options information.

## Silent Mode

Suppress console output:

```bash
nuget-installer --silent
```

## Logging

Write output to a log file:

```bash
nuget-installer --log output.log
```

# Command-Line Options

The following command-line options are supported:

| Option               | Description                                                  |
| -------------------- | ------------------------------------------------------------ |
| `-v`, `--version`    | Display version information                                  |
| `-?`, `-h`, `--help` | Display help message                                         |
| `--silent`           | Suppress console output                                      |
| `--validate`         | Run self-validation                                          |
| `--results <file>`   | Write validation results to file (TRX or JUnit format)       |
| `--depth <#>`        | Set heading depth for markdown output (default: 1)           |
| `--log <file>`       | Write output to log file                                     |

# Examples

## Example 1: Basic Usage

```bash
nuget-installer
```

## Example 2: Self-Validation with Results

```bash
nuget-installer --validate --results validation-results.trx
```

## Example 3: Silent Mode with Logging

```bash
nuget-installer --silent --log tool-output.log
```
