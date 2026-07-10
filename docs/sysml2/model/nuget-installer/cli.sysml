package NuGetInstaller {
    part def CliSubsystem {
        doc /* Command-line interface subsystem: parses user arguments, routes output to the console and an optional log file, and owns process I/O for the rest of the system. */

        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/Cli/CliSubsystemTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/cli.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/cli.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/cli.yaml */

        part context : Context;
    }
}
