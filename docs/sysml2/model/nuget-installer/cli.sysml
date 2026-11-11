package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def CliSubsystem {
        doc /* Command-line interface subsystem: parses user arguments, routes output to the console and an optional log file, and owns process I/O for the rest of the system. */

        metadata info : SoftwareInfo {
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/Cli/CliSubsystemTests.cs");
            designFiles = ("docs/design/nuget-installer/cli.md");
            verificationFiles = ("docs/verification/nuget-installer/cli.md");
            requirementFiles = ("docs/reqstream/nuget-installer/cli.yaml");
        }

        part context : Context;
    }
}
