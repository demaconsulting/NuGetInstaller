package NuGetInstaller {
    private import SoftwareItemMetadata::*;

    part def Context {
        doc /* Command-line argument parser and I/O owner: exposes parsed inputs, output/error writers, and an optional log-file sink to the rest of the system. */

        metadata info : SoftwareInfo {
            description = "Command-line argument parser and I/O owner: exposes parsed inputs, output/error writers, and an optional log-file sink to the rest of the system.";
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/Cli/Context.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/Cli/ContextTests.cs");
            designFiles = ("docs/design/nuget-installer/cli/context.md");
            verificationFiles = ("docs/verification/nuget-installer/cli/context.md");
            requirementFiles = ("docs/reqstream/nuget-installer/cli/context.yaml");
        }
    }
}
