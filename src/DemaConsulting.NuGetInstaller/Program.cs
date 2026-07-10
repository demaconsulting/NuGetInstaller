// Copyright (c) DEMA Consulting
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
using DemaConsulting.NuGetInstaller.Cli;
using DemaConsulting.NuGetInstaller.NuGet;
using DemaConsulting.NuGetInstaller.SelfTest;

namespace DemaConsulting.NuGetInstaller;

/// <summary>
///     Main program entry point for the NuGet Installer.
/// </summary>
internal static class Program
{
    /// <summary>
    ///     Gets the application version string.
    /// </summary>
    /// <returns>
    ///     The version string read from <see cref="AssemblyInformationalVersionAttribute"/>;
    ///     falls back to the assembly's <see cref="System.Version"/> string if the informational
    ///     attribute is absent; returns <c>"0.0.0"</c> when neither attribute is available.
    /// </returns>
    public static string Version
    {
        get
        {
            // Get the assembly containing this program
            var assembly = typeof(Program).Assembly;

            // Try to get version from assembly attributes, fallback to AssemblyVersion, or default to 0.0.0
            return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                   ?? assembly.GetName().Version?.ToString()
                   ?? "0.0.0";
        }
    }

    /// <summary>
    ///     Main entry point for the NuGet Installer.
    /// </summary>
    /// <remarks>
    ///     Exception-handling strategy: <see cref="ArgumentException"/> and
    ///     <see cref="InvalidOperationException"/> are expected errors (bad arguments, missing
    ///     files, inaccessible log) — they are caught, written to stderr, and translated to
    ///     exit code 1 so callers can detect failure without a crash. All other exceptions
    ///     are unexpected; they are written to stderr and re-thrown so the .NET runtime can
    ///     generate an event-log entry and a crash dump for post-mortem analysis.
    /// </remarks>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Exit code: 0 for success, non-zero for failure.</returns>
    private static int Main(string[] args)
    {
        try
        {
            // Create context from command-line arguments
            using var context = Context.Create(args);

            // Run the program logic
            Run(context);

            // Return the exit code from the context
            return context.ExitCode;
        }
        catch (ArgumentException ex)
        {
            // Print expected argument exceptions and return error code
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
        catch (InvalidOperationException ex)
        {
            // Print expected operation exceptions and return error code
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            // Print unexpected exceptions and re-throw to generate event logs
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    ///     Runs the program logic based on the provided context.
    /// </summary>
    /// <remarks>
    ///     Dispatch priority order:
    ///     <list type="number">
    ///       <item><description>
    ///         Version flag — writes the version string and returns immediately.
    ///         <see cref="PrintBanner"/> is intentionally skipped so that the version output
    ///         contains only the version string, with no banner overhead.
    ///       </description></item>
    ///       <item><description>
    ///         PrintBanner — called unconditionally for all other paths so every invocation
    ///         announces the tool name and version before doing useful work.
    ///       </description></item>
    ///       <item><description>
    ///         Help flag — calls <see cref="PrintHelp"/> and returns.
    ///       </description></item>
    ///       <item><description>
    ///         Validate flag — delegates to <see cref="Validation.Run"/>.
    ///       </description></item>
    ///       <item><description>
    ///         Otherwise — delegates to <see cref="RunToolLogic"/> for package installation.
    ///       </description></item>
    ///     </list>
    /// </remarks>
    /// <param name="context">The context containing command line arguments and program state.</param>
    public static void Run(Context context)
    {
        // Priority 1: Version query
        if (context.Version)
        {
            context.WriteLine(Version);
            return;
        }

        // Print application banner
        PrintBanner(context);

        // Priority 2: Help
        if (context.Help)
        {
            PrintHelp(context);
            return;
        }

        // Priority 3: Self-Validation
        if (context.Validate)
        {
            Validation.Run(context);
            return;
        }

        // Priority 4: Main tool functionality
        RunToolLogic(context);
    }

    /// <summary>
    ///     Prints the application banner.
    /// </summary>
    /// <remarks>
    ///     Called by <see cref="Run"/> before dispatching to <see cref="PrintHelp"/>,
    ///     <see cref="Validation.Run"/>, or <see cref="RunToolLogic"/>. Intentionally
    ///     not called when the Version flag is set — the version path returns before
    ///     reaching this method so that <c>--version</c> output contains only the version
    ///     string with no additional decoration.
    /// </remarks>
    /// <param name="context">The context for output.</param>
    private static void PrintBanner(Context context)
    {
        context.WriteLine($"NuGet Installer version {Version}");
        context.WriteLine("Copyright (c) DEMA Consulting");
        context.WriteLine("");
    }

    /// <summary>
    ///     Prints usage information.
    /// </summary>
    /// <remarks>
    ///     Called by <see cref="Run"/> when the Help flag is set, immediately after
    ///     <see cref="PrintBanner"/>. Lists all supported command-line options so that
    ///     users can self-serve without consulting external documentation.
    /// </remarks>
    /// <param name="context">The context for output.</param>
    private static void PrintHelp(Context context)
    {
        context.WriteLine("Usage: nuget-installer [packages.config] [options]");
        context.WriteLine("");
        context.WriteLine("Options:");
        context.WriteLine("  [packages.config]            Path to packages.config (default: packages.config)");
        context.WriteLine("  -x, -ExcludeVersion          Name output folder {Id}/ instead of {Id}.{Version}/");
        context.WriteLine("  -o, -OutputDirectory <dir>   Output directory (default: current directory)");
        context.WriteLine("  -v, --version                Display version information");
        context.WriteLine("  -?, -h, --help               Display this help message");
        context.WriteLine("  --silent                     Suppress console output");
        context.WriteLine("  --validate                   Run self-validation");
        context.WriteLine("  --results, --result <file>   Write validation results to file (.trx or .xml)");
        context.WriteLine("  --depth <#>                  Set heading depth for markdown output (default: 1)");
        context.WriteLine("  --log <file>                 Write output to log file");
    }

    /// <summary>
    ///     Runs the main tool logic.
    /// </summary>
    /// <remarks>
    ///     Four-step package installation workflow:
    ///     <list type="number">
    ///       <item><description>
    ///         Verify <see cref="Cli.Context.PackagesConfigFile"/> exists; report an error
    ///         via <see cref="Cli.Context.WriteError"/> and return early if not found.
    ///       </description></item>
    ///       <item><description>
    ///         Read the package list from the config file via
    ///         <see cref="PackagesConfigReader.Read"/>.
    ///       </description></item>
    ///       <item><description>
    ///         Resolve the output directory from <see cref="Cli.Context.OutputDirectory"/>,
    ///         defaulting to the current working directory when the flag is absent.
    ///       </description></item>
    ///       <item><description>
    ///         Install all packages via <see cref="PackageInstaller.InstallAsync"/>,
    ///         forwarding <see cref="Cli.Context.ExcludeVersion"/> to control whether output
    ///         folders use <c>{Id}.{Version}/</c> or <c>{Id}/</c> naming.
    ///       </description></item>
    ///     </list>
    /// </remarks>
    /// <param name="context">The context containing command line arguments and program state.</param>
    private static void RunToolLogic(Context context)
    {
        // Resolve and validate packages.config path
        if (!File.Exists(context.PackagesConfigFile))
        {
            context.WriteError($"packages.config not found: {context.PackagesConfigFile}");
            return;
        }

        // Read packages
        var packages = PackagesConfigReader.Read(context.PackagesConfigFile);
        context.WriteLine($"Found {packages.Count} package(s) in {context.PackagesConfigFile}");

        // Resolve output directory
        var outputDirectory = context.OutputDirectory ?? Directory.GetCurrentDirectory();

        // Resolve the directory containing packages.config so a project/repo-local nuget.config
        // is discovered the same way `dotnet restore` discovers it
        var configRoot = Path.GetDirectoryName(Path.GetFullPath(context.PackagesConfigFile));

        // Install
        PackageInstaller.InstallAsync(context, packages, outputDirectory, context.ExcludeVersion, configRoot)
            .GetAwaiter().GetResult();
    }
}
