## ReviewMark Verification

### Required Functionality

ReviewMark shall generate a review plan from the review configuration
(`NuGetInstaller-OTS-ReviewMark-PlanGeneration`) and shall generate a review report from the
review configuration and evidence store (`NuGetInstaller-OTS-ReviewMark-ReportGeneration`). The
`DemaConsulting.ReviewMark` tool reads `.reviewmark.yaml` and the review evidence store to
produce these two documents.

### Verification Approach

ReviewMark is verified by CI pipeline step evidence. The tool's built-in `--validate`
command is executed in the CI pipeline and writes test method results to a TRX file.
The TRX file is consumed by ReqStream to satisfy the OTS requirements.

Test evidence names (test methods written to the TRX file by `dotnet reviewmark --validate`):

- `ReviewMark_ReviewPlanGeneration` — validates that ReviewMark can generate a review plan
- `ReviewMark_ReviewReportGeneration` — validates that ReviewMark can generate a review report

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-ReviewMark-PlanGeneration` | `ReviewMark_ReviewPlanGeneration` |
| `NuGetInstaller-OTS-ReviewMark-ReportGeneration` | `ReviewMark_ReviewReportGeneration` |
