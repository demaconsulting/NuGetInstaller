## FileAssert Verification

### Required Functionality

FileAssert (`NuGetInstaller-OTS-FileAssert`) shall validate generated documents against
acceptance criteria. It validates HTML and PDF documents produced during the build, asserting
that each document exists, has a non-trivial size, is structurally valid, and contains
expected content. It also provides verification evidence for Pandoc and WeasyPrint.

### Verification Approach

FileAssert is verified using its built-in self-validation mechanism, invoked in the CI pipeline
as `dotnet fileassert --validate --results <trx-file>`. Self-validation exercises the tool's own
document-assertion harness — including its file-existence, text, HTML, XML, YAML, JSON, PDF,
and ZIP content-assertion capabilities — and writes TRX evidence for each check. A zero exit
code and passing TRX results confirm the tool is operational before it validates the generated
documents.

Test evidence names:

- `FileAssert_VersionDisplay` — confirms FileAssert responds correctly to `--version`
- `FileAssert_HelpDisplay` — confirms FileAssert responds correctly to `--help`
- `FileAssert_File` — confirms file-existence and size assertions are functioning
- `FileAssert_Text` — confirms text-content assertions are functioning
- `FileAssert_Html` — confirms HTML structural (XPath) assertions are functioning
- `FileAssert_Xml` — confirms XML structural assertions are functioning
- `FileAssert_Yaml` — confirms YAML content assertions are functioning
- `FileAssert_Json` — confirms JSON content assertions are functioning
- `FileAssert_Pdf` — confirms PDF metadata, page-count, and text assertions are functioning
- `FileAssert_Zip` — confirms ZIP archive content assertions are functioning
- `FileAssert_Results` — confirms TRX/JUnit result-file generation is functioning

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-OTS-FileAssert` | `FileAssert_VersionDisplay`, `FileAssert_HelpDisplay`, `FileAssert_File`, `FileAssert_Text`, `FileAssert_Html`, `FileAssert_Xml`, `FileAssert_Yaml`, `FileAssert_Json`, `FileAssert_Pdf`, `FileAssert_Zip`, `FileAssert_Results` |
