package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def PackageExtractor {
        doc /* ZIP extraction wrapper for .nupkg files that safely writes package contents into a destination folder. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/NuGet/PackageExtractor.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/NuGet/PackageExtractorTests.cs");
            designFiles = ("docs/design/nuget-installer/nuget/package-extractor.md");
            verificationFiles = ("docs/verification/nuget-installer/nuget/package-extractor.md");
            requirementFiles = ("docs/reqstream/nuget-installer/nuget/package-extractor.yaml");
        }
    }
}
