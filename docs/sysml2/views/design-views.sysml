// Views rendered into docs/design/generated/ for the Design document.
// Per SysML2Tools syntax, 'expose' is only valid inside a named 'view' usage
// (not a 'view def' definition), and it - not 'render' - is what scopes the
// rendered diagram content.
package NuGetInstallerDesignViews {
    private import StandardViewDefinitions::*;
    private import NuGetInstaller::*;

    view SoftwareStructureView : InterconnectionView {
        expose NuGetInstaller::NuGetInstallerSystem;
    }

    view NuGetInstallerView {
        expose NuGetInstallerSystem;
    }

    view CliView {
        expose CliSubsystem;
    }

    view NuGetView {
        expose NuGetSubsystem;
    }

    view NuGetModelsView {
        expose ModelsSubsystem;
    }

    view SelfTestView {
        expose SelfTestSubsystem;
    }

    view UtilitiesView {
        expose UtilitiesSubsystem;
    }
}
