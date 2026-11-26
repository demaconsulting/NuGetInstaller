---
title: Getting started
summary: Install the tool, list your packages, and install them into a folder.
aliases:
  - start
  - quickstart
  - examples
  - install
tags:
  - tutorial
order: 1
see-also:
  - command-line
  - packages-config
  - package-sources
examples:
  - name: install-local
    kind: snippet
    language: shell
    save-as: install-local.sh
    description: Add the tool to the tool manifest of a repository.
    content: |
      dotnet new tool-manifest
      dotnet tool install --local DemaConsulting.NuGetInstaller
  - name: run-in-ci
    kind: snippet
    language: shell
    save-as: run-in-ci.sh
    description: Restore the manifest tools in CI, then run the tool.
    content: |
      dotnet tool restore
      dotnet {{tool}} packages.config -o packages
  - name: version
    kind: command
    language: shell
    description: Print the version and exit.
    content: '{{tool}} --version'
  - name: validate
    kind: snippet
    language: shell
    save-as: validate.sh
    description: Run the built-in self-tests; they need nuget.org.
    content: |
      {{tool}} --validate
  - name: first
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
    description: Install the listed packages into the folder packages.
    content: '{{tool}} packages.config -o packages'
    expect: 'Installed Example.Tools 2.0.0'
  - name: topic
    kind: command
    language: shell
    description: Print a help topic, here the packages.config file.
    content: '{{tool}} help packages-config'
    expect: 'targetFramework'
---
{{tool}} {{version}} is a .NET tool.
It installs the NuGet packages that a packages.config file lists into a
folder, as `nuget.exe install` does.

1. **Install it.** The tool needs the .NET 10 SDK or later. In CI, set it
   up first when the runner has an older .NET, for example with
   `actions/setup-dotnet` and `dotnet-version: 10.x`.
   For a repository or CI, install it as a local tool. Run the commands
   that [[#install-local]] prints once, without
   `dotnet new tool-manifest` when `.config/dotnet-tools.json` exists, and
   commit that file. In CI, run `dotnet tool restore` first, then run the
   tool as `dotnet {{tool}}`, as in the commands that
   [[#run-in-ci]] prints. For one machine, install it globally and
   run it as `{{tool}}`:

   ```bash
   dotnet tool install -g DemaConsulting.NuGetInstaller
   ```

2. **Check it.** The command that [[#version]] prints shows the version.
   The command from [[#validate]] runs the built-in
   self-tests, which download a package from nuget.org;
   see [[self-validation]].
3. **List the packages.** Write a packages.config file with one `package`
   element for each package, as [[#first]] shows;
   see [[packages-config]].
4. **Install them.** The command that [[#install]] prints
   installs every listed package into the folder `packages`; see
   [[output-folders]]. The packages come from the sources of your
   nuget.config files, normally nuget.org; see [[package-sources]].
   The examples in this help use the packages `Example.Tools` and
   `Example.Headers` from a local test feed: use your own package ids.

Print any help topic with `{{tool}} help <topic>`, as in the command that
[[#topic]] prints. Add `--search <words>` to find a topic, and `--json` to
get any help answer as JSON.
