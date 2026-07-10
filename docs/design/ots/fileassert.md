## FileAssert OTS Design

DemaConsulting.FileAssert is a .NET dotnet global tool that asserts file content against
YAML-defined test suites. It validates the HTML and PDF documents produced by Pandoc and
WeasyPrint during the CI pipeline, ensuring the generated release artifacts meet content and
structural expectations.

### Purpose

FileAssert acts as the acceptance-test harness for every document collection produced by the
build. After Pandoc renders each Markdown collection to HTML and WeasyPrint renders the HTML to
PDF, FileAssert reads a YAML test suite that describes expected content, structural properties,
and file-existence rules, and reports the outcome as TRX evidence that ReqStream then consumes
via `--enforce`.

FileAssert is chosen because it is purpose-built for this project family's Continuous
Compliance methodology: it consumes a project-local YAML test suite (`.fileassert.yaml`),
produces TRX output on the same schema the rest of the pipeline uses, and is available as a
dotnet global tool that requires no wrapper code.

### Features Used

- YAML-driven test-suite execution via `dotnet fileassert --results <trx-file> <tag>`, which
  runs a named set of file-content and structural assertions and writes TRX evidence.
- Built-in self-validation via `dotnet fileassert --validate --results <trx>`, which exercises
  the tool's own test harness and writes TRX evidence for its own correctness.

### Integration Pattern

FileAssert is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.fileassert` and restored with `dotnet tool restore`. It reads its
test suite from `.fileassert.yaml` at the repository root, which defines named test scenarios
(one per generated HTML/PDF document collection) that each specify the file under test, the
expected size floor, and the content assertions that must hold; all runtime options are
otherwise supplied on the command line. The CI pipeline invokes it once per document collection
in the build-docs job, each invocation writing a TRX result file to `artifacts/`.

The generated TRX files are consumed by `dotnet reqstream --enforce` to verify that every OTS
and unit requirement has passing evidence. FileAssert operates as an isolated tool process; its
transitive NuGet dependencies do not propagate to the main source project or the published
NuGet package, and no external service or network access is required.
