package NuGetInstaller {
    part def SelfTestSubsystem {
        doc /* Self-validation subsystem: runs a built-in suite of tests that prove the tool is operational in the deployment environment. */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/SelfTest/SelfTestSubsystemTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/self-test.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/self-test.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/self-test.yaml */

        part validation : Validation;
    }
}
