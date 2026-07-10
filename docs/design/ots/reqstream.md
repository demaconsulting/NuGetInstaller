## ReqStream OTS Design

DemaConsulting.ReqStream is a .NET dotnet global tool that processes requirements YAML files
and TRX test-result files to generate requirements reports and enforce that every requirement
is linked to at least one passing test.

### Purpose

ReqStream reads the project's `requirements.yaml` root file — which includes all subsystem and
OTS requirements YAML files — together with the TRX files accumulated by the CI pipeline, and
produces three generated Markdown reports: a requirements report, a justifications document,
and a traceability matrix. When invoked with `--enforce`, ReqStream exits with a non-zero exit
code if any requirement lacks at least one passing test result, making incomplete traceability
a build-breaking condition.

ReqStream is chosen because it understands the project's YAML requirements format natively,
processes TRX files directly, and integrates with the `--enforce` flag as a quality gate
without requiring a separate CI plugin or external service.

### Features Used

- Requirements YAML linting via `dotnet reqstream --lint --requirements <requirements.yaml>`,
  which validates YAML structure and cross-references (used in `lint.ps1`, all CI jobs, and
  local pre-PR checks).
- Coverage enforcement and report generation via `dotnet reqstream ... --enforce
  <requirements.yaml> <trx-files...>`, which produces a requirements report, a justifications
  document, and a traceability matrix, and fails if any requirement lacks passing evidence.
- YAML `includes:` composition, which lets `requirements.yaml` assemble all subsystem and OTS
  requirements YAML files under `docs/reqstream/` into a single requirements set.

### Integration Pattern

ReqStream is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.reqstream` and restored with `dotnet tool restore`. It is
configured entirely through the `requirements.yaml` root file, where each included YAML file
defines a hierarchy of sections and requirements with an `id`, `title`, `justification`, `tags`,
and a list of `tests` that must pass. The CI pipeline invokes it in two places: `lint.ps1`'s
lint step, and the build-docs job's enforce step after all test and self-validation TRX files
have been accumulated as workflow artifacts.

The generated Markdown files are written to `docs/requirements_report/generated/` and consumed
by Pandoc to produce the Requirements and Trace Matrix PDFs. ReqStream has no transitive NuGet
dependencies that propagate to the main source project; it requires TRX files produced by
`dotnet test --logger trx` and by `dotnet fileassert --results`, operates entirely at the file
system level, and requires no network access.
