package NuGetInstaller {
    doc
    /*
     * NuGetInstaller is a cross-platform .NET command-line tool that installs
     * the NuGet packages listed in a `packages.config` file into a local output
     * directory, mirroring the behavior of `nuget.exe install`.
     */

    part def NuGetInstallerSystem {
        doc /* The NuGet Installer system as a whole. */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/IntegrationTests.cs */
        comment designRef /* Design: docs/design/nuget-installer.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer.yaml */

        part program : Program;
        part cli : CliSubsystem;
        part nuget : NuGetSubsystem;
        part selfTest : SelfTestSubsystem;
        part utilities : UtilitiesSubsystem;
    }
}
