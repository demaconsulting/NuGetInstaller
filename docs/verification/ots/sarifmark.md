## SarifMark Verification

### Required Functionality

SarifMark (`NuGetInstaller-OTS-SarifMark`) shall convert CodeQL SARIF results into a
Markdown report. The `DemaConsulting.SarifMark` tool reads the SARIF output produced by
CodeQL code scanning and renders it as a human-readable Markdown document included in the
release artifacts.

### Verification Approach

SarifMark is verified by CI pipeline step evidence. The tool runs in the same CI pipeline
that produces the TRX test results. A successful pipeline run demonstrates that SarifMark
read the SARIF input and generated the Markdown report without error.

Test evidence names:

- `SarifMark_SarifReading` — linked to the CI step that reads the CodeQL SARIF output
- `SarifMark_MarkdownReportGeneration` — linked to the CI step that writes the Markdown report

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-SarifMark` | `SarifMark_SarifReading`, `SarifMark_MarkdownReportGeneration` |
