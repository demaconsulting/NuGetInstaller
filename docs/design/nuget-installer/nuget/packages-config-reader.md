# PackagesConfigReader

The `PackagesConfigReader` class reads and parses packages.config XML files, returning a
list of `PackageEntry` objects.

## Overview

`PackagesConfigReader` is a static utility class with a single `Read` method. It loads the
XML document, iterates over `<package>` elements, and constructs `PackageEntry` objects from
the `id`, `version`, and optional `targetFramework` attributes.

## Data Model

`PackagesConfigReader` holds no instance state. All state is local to the `Read` method.

## Methods

### Read(string filePath)

Reads and parses a packages.config file.

**Parameters:**

| Parameter  | Type     | Description                          |
|------------|----------|--------------------------------------|
| `filePath` | `string` | Path to the packages.config file.    |

**Returns:** `IReadOnlyList<PackageEntry>` — the parsed package entries.

**Algorithm:**

1. Check that the file exists; throw `InvalidOperationException` if not found.
2. Load the XML document via `XDocument.Load`.
3. Select all `<package>` elements from the root.
4. For each element, read `id` (required), `version` (required), and `targetFramework`
   (optional) attributes.
5. Throw `InvalidOperationException` if `id` or `version` is missing.
6. Return a read-only list of `PackageEntry` objects.

## Interactions

| Dependency     | Direction | Purpose                                          |
|----------------|-----------|--------------------------------------------------|
| `PackageEntry` | Creates   | Constructs `PackageEntry` instances from XML.    |

## Error Handling

`Read` throws the following exceptions:

| Exception                  | Condition                                                           |
|----------------------------|---------------------------------------------------------------------|
| `InvalidOperationException` | The file is not found, or `id`/`version` attribute is missing.    |
| `System.Xml.XmlException`  | The file contains malformed XML; propagated directly from `XDocument.Load`. |

The `XDocument.Load` call propagates `XmlException` for malformed XML without wrapping it,
preserving the original diagnostic message (line number, position) for the caller.
