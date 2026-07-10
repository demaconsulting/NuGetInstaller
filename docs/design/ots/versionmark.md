## VersionMark OTS Design

DemaConsulting.VersionMark is a .NET dotnet global tool that captures installed tool version
information during each CI job and publishes it as a Markdown document included in the Build
Notes PDF artifact.

### Purpose

VersionMark provides an audit record of the exact tool versions used to produce each release.
Each CI job captures the versions of the dotnet tools and runtime components it uses; the
build-docs job then merges all captured data and publishes a Markdown document included in the
Build Notes artifact. This supports reproducibility and compliance traceability for all build
tools.

VersionMark is chosen because it operates as a local dotnet tool alongside the other pipeline
tools, requires no external service, and produces Markdown output compatible with the Pandoc
pipeline.

### Features Used

- Capture of installed tool-version metadata for a CI job via
  `dotnet versionmark --capture --job-id <id> --output <json-file> -- <tools...>`.
- Merging of captured JSON files and publication of a consolidated Markdown versions document
  via `dotnet versionmark --publish --report <markdown-file> --report-depth <depth> --
  <glob-patterns...>`.
- Built-in self-validation that writes TRX evidence for ReqStream via
  `dotnet versionmark --validate --results <trx-file>`.
- Configuration linting via `dotnet versionmark --lint` in `lint.ps1`, which validates the
  `.versionmark.yaml` capture manifest.

### Integration Pattern

VersionMark is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.versionmark` and restored with `dotnet tool restore`. Its capture
manifest is `.versionmark.yaml` at the repository root, which lists every tool whose version
should be recorded (dotnet, git, node, npm, dotnet-sonarscanner, pandoc, weasyprint, sarifmark,
sonarmark, reqstream, buildmark, versionmark, reviewmark, fileassert, sysml2tools).

It is used in three modes across the CI pipeline:

- **Capture mode** (each CI job): invoked with `--capture --job-id <job> --output <json> --
  <tools...>` to interrogate installed tool versions for the listed tools and write a JSON file
  to the artifacts folder.
- **Publish mode** (build-docs job): invoked with `--publish --report <md> --report-depth
  <depth> -- <glob-patterns...>` to merge all captured JSON files matching the glob patterns and
  write the consolidated Markdown versions document, which is included in the Build Notes
  document and consumed by Pandoc.
- **Self-validation**: invoked with `--validate --results <trx-file>` to run the built-in
  validation suite and write TRX evidence for ReqStream consumption.

VersionMark reads installed tool metadata from the local environment and writes JSON and
Markdown files. It requires no external service or network access, and it has no transitive
NuGet dependencies that propagate to the main source project; it is a build-time tool only.
