package NuGetInstaller {
    part def UtilitiesSubsystem {
        doc /* Shared utility subsystem: general-purpose helpers reused by other subsystems (path safety, etc.). */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/Utilities/UtilitiesSubsystemTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/utilities.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/utilities.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/utilities.yaml */

        part pathHelpers : PathHelpers;
    }
}
