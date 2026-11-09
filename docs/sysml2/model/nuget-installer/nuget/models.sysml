package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def ModelsSubsystem {
        doc /* Data models carried through the NuGet subsystem pipeline: immutable records that describe each package to install. */

        metadata info : SoftwareInfo {
            description = "Data models carried through the NuGet subsystem pipeline: immutable records that describe each package to install.";
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/NuGet/Models/ModelsSubsystemTests.cs");
            designFiles = ("docs/design/nuget-installer/nuget/models.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget/models.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget/models.yaml");
        }

        part packageEntry : PackageEntry;
    }
}
