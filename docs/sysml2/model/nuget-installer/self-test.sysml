package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def SelfTestSubsystem {
        doc /* Self-validation subsystem: runs a built-in suite of tests that prove the tool is operational in the deployment environment. */

        metadata info : SoftwareInfo {
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/SelfTest/SelfTestSubsystemTests.cs");
            designFiles = ("docs/design/nuget-installer/self-test.md");
            verificationFiles = ("docs/verification/nuget-installer/self-test.md");
            requirementFiles = ("docs/reqstream/nuget-installer/self-test.yaml");
        }

        part validation : Validation;
    }
}
