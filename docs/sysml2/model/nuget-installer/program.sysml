package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def Program {
        doc /* Entry point and execution orchestrator that wires the Cli Context, PackagesConfigReader, and PackageInstaller together and returns the process exit code. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/Program.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/ProgramTests.cs");
            designFiles = ("docs/design/nuget-installer/program.md");
            verificationFiles = ("docs/verification/nuget-installer/program.md");
            requirementFiles = ("docs/reqstream/nuget-installer/program.yaml");
        }
    }
}
