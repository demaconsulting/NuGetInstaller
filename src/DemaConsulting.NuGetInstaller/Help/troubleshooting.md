---
title: Troubleshooting
summary: What the common error messages mean, and how to fix them.
aliases:
  - errors
  - problems
  - faq
tags:
  - troubleshooting
order: 7
see-also:
  - packages-config
  - package-sources
  - command-line
examples:
  - name: no-file
    kind: command
    language: shell
    description: Run in a folder without packages.config. Exits 1.
    content: '{{tool}}'
    exit: 1
    expect: 'packages.config not found: packages.config'
  - name: missing-list
    kind: source
    language: xml
    save-as: missing-package.config
    description: A list with a package that no source has.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <packages>
        <package id="Example.Missing" version="9.9.9" />
      </packages>
  - name: missing-package
    kind: command
    language: shell
    description: Install a package that no source has. Exits 1.
    content: '{{tool}} missing-package.config'
    exit: 1
    expect: 'was not found in any configured NuGet source'
  - name: range-list
    kind: source
    language: xml
    save-as: range.config
    description: A list with a version range instead of a version.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <packages>
        <package id="Example.Tools" version="[2.0.0,3.0.0)" />
      </packages>
  - name: range
    kind: command
    language: shell
    description: Install from a list with a version range. Exits 1.
    content: '{{tool}} range.config'
    exit: 1
    expect: 'is not a valid version string'
---
Each paragraph starts with the message that the tool prints.

**`packages.config not found: <path>`.** The file does not exist, as in
[[#no-file]]. Run the tool in the folder of the file, or name the file.
The message says packages.config whatever the file is named.

**`Package '<id>' version '<version>' was not found in any configured
NuGet source.`** No source has this package version, as in
[[#missing-package]]. Check the id and the version, the
nuget.config files that the tool reads, the package source mapping, and
the credentials of a private feed; see [[package-sources]]. This message
is printed even with `--silent` and is not logged.

**`'<version>' is not a valid version string.`** The version is a range
or a floating version, as in [[#range]]. Write the exact
version; see [[packages-config]]. This message is printed even with
`--silent` and is not logged.

**`Package element missing 'id' attribute`.** A `package` element has no
`id` (or no `version`). Add it.

**`Unexpected error:` and a stack trace.** The packages.config file is
not valid XML, or a version is not in its normal form, such as `2.0` or
`2.0.0.0` for `2.0.0`. Fix the file. In this case the exit code is not 1.

**`Skipping <id> <version> (already exists)`.** The package folder exists
from an earlier run. With `-x` a changed version is not installed until
you delete the folder; see [[output-folders]].

**`Unsupported argument`** or **`requires`.** An option is misspelled or
has no value. Options are case-sensitive; see [[command-line]].

**`Failed to open log file`.** The folder of the `--log` file must exist.

**`NuGetInstaller_InstallPackage - FAILED`.** The self-tests could not
download their test package; see [[self-validation]].

**An unknown help topic.** `{{tool}} help <topic>` exits 2 and names the
topic. Run `{{tool}} help` for the list of topics.
