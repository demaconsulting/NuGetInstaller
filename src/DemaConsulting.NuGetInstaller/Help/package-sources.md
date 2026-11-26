---
title: Package sources and nuget.config
summary: Where the packages come from, how to add a private feed, and the package cache.
aliases:
  - nuget-config
  - sources
  - feeds
  - credentials
  - authentication
  - cache
tags:
  - reference
order: 4
see-also:
  - packages-config
  - troubleshooting
examples:
  - name: full
    kind: source
    language: xml
    save-as: nuget.config
    description: Use nuget.org and a private feed that needs a token.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <configuration>
        <packageSources>
          <clear />
          <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          <add key="company" value="https://pkgs.example.com/nuget/v3/index.json" />
        </packageSources>
        <packageSourceCredentials>
          <company>
            <add key="Username" value="ci" />
            <add key="ClearTextPassword" value="%FEED_TOKEN%" />
          </company>
        </packageSourceCredentials>
        <packageSourceMapping>
          <packageSource key="nuget.org">
            <package pattern="*" />
          </packageSource>
          <packageSource key="company">
            <package pattern="Example.*" />
          </packageSource>
        </packageSourceMapping>
      </configuration>
---
The tool downloads packages with the NuGet client, so it uses the same
nuget.config files as `dotnet restore`.

**Discovery.** The tool reads the nuget.config file in the folder of the
packages.config file and in every folder above it, then the user
configuration: `%AppData%\NuGet\NuGet.Config` on Windows and
`~/.nuget/NuGet/NuGet.Config` on Linux and macOS. Without a nuget.config
of your own, the sources of the user configuration apply, which is
normally nuget.org. `<clear />` drops the sources of the folders above
and of the user configuration, as in [[#full]].

**Local feeds.** A source can be a folder of `.nupkg` files. A relative
path is relative to the folder of the nuget.config file that names it:

```xml
<add key="local" value="feed" />
```

**Credentials.** For a private feed, add `packageSourceCredentials` for
the key of its source, as [[#full]] shows. NuGet replaces
`%FEED_TOKEN%` with the environment variable `FEED_TOKEN`, so CI can pass
a secret token without writing it to the file. Never commit a token. A
credential provider, such as the Azure Artifacts credential provider, is
used when it is installed.

**Source mapping.** With `packageSourceMapping`, a package is only looked
for on the sources that are mapped to its id. In [[#full]],
`Example.*` packages come from `company` and all others from nuget.org.

**Cache.** Every package is first stored in the NuGet global packages
folder, normally `~/.nuget/packages`, and reused from there, so a second
run needs no network. The environment variable `NUGET_PACKAGES` or the
`globalPackagesFolder` setting of nuget.config moves this folder.

**Self-validation.** `--validate` uses the nuget.config files of the
current folder, not those of a packages.config file; see
[[self-validation]].
