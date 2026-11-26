---
title: Self-validation
summary: Prove that the installed tool works, and save the evidence as test results.
aliases:
  - validate
  - qualification
tags:
  - reference
order: 6
see-also:
  - command-line
  - package-sources
examples:
  - name: run
    kind: snippet
    language: shell
    save-as: validate.sh
    description: Run the self-tests; they need nuget.org.
    content: |
      {{tool}} --validate
  - name: trx
    kind: snippet
    language: shell
    save-as: trx.sh
    description: Also write the results as a TRX file.
    content: |
      {{tool}} --validate --results validation.trx
  - name: junit
    kind: snippet
    language: shell
    save-as: junit.sh
    description: Write the results as JUnit XML instead.
    content: |
      {{tool}} --validate --results validation.xml
  - name: depth
    kind: snippet
    language: shell
    save-as: depth.sh
    description: Start the report at heading level 2.
    content: |
      {{tool}} --validate --depth 2
  - name: bad-extension
    kind: command
    language: shell
    description: Any other extension is an error. Exits 1.
    content: '{{tool}} --validate --results validation.txt'
    exit: 1
    expect: 'Unsupported results file format'
---
`--validate` runs a short set of tests of the tool itself in a temporary
folder, so a regulated project can show that the installed version works
on its build machine. The command that [[#run]] prints
runs them.

**Network.** One test installs the package `DemaConsulting.NuGet.Caching`
1.0.0, so the tests need nuget.org, or a nuget.config in the current
folder with a source that has this package. They also pass offline when
the package is already in the NuGet global packages folder.

**Tests.** `NuGetInstaller_VersionDisplay` checks the version output,
`NuGetInstaller_HelpDisplay` checks the usage text, and
`NuGetInstaller_InstallPackage` installs the package. These names are
stable, so a requirements file can list them as the tests that prove the
tool works in its environment.

**Report.** The output is Markdown. It starts with a heading and a table
of the tool version, machine name, operating system, .NET runtime, and
time stamp. Each test prints one line that says whether it passed, with
the reason when it failed, and the report ends with `Total Tests`,
`Passed`, and `Failed` lines. The exit code is 0 only when every test
passes. `--depth` sets the level of the first heading, as
[[#depth]] shows.

**Results files.** `--results <file>` also writes the outcome; the
extension chooses the format. `.trx` writes a Visual Studio TRX file, as
in [[#trx]],
and `.xml` writes JUnit XML, as in [[#junit]];
CI test-report tasks read both. Any other extension is an error after
the tests ran, as in [[#bad-extension]].
The folder of the file must exist.
