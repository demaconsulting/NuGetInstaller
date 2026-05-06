## SonarMark Verification

### Required Functionality

SonarMark (`NuGetInstaller-OTS-SonarMark`) shall generate a SonarCloud quality report. The
`DemaConsulting.SonarMark` tool retrieves quality-gate and metrics data from SonarCloud and
renders it as a Markdown document included in the release artifacts.

### Verification Approach

SonarMark is verified by CI pipeline step evidence. The tool runs in the same CI pipeline
that produces the TRX test results. A successful pipeline run demonstrates that SonarMark
connected to SonarCloud, retrieved quality data, and generated the Markdown quality report
without error.

Test evidence names:

- `SonarMark_QualityGateRetrieval` — linked to the CI step that fetches the quality gate
- `SonarMark_IssuesRetrieval` — linked to the CI step that fetches issue counts
- `SonarMark_HotSpotsRetrieval` — linked to the CI step that fetches hotspot data
- `SonarMark_MarkdownReportGeneration` — linked to the CI step that writes the Markdown report

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-SonarMark` | `SonarMark_QualityGateRetrieval`, `SonarMark_IssuesRetrieval`, `SonarMark_HotSpotsRetrieval`, `SonarMark_MarkdownReportGeneration` |
