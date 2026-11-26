---
title: The packages.config file
summary: The file that lists the packages to install, each with an exact version.
aliases:
  - packages
  - config
  - package-list
  - versions
tags:
  - reference
order: 3
see-also:
  - package-sources
  - output-folders
  - troubleshooting
examples:
  - name: full
    kind: source
    language: xml
    save-as: packages.config
    description: A packages.config file that lists two packages.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <packages>
        <package id="Example.Tools" version="2.0.0" />
        <package id="Example.Headers" version="1.2.0" targetFramework="native" />
      </packages>
  - name: install
    kind: command
    language: shell
    description: Install both packages into the folder packages.
    content: '{{tool}} packages.config -o packages'
    expect: |
      Found 2 package(s) in packages.config
      Installed Example.Tools 2.0.0
      Installed Example.Headers 1.2.0
  - name: other-name
    kind: command
    language: shell
    description: Name another file. A missing file exits 1.
    content: '{{tool}} missing.config'
    exit: 1
    expect: 'packages.config not found: missing.config'
---
A packages.config file lists the packages to install, one `package`
element each, as [[#full]] shows. The tool reads `packages.config` in
the current folder, or the file that the command line names, as in
[[#other-name]].

**Attributes.** Every `package` element needs `id` and `version`; a
missing one is the error `Package element missing 'id' attribute` (or
`'version'`), and the tool exits 1. Other attributes, such as
`targetFramework`, are ignored. The name of the root element is not
checked.

**Versions.** Write the exact version in its normal form, such as
`2.0.0` or `1.0.0-beta.1`. A version range such as `[2.0.0,3.0.0)` or a
floating version such as `2.*` is not a valid version string, and the
tool exits 1. A version that is not in its normal form, such as `2.0`
for `2.0.0`, fails; see [[troubleshooting]].

**Dependencies.** Only the listed packages are installed, not the
packages that they depend on. List every package that you need.

**Output.** The command that [[#install]] prints
reports `Found 2 package(s) in packages.config`, then one line per
package. The packages install in parallel, so the order of these lines
can change from run to run. See [[output-folders]] for the folders.

**Missing file.** When the file does not exist, the tool exits 1 and
reports `packages.config not found: <path>`, whatever the file name.
`--silent` hides this message and `--log` records it.
