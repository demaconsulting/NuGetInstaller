package OtsDependencies {
    private import SoftwareItemMetadata::*;

    doc /* Off-the-shelf (OTS) runtime dependencies used by NuGetInstaller. */

    part def HelpCards {
        doc /* OTS: DemaConsulting.HelpCards. */

        metadata info : SoftwareInfo {
            designFiles = ("docs/design/ots/help-cards.md");
            verificationFiles = ("docs/verification/ots/help-cards.md");
            requirementFiles = ("docs/reqstream/ots/help-cards.yaml");
        }
    }
}
