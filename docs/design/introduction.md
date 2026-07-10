# Introduction

This document provides the detailed design for the NuGet Installer, a .NET command-line
application that installs the NuGet packages listed in a `packages.config` file into a local
output directory. It covers local software items (the system, its subsystems, and units) and
the OTS software items consumed by this project's build pipeline.

## Purpose

The purpose of this document is to define the design for each software item in the NuGet
Installer — full architectural and detailed design for local items (system, subsystems, and
units) and integration and usage design for OTS software items. A reviewer should be able to
understand how each item satisfies its requirements without reading source code. The document
does not restate requirements; it explains how they are realized.

## Scope

This document covers the following software items:

Local items:

- **NuGetInstaller**: system, subsystem, and unit design for all local components.

OTS items (build-time and pipeline tools) are also in scope; each has integration and usage
design documented under `docs/design/ots/`:

- **BuildMark**, **FileAssert**, **Pandoc**, **ReqStream**, **ReviewMark**, **SarifMark**,
  **SonarMark**, **SysML2Tools**, **VersionMark**, **WeasyPrint**, **xUnit**.

The following topics are out of scope:

- External library internals
- Build pipeline configuration
- Deployment and packaging
- Test projects

## Software Structure

The software structure is modeled in SysML2 under `docs/sysml2/` and rendered to the
diagram below by SysML2Tools as part of the build pipeline. AI agents should query the
SysML2 model directly (see the `sysml2tools-query` skill) rather than parsing this
diagram or the prose below. The model covers the `NuGetInstaller` system and its
subsystems/units only; the OTS build-time and pipeline tools listed above are not
modeled as SysML2 parts because they are not runtime dependencies of the shipped
system — their integration and usage are documented as prose design under
`docs/design/ots/` instead.

![Software Structure](SoftwareStructureView.svg)

## Folder Layout

The source code folder structure mirrors the top-level subsystem breakdown above, giving
reviewers an explicit navigation aid from design to code:

```text
src/DemaConsulting.NuGetInstaller/
├── Program.cs                  — entry point and execution orchestrator
├── Cli/
│   └── Context.cs              — command-line argument parser and I/O owner
├── NuGet/
│   ├── Models/
│   │   └── PackageEntry.cs     — package entry data model
│   ├── PackagesConfigReader.cs — XML parser for packages.config files
│   ├── PackageExtractor.cs     — ZIP extraction wrapper for .nupkg files
│   └── PackageInstaller.cs     — parallel install orchestrator
├── SelfTest/
│   └── Validation.cs           — self-validation test runner
└── Utilities/
    └── PathHelpers.cs          — safe path combination utilities
```

## Document Conventions

Throughout this document:

- Class names, method names, property names, and file names appear in `monospace` font.
- The word **shall** denotes a design constraint that the implementation must satisfy.
- Section headings within each unit chapter follow a consistent structure: overview, data model,
  methods/algorithms, and interactions with other units.
- Text tables are used in preference to diagrams, which may not render in all PDF viewers.

## Companion Artifact Structure

Each in-house software item has corresponding artifacts in parallel directory trees:

- Requirements: `docs/reqstream/{system-name}.yaml`, `docs/reqstream/{system-name}/.../{item}.yaml`
- Design docs: `docs/design/{system-name}.md`, `docs/design/{system-name}/.../{item}.md`
- Verification: `docs/verification/{system-name}.md`, `docs/verification/{system-name}/.../{item}.md`
- Source code: `src/{SystemName}/.../{Item}.cs` (PascalCase for C#)
- Tests: `test/{SystemName}.Tests/.../{Item}Tests.cs` (PascalCase for C#)

OTS items have integration and usage design docs at `docs/design/ots/{ots-name}.md` describing
how the NuGet Installer integrates each third-party tool or library; their artifacts sit
parallel to system folders:

- Requirements: `docs/reqstream/ots/{ots-name}.yaml`
- Design: `docs/design/ots/{ots-name}.md`
- Verification: `docs/verification/ots/{ots-name}.md`

Review-sets for all items are defined in `.reviewmark.yaml` at the repository root.

## References

- NuGet Installer System Requirements (`docs/reqstream/nuget-installer.yaml`)
- NuGet Installer User Guide (`docs/user_guide/introduction.md`)
- NuGet Installer Repository: <https://github.com/demaconsulting/NuGetInstaller>
