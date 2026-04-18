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

The class defines two constants:

| Constant              | Value         | Description                                        |
|-----------------------|---------------|----------------------------------------------------|
| `MaxExtractedBytes`   | 1 073 741 824 | Maximum bytes that may be extracted (1 GB ceiling) |
| `CopyBufferSize`      | 81 920        | Buffer size (80 KiB) used when copying entry data  |

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
2. Allocate a single reusable `CopyBufferSize` byte buffer and initialize
   `totalExtractedBytes` to zero.
3. Open the archive with `ZipFile.OpenRead(nupkgPath)`.
4. For each `ZipArchiveEntry` in the archive:
   a. Skip entries that have an empty `Name` (directory markers).
   b. Resolve the canonical destination path: `destPath = Path.GetFullPath(Path.Combine(destFolder, entry.FullName))`.
      Verify `destPath` starts with the canonical destination folder plus a trailing directory
      separator (`canonicalDestFolderWithSep`). If it does not, throw `InvalidOperationException`
      with a descriptive zip-slip message.
   c. Ensure the parent directory exists (`Directory.CreateDirectory`).
   d. Open the entry stream and the destination file stream.
   e. Read the entry in `CopyBufferSize` (80 KiB) chunks; after each chunk add the bytes
      read to `totalExtractedBytes`. If `totalExtractedBytes` exceeds `MaxExtractedBytes`,
      throw `InvalidOperationException` (zip-bomb defense).
   f. Write the chunk to the destination file stream.
5. Return `true`.

## Security

Two attack vectors are mitigated:

- **Zip-bomb**: The cumulative decompressed bytes written to disk is tracked across all
  entries. If the total exceeds `MaxExtractedBytes` (1 GB), extraction is aborted with an
  `InvalidOperationException`. The limit is enforced incrementally as each chunk is
  decompressed, so memory consumption remains bounded regardless of the archive's claimed
  uncompressed sizes.
- **Zip-slip**: The canonical destination path for each entry is resolved with `Path.GetFullPath`
  and verified to start with the canonical destination folder (with a trailing directory separator)
  before any file is written. This follows the SonarQube S6096 / CodeQL `cs/zipslip`-recognized
  pattern and guards against path-traversal entries that would write outside `destFolder`.
- **Partial-extraction on exception**: When a zip-slip or zip-bomb exception is thrown
  mid-extraction, no cleanup of the partially created destination folder is performed.
  The caller is responsible for cleaning up any partially populated folder if desired.

## Interactions

`PackageExtractor` uses .NET base class library types (`Directory`, `File`, `ZipFile`, `ZipArchiveEntry`).
