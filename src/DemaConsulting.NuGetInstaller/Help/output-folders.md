---
title: Output folders and re-runs
summary: Where each package is installed, and what a second run does.
aliases:
  - output
  - output-directory
  - exclude-version
  - layout
  - skip
tags:
  - reference
order: 5
see-also:
  - command-line
  - packages-config
examples:
  - name: list
    kind: source
    language: xml
    save-as: packages.config
    description: A packages.config file that lists one package.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <packages>
        <package id="Example.Tools" version="2.0.0" />
      </packages>
  - name: install
    kind: command
    language: shell
    description: Install into packages/Example.Tools.2.0.0.
    content: '{{tool}} packages.config -o packages'
    expect: 'Installed Example.Tools 2.0.0'
  - name: again
    kind: command
    language: shell
    description: Run it again. The existing folder is kept.
    content: '{{tool}} packages.config -o packages'
    expect: 'Skipping Example.Tools 2.0.0 (already exists)'
  - name: flat
    kind: command
    language: shell
    description: Install into flat/Example.Tools, without the version.
    content: '{{tool}} packages.config -o flat -x'
    expect: 'Installed Example.Tools 2.0.0'
---
Each package is extracted into its own folder inside the output folder.
`-o <dir>` (or `-OutputDirectory <dir>`) names the output folder; without
it, the tool uses the current folder. The tool creates the output folder
when it does not exist.

**Folder names.** A package folder is named `{Id}.{Version}`, such as
`Example.Tools.2.0.0` for the command that [[#install]] prints. With
`-x` (or `-ExcludeVersion`) it is named `{Id}` only, such as
`Example.Tools`, as in [[#flat]]. The id and the version are
written as the packages.config file writes them.

**Re-runs.** When the folder of a package already exists, the tool does
not touch it and prints `Skipping <id> <version> (already exists)`, as
the command from [[#again]] shows. A second run is therefore fast and
safe in CI. With `-x`, the folder name has no version, so a new version
in packages.config is not installed until you delete the old folder.

**Cache.** The tool extracts each package from the NuGet global packages
folder, which keeps a copy of every package; see [[package-sources]].
