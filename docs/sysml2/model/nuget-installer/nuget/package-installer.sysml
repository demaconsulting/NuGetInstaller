package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def PackageInstaller {
        doc /* Parallel install orchestrator that resolves each PackageEntry from the NuGet global cache and drives PackageExtractor to install it into the output directory. */

        metadata info : SoftwareInfo {
            description = "Parallel install orchestrator that resolves each PackageEntry from the NuGet global cache and drives PackageExtractor to install it into the output directory.";
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/NuGet/PackageInstaller.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/NuGet/PackageInstallerTests.cs");
            designFiles = ("docs/design/nuget-installer/nuget/package-installer.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget/package-installer.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget/package-installer.yaml");
        }
    }
}
