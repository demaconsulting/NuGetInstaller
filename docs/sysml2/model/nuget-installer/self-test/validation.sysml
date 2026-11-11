package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def Validation {
        doc /* Self-validation test runner exercised at startup for tool self-check evidence. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/SelfTest/Validation.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/SelfTest/ValidationTests.cs");
            designFiles = ("docs/design/nuget-installer/self-test/validation.md");
            verificationFiles = ("docs/verification/nuget-installer/self-test/validation.md");
            requirementFiles = ("docs/reqstream/nuget-installer/self-test/validation.yaml");
        }
    }
}
