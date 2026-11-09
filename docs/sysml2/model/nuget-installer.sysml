package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    doc
    /*
     * NuGetInstaller is a cross-platform .NET command-line tool that installs
     * the NuGet packages listed in a `packages.config` file into a local output
     * directory, mirroring the behavior of `nuget.exe install`.
     */

    part def NuGetInstallerSystem {
        doc /* The NuGet Installer system as a whole. */

        metadata info : SoftwareInfo {
            description = "The NuGet Installer system as a whole.";
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/IntegrationTests.cs");
            designFiles = ("docs/design/nuget-installer.md");
            verificationFiles = ("docs/verification/nuget-installer.md");
            requirementFiles = ("docs/reqstream/nuget-installer.yaml");
        }

        part program : Program;
        part cli : CliSubsystem;
        part nuget : NuGetSubsystem;
        part selfTest : SelfTestSubsystem;
        part utilities : UtilitiesSubsystem;
    }
}
