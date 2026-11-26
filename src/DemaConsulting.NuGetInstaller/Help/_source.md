---
name: nuget-installer
schema: 2
title: NuGet Installer
summary: Built-in help of the DEMA Consulting tool that installs the packages of a packages.config file.
---
{{tool}} installs the NuGet packages that a packages.config file lists into a folder, as `nuget.exe install`
does, using the package sources of your nuget.config files.

Every operation is an option. The only command is `{{tool}} help`, which must be the first argument and takes
topic names, such as `{{tool}} help packages-config`.
