## WeasyPrint OTS Design

DemaConsulting.WeasyPrintTool is a .NET dotnet global tool wrapper around the WeasyPrint Python
library. It converts HTML documents produced by Pandoc into PDF artifacts for release
distribution and compliance archiving.

### Purpose

WeasyPrint renders the HTML output produced by Pandoc into PDF documents for all document
collections: Build Notes, Code Quality, Review Plan, Review Report, Design, Verification, User
Guide, and Requirements/Trace-Matrix. PDF output enables FileAssert to assert PDF metadata and
content during CI validation, and provides a format suitable for long-term archivability and
distribution to reviewers.

WeasyPrint is chosen because it applies CSS-based page layout, supports custom fonts and
headers via the shared `docs/template/` CSS, and is available as a dotnet global tool wrapper
that integrates with the existing pipeline without requiring a separate Python invocation
step.

### Features Used

- CSS-based HTML-to-PDF rendering via `dotnet weasyprint --pdf-variant <variant> <html-in>
  <pdf-out>`, applying the shared CSS in `docs/template/` for page layout, fonts, and headers.

### Integration Pattern

WeasyPrint is installed as a .NET local tool via the package `demaconsulting.weasyprinttool`
in `.config/dotnet-tools.json` and restored with `dotnet tool restore`. It is configured
entirely through command-line arguments — the input HTML file path and the output PDF file
path — with no separate WeasyPrint configuration files used in this project. The tool is
invoked once per document collection after Pandoc has produced the HTML. The CI workflow
installs Python via `actions/setup-python` to satisfy the WeasyPrintTool's internal Python
dependency; the WeasyPrint Python library and its CSS rendering dependencies (Pango, Cairo,
fontconfig) must be present in the runner image.

The generated PDF files are validated by FileAssert assertions and uploaded as release
artifacts. No WeasyPrint NuGet dependencies are propagated to the main source project or the
published NuGet package.
