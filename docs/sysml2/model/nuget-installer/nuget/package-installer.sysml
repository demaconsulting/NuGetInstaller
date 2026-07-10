package NuGetInstaller {
    part def PackageInstaller {
        doc /* Parallel install orchestrator that resolves each PackageEntry from the NuGet global cache and drives PackageExtractor to install it into the output directory. */

        comment sourceRef /* Source: src/DemaConsulting.NuGetInstaller/NuGet/PackageInstaller.cs */
        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/NuGet/PackageInstallerTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/nuget/package-installer.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/nuget/package-installer.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/nuget/package-installer.yaml */
    }
}
