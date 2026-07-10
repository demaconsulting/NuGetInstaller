package NuGetInstaller {
    part def PackageExtractor {
        doc /* ZIP extraction wrapper for .nupkg files that safely writes package contents into a destination folder. */

        comment sourceRef /* Source: src/DemaConsulting.NuGetInstaller/NuGet/PackageExtractor.cs */
        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/NuGet/PackageExtractorTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/nuget/package-extractor.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/nuget/package-extractor.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/nuget/package-extractor.yaml */
    }
}
