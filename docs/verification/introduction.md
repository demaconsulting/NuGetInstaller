# Introduction

This document describes the verification design for the NuGet Installer, a .NET command-line
application that installs NuGet packages from a `packages.config` file into a local output
directory. It establishes the test approach for each software item and proves that every
requirement is covered by at least one named test scenario.

## Purpose

The purpose of this document is to prove that all requirements for the NuGet Installer system
are covered by named test scenarios. Each requirement at every level — system, subsystem, and
unit — is mapped to at least one test method so that reviewers can confirm completeness without
reading implementation code.

## Scope

This document covers verification of the following software units:

- **Program** — entry point and execution orchestrator (`Program.cs`)
- **Cli** subsystem:
  - **Context** unit — command-line argument parser and I/O owner (`Cli/Context.cs`)
- **NuGet** subsystem:
  - **Models** sub-subsystem:
    - **PackageEntry** unit — package entry data model (`NuGet/Models/PackageEntry.cs`)
  - **PackagesConfigReader** unit — XML parser for `packages.config` files (`NuGet/PackagesConfigReader.cs`)
  - **PackageExtractor** unit — ZIP extraction wrapper for `.nupkg` files (`NuGet/PackageExtractor.cs`)
  - **PackageInstaller** unit — parallel install orchestrator (`NuGet/PackageInstaller.cs`)
- **SelfTest** subsystem:
  - **Validation** unit — self-validation test runner (`SelfTest/Validation.cs`)
- **Utilities** subsystem:
  - **PathHelpers** unit — safe path combination utilities (`Utilities/PathHelpers.cs`)

The following ten OTS items are also verified:

- **BuildMark** — build-notes documentation generator
- **FileAssert** — document assertion tool
- **xUnit** — unit testing framework
- **Pandoc** — Markdown to HTML converter
- **ReqStream** — requirements traceability tool
- **ReviewMark** — file review tracking tool
- **SarifMark** — SARIF report processor
- **SonarMark** — SonarCloud quality reporter
- **VersionMark** — version tracking tool
- **WeasyPrint** — HTML to PDF converter

The following topics are out of scope:

- External library internals
- Build pipeline configuration
- Deployment and packaging

## Companion Artifacts

In-house software items have parallel artifacts organized as follows:

- **Requirements**: `docs/reqstream/nuget-installer.yaml` and
  `docs/reqstream/nuget-installer/{subsystem}/{item}.yaml`
- **Design**: `docs/design/introduction.md`, `docs/design/nuget-installer.md`, and
  `docs/design/nuget-installer/{subsystem}/{item}.md`
- **Verification**: `docs/verification/introduction.md` (this document) and
  `docs/verification/nuget-installer/{subsystem}/{item}.md`
- **Source**: `src/DemaConsulting.NuGetInstaller/{Subsystem}/{Item}.cs`
- **Tests**: `test/DemaConsulting.NuGetInstaller.Tests/{Subsystem}/{Item}Tests.cs`

OTS software items have no design documentation. Their artifacts are:

- **Requirements**: `docs/reqstream/ots/{ots-name}.yaml`
- **Verification**: `docs/verification/ots/{ots-name}.md`

Review-sets for all items are defined in `.reviewmark.yaml` at the repository root.

## References

- NuGet Installer System Requirements
- NuGet Installer Software Design Document
- NuGet Installer User Guide
