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

namespace DemaConsulting.NuGetInstaller.Tests.Cli;

/// <summary>
///     Subsystem tests for the CLI subsystem covering Context and Program integration.
/// </summary>
[TestClass]
public class CliSubsystemTests
{
    /// <summary>
    ///     Test that Context and Program work together to handle version flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits()
    {
        // Arrange: command line arguments with version flag; capture console output
        var args = new[] { "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: version flag is parsed, version text is displayed, and exit code is success
            Assert.IsTrue(context.Version, "Context should parse version flag");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            Assert.Contains(Program.Version, capturedOut.ToString(), "Console output should contain the program version");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle help flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits()
    {
        // Arrange: command line arguments with help flag; capture console output
        var args = new[] { "--help" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: help flag is parsed, usage text is displayed, and exit code is success
            Assert.IsTrue(context.Help, "Context should parse help flag");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            var output = capturedOut.ToString();
            Assert.Contains("Usage:", output, "Console output should contain usage information");
            Assert.Contains("Options:", output, "Console output should contain options information");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the -? short help flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortQuestionFlag()
    {
        // Arrange: command line arguments with -? short help flag; capture console output
        var args = new[] { "-?" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: help flag is parsed, usage text is displayed, and exit code is success
            Assert.IsTrue(context.Help, "Context should parse -? flag as help");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            var output = capturedOut.ToString();
            Assert.Contains("Usage:", output, "Console output should contain usage information");
            Assert.Contains("Options:", output, "Console output should contain options information");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the -h short help flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_HelpFlow_ContextAndProgram_DisplaysHelpAndExits_WithShortHFlag()
    {
        // Arrange: command line arguments with -h short help flag; capture console output
        var args = new[] { "-h" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: help flag is parsed, usage text is displayed, and exit code is success
            Assert.IsTrue(context.Help, "Context should parse -h flag as help");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            var output = capturedOut.ToString();
            Assert.Contains("Usage:", output, "Console output should contain usage information");
            Assert.Contains("Options:", output, "Console output should contain options information");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle validation flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ValidateFlow_ContextAndProgram_RunsValidationAndExits()
    {
        // Arrange: command line arguments with validate flag
        var args = new[] { "--validate" };

        // Act: create context and run program logic
        using var context = Context.Create(args);
        Program.Run(context);

        // Assert: validate flag is parsed and exit code is success
        Assert.IsTrue(context.Validate, "Context should parse validate flag");
        Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
    }

    /// <summary>
    ///     Test that Context and Program work together to handle silent flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_SilentFlow_ContextAndProgram_SuppressesOutput()
    {
        // Arrange: command line arguments with version and silent flags; capture console streams
        var args = new[] { "--version", "--silent" };
        var originalOut = Console.Out;
        var originalError = Console.Error;
        using var capturedOut = new StringWriter();
        using var capturedError = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);
            Console.SetError(capturedError);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: silent flag is parsed, exit code is success, and no console output is produced
            Assert.IsTrue(context.Silent, "Context should parse silent flag");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            Assert.AreEqual(string.Empty, capturedOut.ToString(), "Program should not write to stdout when --silent is set");
            Assert.AreEqual(string.Empty, capturedError.ToString(), "Program should not write to stderr when --silent is set");
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle results flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ResultsFlow_ContextAndProgram_WritesResultsFile()
    {
        // Arrange: temporary results file path and validation command with results output
        var tempDir = Path.GetTempPath();
        var resultsFile = Path.Combine(tempDir, $"cli_test_{Guid.NewGuid()}.trx");
        var args = new[] { "--validate", "--silent", "--results", resultsFile };

        try
        {
            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: results flag is parsed, validation runs, and results file is written
            Assert.AreEqual(resultsFile, context.ResultsFile, "Context should parse results file path");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
            Assert.IsTrue(File.Exists(resultsFile), "Results file should be written to specified path");
        }
        finally
        {
            // Cleanup
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle log flag workflow.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_LogFlow_ContextAndProgram_WritesLogFile()
    {
        // Arrange: temporary log file path and version command with log output
        var tempDir = Path.GetTempPath();
        var logFile = Path.Combine(tempDir, $"cli_test_{Guid.NewGuid()}.log");
        var args = new[] { "--version", "--log", logFile };

        try
        {
            // Act: create context and run program logic
            using (var context = Context.Create(args))
            {
                Program.Run(context);

                // Assert: version flag is parsed and exit code is success
                Assert.IsTrue(context.Version, "Context should parse version flag");
                Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
            }

            // Assert: log file is written with version output
            Assert.IsTrue(File.Exists(logFile), "Log file should be created at specified path");
            var logContent = File.ReadAllText(logFile);
            Assert.IsFalse(string.IsNullOrWhiteSpace(logContent), "Log file should contain version output");
        }
        finally
        {
            // Cleanup
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }

    /// <summary>
    ///     Test that Program rejects unknown arguments, writes an error to stderr, and exits non-zero.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_InvalidArgs_ContextAndProgram_RejectsUnknownArgumentsAndExitsNonZero()
    {
        // Arrange: unknown command-line argument and reflection to access private Program.Main
        var args = new[] { "--unknown-flag" };
        var mainMethod = typeof(Program).GetMethod(
            "Main",
            BindingFlags.Static | BindingFlags.NonPublic,
            binder: null,
            types: [typeof(string[])],
            modifiers: null);

        Assert.IsNotNull(mainMethod, "Program.Main(string[]) should exist");

        var originalError = Console.Error;
        try
        {
            using var errWriter = new StringWriter();
            Console.SetError(errWriter);

            // Act: invoke the actual CLI entry point with an unknown flag
            var result = mainMethod.Invoke(null, [args]);

            // Assert: invalid arguments produce a non-zero exit code and an error on stderr
            Assert.IsNotNull(result, "Program.Main should return an exit code");
            Assert.AreEqual(1, Convert.ToInt32(result), "Unknown arguments should cause exit code 1");
            var errorOutput = errWriter.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(errorOutput), "Program should write an error to stderr for unknown arguments");
            Assert.Contains("--unknown-flag", errorOutput);
        }
        finally
        {
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that Context writes error messages to stderr.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ErrorOutput_ContextAndProgram_WritesErrorToStderr()
    {
        // Arrange: redirect stderr to capture error output
        var originalError = Console.Error;
        try
        {
            using var errWriter = new StringWriter();
            Console.SetError(errWriter);
            using var context = Context.Create([]);

            // Act: write an error message through the context
            context.WriteError("Test error message");

            // Assert: error is written to stderr and exit code reflects failure
            var errorOutput = errWriter.ToString();
            Assert.Contains("Test error message", errorOutput);
            Assert.AreEqual(1, context.ExitCode, "Exit code should be non-zero after error");
        }
        finally
        {
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the --result legacy alias for results.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ResultAliasFlow_ContextAndProgram_WritesResultsFile()
    {
        // Arrange: temporary results file path and validation command with legacy --result alias
        var tempDir = Path.GetTempPath();
        var resultsFile = Path.Combine(tempDir, $"cli_test_{Guid.NewGuid()}.trx");
        var args = new[] { "--validate", "--silent", "--result", resultsFile };

        try
        {
            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: legacy --result alias is parsed, validation runs, and results file is written
            Assert.AreEqual(resultsFile, context.ResultsFile, "Context should parse results file path via --result alias");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
            Assert.IsTrue(File.Exists(resultsFile), "Results file should be written to specified path");
        }
        finally
        {
            // Cleanup
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle depth flag with self-validation.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_DepthFlow_ContextAndProgram_AdjustsHeadingDepth()
    {
        // Arrange: command line with --validate, --depth 2, and a log file to capture output
        var tempDir = Path.GetTempPath();
        var logFile = Path.Combine(tempDir, $"cli_test_{Guid.NewGuid()}.log");
        var args = new[] { "--validate", "--silent", "--depth", "2", "--log", logFile };

        try
        {
            // Act: create context and run program logic
            using (var context = Context.Create(args))
            {
                Program.Run(context);

                // Assert: depth is parsed correctly
                Assert.AreEqual(2, context.HeadingDepth, "Context should parse depth value");
                Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
            }

            // Assert: log contains level-2 heading
            var logContent = File.ReadAllText(logFile);
            Assert.Contains("## DEMA Consulting", logContent, "Validation output should use depth-2 heading");
        }
        finally
        {
            // Cleanup
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to accept a positional packages.config argument.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_PackagesConfigFlow_ContextAndProgram_AcceptsPositionalArgument()
    {
        // Arrange: command line with a positional argument and --version to avoid file lookup
        var args = new[] { "custom.config", "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: positional argument is parsed as packages.config path
            Assert.AreEqual("custom.config", context.PackagesConfigFile,
                "Context should parse positional argument as packages.config path");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle -o output directory flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory()
    {
        // Arrange: command line with -o flag and --version to avoid file lookup
        var args = new[] { "-o", "./packages", "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: output directory is parsed from -o flag
            Assert.AreEqual("./packages", context.OutputDirectory,
                "Context should parse -o flag as output directory");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle -x exclude version flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion()
    {
        // Arrange: command line with -x flag and --version to avoid file lookup
        var args = new[] { "-x", "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: exclude version flag is parsed
            Assert.IsTrue(context.ExcludeVersion,
                "Context should parse -x flag as exclude version");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the -v short version flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_VersionFlow_ContextAndProgram_DisplaysVersionAndExits_WithShortVFlag()
    {
        // Arrange: command line arguments with -v short version flag; capture console output
        var args = new[] { "-v" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: version flag is parsed, version text is displayed, and exit code is success
            Assert.IsTrue(context.Version, "Context should parse -v flag as version");
            Assert.AreEqual(0, context.ExitCode, "Context should have success exit code");
            Assert.Contains(Program.Version, capturedOut.ToString(), "Console output should contain the program version");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the -OutputDirectory long flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_OutputDirectoryFlow_ContextAndProgram_SetsOutputDirectory_WithLongFlag()
    {
        // Arrange: command line arguments with -OutputDirectory long flag and --version to avoid file lookup
        var args = new[] { "-OutputDirectory", "./packages", "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: output directory is parsed from -OutputDirectory flag
            Assert.AreEqual("./packages", context.OutputDirectory,
                "Context should parse -OutputDirectory flag as output directory");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Context and Program work together to handle the -ExcludeVersion long flag.
    /// </summary>
    [TestMethod]
    public void CliSubsystem_ExcludeVersionFlow_ContextAndProgram_SetsExcludeVersion_WithLongFlag()
    {
        // Arrange: command line arguments with -ExcludeVersion long flag and --version to avoid file lookup
        var args = new[] { "-ExcludeVersion", "--version" };
        var originalOut = Console.Out;
        using var capturedOut = new StringWriter();

        try
        {
            Console.SetOut(capturedOut);

            // Act: create context and run program logic
            using var context = Context.Create(args);
            Program.Run(context);

            // Assert: exclude version flag is parsed from -ExcludeVersion flag
            Assert.IsTrue(context.ExcludeVersion,
                "Context should parse -ExcludeVersion flag as exclude version");
            Assert.AreEqual(0, context.ExitCode, "Program should complete successfully");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}

