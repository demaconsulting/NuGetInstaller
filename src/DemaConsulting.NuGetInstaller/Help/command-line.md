---
title: Command-line options
summary: Every option of the tool, the order of a run, and the exit codes.
aliases:
  - options
  - flags
  - arguments
  - usage
  - exit-codes
tags:
  - reference
order: 2
see-also:
  - packages-config
  - output-folders
  - self-validation
examples:
  - name: usage
    kind: command
    language: shell
    description: Print the short usage text. It ends with a pointer to this help.
    content: '{{tool}} --help'
    expect: "Run '{{tool}} help' for topics and examples."
  - name: silent-log
    kind: command
    language: shell
    description: Hide the console output and keep it in a log file.
    content: '{{tool}} --silent --log output.log --version'
  - name: unknown-option
    kind: command
    language: shell
    description: An unknown argument is an error, printed even with --silent. Exits 1.
    content: '{{tool}} --silent --unknown'
    exit: 1
    expect: "Error: Unsupported argument '--unknown'"
  - name: missing-value
    kind: command
    language: shell
    description: An option without its value is an error. Exits 1.
    content: '{{tool}} -o'
    exit: 1
    expect: 'Error: -o requires a directory argument'
---
`{{tool}} [packages.config] [options]` installs the packages that a
packages.config file lists. An argument that does not start with `-` is
the path of that file; without one, the tool reads `packages.config` in
the current folder. When several paths are given, the last one is used.

| Option | Meaning |
| --- | --- |
| `-o`, `-OutputDirectory <dir>` | Install into this folder; default the current folder. |
| `-x`, `-ExcludeVersion` | Name each package folder `{Id}`, not `{Id}.{Version}`. |
| `-v`, `--version` | Print only the version. |
| `-?`, `-h`, `--help` | Print the short usage text. |
| `--silent` | Hide console output. Errors still set exit code 1. |
| `--log <file>` | Also write the output to a file. |
| `--validate` | Run the self-tests; see [[self-validation]]. |
| `--results <file>` | Write the self-test results as `.trx` or `.xml`. |
| `--depth <n>` | First heading level of the self-test report, 1 to 6. |

Write the options exactly as shown: they are case-sensitive, and
`-o`, `-OutputDirectory`, `-x`, and `-ExcludeVersion` take one dash.
`--result` is the same as `--results`, and the default `--depth` is 1.
See [[output-folders]] for `-o` and `-x`.

**Order.** `--version`, when all arguments are valid, prints only the
version and skips everything else. Without it, the tool prints a banner,
then handles the first of `--help`, `--validate`, or the install.
The command that [[#silent-log]] prints writes the version to the log
file only.

**Errors.** An unknown argument, as in [[#unknown-option]],
or an option without its value, as in [[#missing-value]],
prints `Error: ...` and exits 1. These errors are found before `--silent`
and `--log` take effect, so they are always printed and never logged.

**Exit codes.** 0 means success. 1 means an error: a wrong argument, a log
file that cannot be opened, a missing or invalid packages.config, a
package or version that no source has, or a failed self-test. 2 means a
wrong use of `help`, such as an unknown topic. A malformed file can end
with a stack trace and another exit code; see [[troubleshooting]].

**Help.** `help` is a command, not an option, and must be the first
argument, in any letter case. `--silent` and `--log` do not apply to it,
and an unknown topic exits 2. To install from a packages.config file
named `help`, give its path as `./help`.
