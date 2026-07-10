package NuGetInstaller {
    part def Program {
        doc /* Entry point and execution orchestrator that wires the Cli Context, PackagesConfigReader, and PackageInstaller together and returns the process exit code. */

        comment sourceRef /* Source: src/DemaConsulting.NuGetInstaller/Program.cs */
        comment testRef /* Test: test/DemaConsulting.NuGetInstaller.Tests/ProgramTests.cs */
        comment designRef /* Design: docs/design/nuget-installer/program.md */
        comment verificationRef /* Verification: docs/verification/nuget-installer/program.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-installer/program.yaml */
    }
}
