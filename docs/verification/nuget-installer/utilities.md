## Utilities Subsystem Verification

### Verification Approach

The Utilities subsystem is verified using xUnit integration tests in
`UtilitiesSubsystemTests.cs`. Tests exercise subsystem-level workflows — path resolution,
directory creation, path traversal validation, and null-argument rejection — by calling
the subsystem API directly. No mocking is applied at the subsystem level.

### Test Environment

Tests run under the standard xUnit runner with no external services. Directory-creation
tests write to uniquely named folders under the OS temporary directory and remove them
during teardown; no persistent state is required.

### Acceptance Criteria

A subsystem test passes when `PathHelpers.SafePathCombine` returns the expected resolved
path for valid inputs, and throws the expected exception type (`ArgumentNullException` for
null arguments, `ArgumentException` for traversal or rooted-path attempts) for invalid
inputs.

### Test Scenarios

#### Safe Path Resolution Scenario

Tests verify that valid relative paths are resolved correctly and that dangerous paths
(containing `..` traversal sequences or rooted paths) are rejected with appropriate exceptions.

Test methods:

- `UtilitiesSubsystem_PathResolutionWorkflow_ValidPaths_ResolvesCorrectly`
- `UtilitiesSubsystem_DirectoryCreationWorkflow_ValidPaths_CreatesDirectories`
- `UtilitiesSubsystem_PathTraversalValidation_DangerousPaths_ThrowsException`
- `UtilitiesSubsystem_SafePathCombine_RootedRelativePath_ThrowsArgumentException`

#### Null Rejection Scenario

Tests verify that null base path or null relative path arguments are rejected with an
`ArgumentNullException`.

Test methods:

- `UtilitiesSubsystem_SafePathCombine_NullBasePath_ThrowsArgumentNullException`
- `UtilitiesSubsystem_SafePathCombine_NullRelativePath_ThrowsArgumentNullException`

### Coverage Summary

| Requirement ID | Test Method(s) |
| --- | --- |
| `NuGetInstaller-Utilities-SafePaths` | `UtilitiesSubsystem_PathResolutionWorkflow_ValidPaths_ResolvesCorrectly`, `UtilitiesSubsystem_DirectoryCreationWorkflow_ValidPaths_CreatesDirectories`, `UtilitiesSubsystem_PathTraversalValidation_DangerousPaths_ThrowsException`, `UtilitiesSubsystem_SafePathCombine_RootedRelativePath_ThrowsArgumentException` |
| `NuGetInstaller-Utilities-NullRejection` | `UtilitiesSubsystem_SafePathCombine_NullBasePath_ThrowsArgumentNullException`, `UtilitiesSubsystem_SafePathCombine_NullRelativePath_ThrowsArgumentNullException` |
