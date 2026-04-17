# PackageExtractor

The `PackageExtractor` class extracts NuGet package (.nupkg) contents to a destination
folder.

## Overview

`PackageExtractor` is a static utility class with a single `Extract` method. It wraps
`ZipFile.ExtractToDirectory` with an idempotency check: if the destination folder already
exists, extraction is skipped and the method returns `false`.

## Data Model

`PackageExtractor` holds no instance state. All state is local to the `Extract` method.

## Methods

### Extract(string nupkgPath, string destFolder)

Extracts all entries from a .nupkg file into the destination folder.

**Parameters:**

| Parameter    | Type     | Description                              |
|--------------|----------|------------------------------------------|
| `nupkgPath`  | `string` | Path to the .nupkg file.                 |
| `destFolder` | `string` | Destination folder for extraction.       |

**Returns:** `bool` — `true` if extraction was performed; `false` if skipped.

**Algorithm:**

1. If `destFolder` already exists, return `false`.
2. Call `ZipFile.ExtractToDirectory(nupkgPath, destFolder)`.
3. Return `true`.

## Interactions

`PackageExtractor` has no dependencies on other tool units. It uses only .NET base class
library types (`Directory`, `ZipFile`).
