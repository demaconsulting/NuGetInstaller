### PathHelpers Unit Verification

#### Verification Strategy

The `PathHelpers` unit is verified using xUnit unit tests in `PathHelpersTests.cs`. Tests
call `PathHelpers.SafePathCombine` directly with a range of base path and relative path
combinations and assert on the returned combined path or the thrown exception type. Boundary
values include empty relative path, single-dot reference, nested sub-paths, and the
platform-specific Windows absolute path (verified only on Windows).

#### Test Scenarios

##### Safe Combine Scenario

Tests verify correct combination for valid inputs and rejection for all documented dangerous
patterns.

Test methods:

- `PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly` — asserts a simple relative path
  is combined under the base path
- `PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException` — leading
  `../` traversal is rejected
- `PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException` — embedded `..`
  segment is rejected
- `PathHelpers_SafePathCombine_SlashRootedRelativePath_ThrowsArgumentException` — relative
  path starting with `/` is rejected
- `windows@PathHelpers_SafePathCombine_WindowsAbsolutePath_ThrowsArgumentException` — Windows
  absolute path (e.g. `C:\...`) is rejected (Windows only)
- `PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly` — asserts a multi-segment
  relative path is resolved correctly
- `PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly` — `.` reference
  is resolved to the base path
- `PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath` — empty relative path
  returns the base path unchanged
- `PathHelpers_SafePathCombine_DotDotPrefixedName_CombinesCorrectly` — a file name beginning
  with `..` that is not a traversal segment (e.g. `..myfile`) is accepted

##### Null Input Rejection Scenario

Tests verify that null arguments throw `ArgumentNullException`.

Test methods:

- `PathHelpers_SafePathCombine_NullBasePath_ThrowsArgumentNullException`
- `PathHelpers_SafePathCombine_NullRelativePath_ThrowsArgumentNullException`

#### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-PathHelpers-SafeCombine` | `PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly`, `PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException`, `PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException`, `PathHelpers_SafePathCombine_SlashRootedRelativePath_ThrowsArgumentException`, `windows@PathHelpers_SafePathCombine_WindowsAbsolutePath_ThrowsArgumentException`, `PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly`, `PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly`, `PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath`, `PathHelpers_SafePathCombine_DotDotPrefixedName_CombinesCorrectly` |
| `NuGetInstaller-PathHelpers-NullInputRejection` | `PathHelpers_SafePathCombine_NullBasePath_ThrowsArgumentNullException`, `PathHelpers_SafePathCombine_NullRelativePath_ThrowsArgumentNullException` |
