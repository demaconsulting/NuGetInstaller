package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def PathHelpers {
        doc /* Safe path-combination utility that protects callers against path-traversal attacks by verifying the resolved combined path stays within the base directory. */

        metadata info : SoftwareInfo {
            description = "Safe path-combination utility that protects callers against path-traversal attacks by verifying the resolved combined path stays within the base directory.";
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/Utilities/PathHelpers.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/Utilities/PathHelpersTests.cs");
            designFiles = ("docs/design/nuget-installer/utilities/path-helpers.md");
            verificationFiles = ("docs/verification/nuget-installer/utilities/path-helpers.md");
            requirementFiles = ("docs/reqstream/nuget-installer/utilities/path-helpers.yaml");
        }
    }
}
