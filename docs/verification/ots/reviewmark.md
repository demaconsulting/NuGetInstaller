## ReviewMark Verification

### Required Functionality

ReviewMark (`NuGetInstaller-OTS-ReviewMark`) shall generate a review plan and review report
from the review configuration. The `DemaConsulting.ReviewMark` tool reads `.reviewmark.yaml`
and the review evidence store to produce a review plan and review report documenting file
review coverage and currency.

### Verification Approach

ReviewMark is verified by CI pipeline step evidence. The tool runs as part of the same CI
pipeline that produces the TRX test results. A successful pipeline run demonstrates that
ReviewMark read the configuration, queried the evidence store, and generated both the review
plan and the review report without error.

Test evidence names:

- `ReviewMark_ReviewPlanGeneration` — linked to the CI step that generates the review plan
- `ReviewMark_ReviewReportGeneration` — linked to the CI step that generates the review report

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-ReviewMark` | `ReviewMark_ReviewPlanGeneration`, `ReviewMark_ReviewReportGeneration` |
