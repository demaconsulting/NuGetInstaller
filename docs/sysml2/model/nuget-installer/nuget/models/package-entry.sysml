package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def PackageEntry {
        doc /* Immutable data model representing a single package entry read from a packages.config file (id, version, optional target framework). */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/NuGet/Models/PackageEntry.cs");
            designFiles = ("docs/design/nuget-installer/nuget/models/package-entry.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget/models/package-entry.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget/models/package-entry.yaml");
        }
    }
}
