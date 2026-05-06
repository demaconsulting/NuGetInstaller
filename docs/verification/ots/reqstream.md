## ReqStream Verification

### Required Functionality

ReqStream (`NuGetInstaller-OTS-ReqStream`) shall enforce that every requirement is linked to
passing test evidence. The `DemaConsulting.ReqStream` tool processes `requirements.yaml` and
the TRX test result files to produce a requirements report, justifications document, and
traceability matrix.

### Verification Approach

ReqStream is verified by CI pipeline step evidence. The tool runs with the `--enforce` flag
in the pipeline. A zero exit code from the enforcement step proves all requirements are
covered by passing test evidence and that ReqStream is functioning correctly. A non-zero exit
code from any enforcement run is a build-breaking condition that surfaces unproven
requirements immediately.

Test evidence name:

- `ReqStream_EnforcementMode` — linked to the CI step that runs ReqStream with `--enforce`

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-ReqStream` | `ReqStream_EnforcementMode` |
