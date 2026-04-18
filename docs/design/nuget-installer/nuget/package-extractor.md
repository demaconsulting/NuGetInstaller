# PackageExtractor

The `PackageExtractor` class extracts NuGet package (.nupkg) contents to a destination
folder.

## Overview

`PackageExtractor` is a static utility class with a single `Extract` method. It opens the
archive with `ZipFile.OpenRead` and extracts entries one at a time, accumulating the total
number of bytes written. If the destination folder already exists, extraction is skipped
and the method returns `false`. If the total extracted size exceeds 1 GB the method throws
an `InvalidOperationException` to defend against zip-bomb attacks.

## Data Model

`PackageExtractor` holds no instance state. All state is local to the `Extract` method.

The class defines one constant:

| Constant              | Value        | Description                                        |
|-----------------------|--------------|----------------------------------------------------|
| `MaxExtractedBytes`   | 1 073 741 824 | Maximum bytes that may be extracted (1 GB ceiling) |

## Methods

### Extract(string nupkgPath, string destFolder)

Extracts all entries from a .nupkg file into the destination folder.

**Parameters:**

| Parameter    | Type     | Description                              |
|--------------|----------|------------------------------------------|
| `nupkgPath`  | `string` | Path to the .nupkg file.                 |
| `destFolder` | `string` | Destination folder for extraction.       |

**Returns:** `bool` — `true` if extraction was performed; `false` if skipped.

**Throws:** `InvalidOperationException` — if the total extracted size exceeds 1 GB.

**Algorithm:**

1. If `destFolder` already exists, return `false`.
2. Open the archive with `ZipFile.OpenRead(nupkgPath)` and initialise `totalExtractedBytes`
   to zero.
3. For each `ZipArchiveEntry` in the archive:
   a. Skip entries that have an empty `Name` (directory markers).
   b. Determine the destination path by combining `destFolder` with `entry.FullName`.
   c. Ensure the parent directory exists (`Directory.CreateDirectory`).
   d. Open the entry stream and the destination file stream.
   e. Read the entry in 80 KiB chunks; after each chunk add the bytes read to
      `totalExtractedBytes`. If `totalExtractedBytes` exceeds `MaxExtractedBytes`, throw
      `InvalidOperationException` (potential zip-bomb).
   f. Write the chunk to the destination file stream.
4. Return `true`.

## Security

The zip-bomb defence prevents a maliciously crafted archive from consuming unbounded
disk space by limiting total output to 1 GB. The limit is enforced incrementally as each
chunk is decompressed and written, so memory consumption remains bounded regardless of
the archive's claimed uncompressed sizes.

## Interactions

`PackageExtractor` has no dependencies on other tool units. It uses only .NET base class
library types (`Directory`, `File`, `ZipFile`, `ZipArchiveEntry`).
