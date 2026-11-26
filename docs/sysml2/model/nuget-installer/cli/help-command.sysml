package NuGetInstaller_HelpCommand {
    private import SoftwareItemMetadata::*;
    private import OtsDependencies::*;

    part def HelpCommand {
        doc /* Built-in help command: answers the help verb from the help cards embedded in the assembly. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGetInstaller/Cli/HelpCommand.cs");
            testFiles = ("test/DemaConsulting.NuGetInstaller.Tests/Cli/HelpCommandTests.cs");
            designFiles = ("docs/design/nuget-installer/cli/help-command.md");
            verificationFiles = ("docs/verification/nuget-installer/cli/help-command.md");
            requirementFiles = ("docs/reqstream/nuget-installer/cli/help-command.yaml");
        }
    }

    // Serves the help command from the cards embedded in the assembly
    dependency HelpCommand to HelpCards;
}
