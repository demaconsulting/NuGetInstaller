package NuGetInstaller_NuGetSubsystem {
    public import NuGetInstaller_ModelsSubsystem::*;
    public import NuGetInstaller_PackagesConfigReader::*;
    public import NuGetInstaller_PackageExtractor::*;
    public import NuGetInstaller_PackageInstaller::*;
    private import SoftwareItemMetadata::*;

    part def NuGetSubsystem {
        doc /* Package management subsystem: reads packages.config, resolves each package from the NuGet global cache, and installs it into the output directory. */

        metadata info : SoftwareInfo {
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/NuGet/NuGetSubsystemTests.cs");
            designFiles = ("docs/design/nuget-installer/nuget.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget.yaml");
        }

        part models : ModelsSubsystem;
        part packagesConfigReader : PackagesConfigReader;
        part packageExtractor : PackageExtractor;
        part packageInstaller : PackageInstaller;
    }
}
