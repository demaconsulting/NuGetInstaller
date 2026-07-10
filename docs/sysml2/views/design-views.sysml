// Views rendered into docs/design/generated/ for the Design document.
// Reopens the NuGetInstaller package so each 'expose' statement can reference
// its target part def directly. Per SysML2Tools syntax, 'expose' is only valid
// inside a named 'view' usage (not a 'view def' definition), and it - not
// 'render' - is what scopes the rendered diagram content.
package NuGetInstaller {
    view SoftwareStructureView {
        expose NuGetInstaller;
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
