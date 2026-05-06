## WeasyPrint Verification

### Required Functionality

WeasyPrint (`NuGetInstaller-OTS-WeasyPrint`) shall convert HTML documents to valid PDF. The
`DemaConsulting.WeasyPrintTool` wrapper converts HTML documents to PDF as part of the
documentation build pipeline.

### Verification Approach

WeasyPrint is verified by CI pipeline step evidence combined with FileAssert document
validation. Each HTML document (build notes, code quality report, review plan, review report,
design document, and user guide) is converted to PDF by WeasyPrint in the CI pipeline.
FileAssert then asserts that each generated PDF file exists, has a non-trivial size, contains
at least one page, and includes expected document content in the rendered text. Passing
FileAssert assertions confirm WeasyPrint executed correctly and produced meaningful output.

Test evidence names:

- `WeasyPrint_BuildNotesPdf` — build-notes PDF document validated
- `WeasyPrint_CodeQualityPdf` — code quality PDF document validated
- `WeasyPrint_ReviewPlanPdf` — review plan PDF document validated
- `WeasyPrint_ReviewReportPdf` — review report PDF document validated
- `WeasyPrint_DesignPdf` — design document PDF validated
- `WeasyPrint_UserGuidePdf` — user guide PDF document validated

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_BuildNotesPdf` |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_CodeQualityPdf` |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_ReviewPlanPdf` |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_ReviewReportPdf` |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_DesignPdf` |
| `NuGetInstaller-OTS-WeasyPrint` | `WeasyPrint_UserGuidePdf` |
