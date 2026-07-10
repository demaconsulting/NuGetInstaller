## Pandoc OTS Design

DemaConsulting.PandocTool is a .NET dotnet global tool wrapper around the Pandoc document
converter. It converts Markdown source documents into HTML as part of the documentation build
pipeline, producing the intermediate output that WeasyPrint then renders to PDF.

### Purpose

Pandoc converts the ordered set of Markdown content files for each document collection into a
single HTML document using a project-specific HTML template. The HTML output is then rendered
to PDF by WeasyPrint. Pandoc is used for all document collections: Build Notes, Code Quality,
Review Plan, Review Report, Design, Verification, User Guide, and the Requirements/Trace-Matrix
documents.

Pandoc is chosen because it handles multi-file document assembly natively, supports a custom
HTML template for consistent styling, and produces numbered sections and table-of-contents
entries without additional tooling.

### Features Used

- Multi-file document assembly via each collection's `definition.yaml`'s ordered `input-files`
  list, which Pandoc concatenates into a single HTML document.
- Custom HTML templating via the `template` key referencing the shared `template.html` in
  `docs/template/`, for consistent styling across all document collections.
- `table-of-contents: true` and `number-sections: true` options, which generate a navigation
  table of contents and numbered section headings in the output HTML without additional
  tooling.

### Integration Pattern

Pandoc is installed as a .NET local tool via the package `demaconsulting.pandoctool` in
`.config/dotnet-tools.json` and restored with `dotnet tool restore`. The tool is invoked as
`dotnet pandoc --defaults <definition.yaml>` for each document collection, with the metadata
version and date supplied on the command line. Each document collection provides a
`definition.yaml` file at `docs/{collection}/definition.yaml` that specifies the
`resource-path` (directories containing the HTML template and Markdown content files),
`input-files`, `template`, `table-of-contents`, and `number-sections` settings.

The generated HTML file is placed in `docs/{collection}/generated/{collection}.html` and is
passed directly to WeasyPrint for PDF conversion. The PandocTool wrapper bundles the Pandoc
executable internally, so no separate Pandoc installation is required; no explicit disposal
step is needed beyond the tool's normal process exit.
