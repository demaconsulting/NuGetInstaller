## SonarMark OTS Design

DemaConsulting.SonarMark is a .NET dotnet local tool that retrieves quality-gate status,
issues, and security hot-spots from the SonarCloud API and renders them as a Markdown report
included in the Code Quality PDF artifact.

### Purpose

SonarMark surfaces the SonarCloud quality-gate result and detailed metrics — issues, code
smells, coverage, duplications, and security hot-spots — as a Markdown document for inclusion
in the Code Quality PDF. This gives reviewers a persistent, artifact-bound quality snapshot for
each release, independent of the SonarCloud web dashboard.

SonarMark is chosen because it integrates with the SonarCloud REST API to retrieve data that
is not available from the CI runner's standard output, and it produces Markdown output
compatible with the Pandoc pipeline.

### Features Used

- SonarCloud quality-gate and metrics retrieval via `dotnet sonarmark --project <key> --output
  <markdown-file>`, which queries issues, code smells, coverage, duplications, and security
  hot-spots and renders them as a Markdown report.

### Integration Pattern

SonarMark is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.sonarmark` and restored with `dotnet tool restore`. It is
configured entirely through command-line arguments — the SonarCloud project key
(`demaconsulting_NuGetInstaller`) and the output Markdown file path — with authentication
supplied via the `SONAR_TOKEN` environment variable injected by the CI job; no additional
configuration files are required. The CI pipeline invokes SonarMark in the build-docs job
after the SonarCloud analysis has completed.

The generated Markdown file is written to `docs/code_quality/generated/sonar-quality.md` and
consumed by Pandoc to produce the Code Quality HTML and subsequently the Code Quality PDF.
SonarMark requires authenticated network access to the SonarCloud REST API at
`sonarcloud.io` and has no transitive NuGet dependencies that propagate to the main source
project.
