package NuGetInstaller {
    part def NuGetSubsystem {
        doc /* Package management subsystem: reads packages.config, resolves each package from the NuGet global cache, and installs it into the output directory. */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/NuGet/NuGetSubsystemTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/nuget.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/nuget.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/nuget.yaml */

        part models : ModelsSubsystem;
        part packagesConfigReader : PackagesConfigReader;
        part packageExtractor : PackageExtractor;
        part packageInstaller : PackageInstaller;
    }
}
