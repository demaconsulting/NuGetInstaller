package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def UtilitiesSubsystem {
        doc /* Shared utility subsystem: general-purpose helpers reused by other subsystems (path safety, etc.). */

        metadata info : SoftwareInfo {
            description = "Shared utility subsystem: general-purpose helpers reused by other subsystems (path safety, etc.).";
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/Utilities/UtilitiesSubsystemTests.cs");
            designFiles = ("docs/design/nuget-installer/utilities.md");
            verificationFiles = ("docs/verification/nuget-installer/utilities.md");
            requirementFiles = ("docs/reqstream/nuget-installer/utilities.yaml");
        }

        part pathHelpers : PathHelpers;
    }
}
