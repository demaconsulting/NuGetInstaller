## BuildMark OTS Design

DemaConsulting.BuildMark is a .NET dotnet global tool that queries the GitHub Actions API to
capture workflow run details and renders them as a Markdown build-notes document included in
the release artifacts.

### Purpose

BuildMark provides an automated build-notes report for each CI pipeline run. It captures the
GitHub Actions workflow run details — including the workflow name, run number, trigger, and
associated commit — and renders them as a Markdown document. This document is compiled into the
Build Notes PDF artifact, giving reviewers a permanent record of the build provenance for each
release.

BuildMark is chosen because it integrates directly with the GitHub Actions event context,
requiring no manual input, and produces Markdown that is compatible with the Pandoc pipeline
already used for all other document collections.

### Features Used

- Markdown build-notes generation via
  `dotnet buildmark --build-version <version> --report <markdown-file> --report-depth <depth>`,
  which queries the GitHub API and writes a Markdown build-notes document.
- Automatic GitHub token resolution: BuildMark resolves a token from the `GH_TOKEN` environment
  variable first, falling back to `GITHUB_TOKEN`, and finally to `gh auth token` if neither is
  set. No token is passed as a command-line argument or stored in configuration.

### Integration Pattern

BuildMark is installed as a .NET local tool defined in `.config/dotnet-tools.json` under the
package name `demaconsulting.buildmark` and restored with `dotnet tool restore`. The tool is
invoked in the CI pipeline's build-docs job with the `GH_TOKEN` environment variable set from
`secrets.GITHUB_TOKEN`, and with the `--build-version`, `--report`, and `--report-depth`
command-line arguments controlling the report content and location; no additional configuration
files are used. The generated file is placed in `docs/build_notes/generated/build_notes.md`,
which Pandoc incorporates into the Build Notes HTML document.

BuildMark operates as an isolated tool process. Its internal dependencies do not propagate to
the main source project or the published NuGet package. The tool requires network access to the
GitHub REST API during the CI job that invokes it. The generated Markdown file has no runtime
dependency on BuildMark after it is produced; no explicit disposal step is required.

The CI step that invokes BuildMark has no `continue-on-error` setting, so a failed GitHub API
call (for example, due to network issues or an invalid/missing token) fails the step, and the
`build-docs` job, hard. There is no fallback report generation path; a successful Build Notes
document depends on the GitHub API call succeeding.
