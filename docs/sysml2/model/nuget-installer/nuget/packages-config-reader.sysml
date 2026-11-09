package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def PackagesConfigReader {
        doc /* Reads and parses a packages.config XML file into a list of PackageEntry records. */

        metadata info : SoftwareInfo {
            description = "Reads and parses a packages.config XML file into a list of PackageEntry records.";
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/NuGet/PackagesConfigReader.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/NuGet/PackagesConfigReaderTests.cs");
            designFiles = ("docs/design/nuget-installer/nuget/packages-config-reader.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget/packages-config-reader.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget/packages-config-reader.yaml");
        }
    }
}
