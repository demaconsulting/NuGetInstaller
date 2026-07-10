## SarifMark OTS Design

DemaConsulting.SarifMark is a .NET dotnet global tool that reads SARIF (Static Analysis Results
Interchange Format) files produced by CodeQL code scanning and renders them as a human-readable
Markdown report included in the Code Quality PDF artifact.

### Purpose

SarifMark converts the CodeQL SARIF output into a Markdown document that provides a persistent,
human-readable record of any security findings identified by CodeQL for each release. This
document is compiled into the Code Quality PDF artifact alongside the SonarCloud quality
report, giving reviewers a unified view of static analysis results.

SarifMark is chosen because it understands the SARIF format natively and produces Markdown
output compatible with the Pandoc pipeline, requiring no custom scripting to transform CodeQL
output into the document format.

### Features Used

- SARIF-to-Markdown conversion via `dotnet sarifmark --sarif <sarif-file> --output <md-file>`,
  which reads the CodeQL SARIF output and renders a human-readable Markdown findings report.

### Integration Pattern

SarifMark is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.sarifmark` and restored with `dotnet tool restore`. It is
configured entirely through command-line arguments — the SARIF input file path and the output
Markdown file path — with no additional configuration files required. The CI pipeline invokes
SarifMark in the build-docs job after the CodeQL scanning step has completed and the SARIF file
(produced by the `github/codeql-action/analyze` GitHub Actions step) has been downloaded as a
workflow artifact.

The generated Markdown file is written to `docs/code_quality/generated/codeql-quality.md` and
consumed by Pandoc to produce the Code Quality HTML and subsequently the Code Quality PDF.
SarifMark has no transitive NuGet dependencies that propagate to the main source project and
requires no network access; it operates entirely on the local SARIF file.
