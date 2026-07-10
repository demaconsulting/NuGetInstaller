package NuGetInstaller {
    part def PathHelpers {
        doc /* Safe path-combination utility that protects callers against path-traversal attacks by verifying the resolved combined path stays within the base directory. */

        comment sourceRef /* Source: src/DemaConsulting.NuGetInstaller/Utilities/PathHelpers.cs */
        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/Utilities/PathHelpersTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/utilities/path-helpers.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/utilities/path-helpers.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/utilities/path-helpers.yaml */
    }
}
