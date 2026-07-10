package NuGetInstaller {
    part def ModelsSubsystem {
        doc /* Data models carried through the NuGet subsystem pipeline: immutable records that describe each package to install. */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/NuGet/Models/ModelsSubsystemTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/nuget/models.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/nuget/models.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/nuget/models.yaml */

        part packageEntry : PackageEntry;
    }
}
