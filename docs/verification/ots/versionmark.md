## VersionMark Verification

### Required Functionality

VersionMark (`NuGetInstaller-OTS-VersionMark`) shall publish captured tool-version
information. The `DemaConsulting.VersionMark` tool reads version metadata for each
`dotnet tool` used in the pipeline and writes a versions Markdown document included in the
release artifacts.

### Verification Approach

VersionMark is verified by CI pipeline step evidence. The tool runs in the same CI pipeline
that produces the TRX test results. A successful pipeline run demonstrates that VersionMark
captured the tool versions and generated the Markdown versions report without error.

Test evidence names:

- `VersionMark_CapturesVersions` — linked to the CI step that collects tool version metadata
- `VersionMark_GeneratesMarkdownReport` — linked to the CI step that writes the versions report

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-VersionMark` | `VersionMark_CapturesVersions`, `VersionMark_GeneratesMarkdownReport` |
