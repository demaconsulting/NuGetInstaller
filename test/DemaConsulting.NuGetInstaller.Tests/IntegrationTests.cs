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

using DemaConsulting.NuGetInstaller.Utilities;

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     Integration tests that run the NuGet Installer application through dotnet.
/// </summary>
[TestClass]
public class IntegrationTests
{
    private string _dllPath = string.Empty;

    /// <summary>
    ///     Initialize test by locating the NuGet Installer DLL.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        // The DLL should be in the same directory as the test assembly
        // because the test project references the main project
        var baseDir = AppContext.BaseDirectory;
        _dllPath = PathHelpers.SafePathCombine(baseDir, "DemaConsulting.NuGetInstaller.dll");

        Assert.IsTrue(File.Exists(_dllPath), $"Could not find NuGet Installer DLL at {_dllPath}");
    }

    /// <summary>
    ///     Test that version flag outputs version information.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_VersionFlag_FlagProvided_OutputsVersion()
    {
        // Act: run the tool with version flag
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--version");

        // Assert: verify version output and success exit code
        Assert.AreEqual(0, exitCode);
        Assert.IsFalse(string.IsNullOrWhiteSpace(output));
        Assert.DoesNotContain("Error", output);
        Assert.DoesNotContain("Copyright", output);
    }

    /// <summary>
    ///     Test that help flag outputs usage information.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_HelpFlag_FlagProvided_OutputsUsageInformation()
    {
        // Act: run the tool with the --help flag
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--help");

        // Assert: verify expected behavior
        Assert.AreEqual(0, exitCode);
        Assert.Contains("Usage:", output);
        Assert.Contains("Options:", output);
        Assert.Contains("--version", output);
    }

    /// <summary>
    ///     Test that validate flag runs self-validation.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_ValidateFlag_FlagProvided_RunsValidation()
    {
        // Act: run the tool with the --validate flag
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--validate");

        // Assert: verify expected behavior
        Assert.AreEqual(0, exitCode);
        Assert.Contains("Total Tests:", output);
        Assert.Contains("Passed:", output);
    }

    /// <summary>
    ///     Test that validate with results flag generates TRX file.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_ValidateWithResults_TrxExtension_GeneratesTrxFile()
    {
        // Arrange: setup test conditions
        var resultsFile = Path.Combine(Path.GetTempPath(), $"integration_test_{Guid.NewGuid()}.trx");

        try
        {
            // Act: run the tool with --validate and --results flags (TRX file)
            var exitCode = Runner.Run(
                out var _,
                null,
                "dotnet",
                _dllPath,
                "--validate",
                "--results",
                resultsFile);

            // Assert: verify expected behavior
            Assert.AreEqual(0, exitCode);
            Assert.IsTrue(File.Exists(resultsFile), "Results file was not created");

            var trxContent = File.ReadAllText(resultsFile);
            Assert.Contains("<TestRun", trxContent);
            Assert.Contains("</TestRun>", trxContent);
        }
        finally
        {
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

    /// <summary>
    ///     Test that silent flag suppresses output.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_SilentFlag_FlagProvided_SuppressesOutput()
    {
        // Act: run the tool with --silent and --version flags
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--silent",
            "--version");

        // Assert: verify expected behavior
        Assert.AreEqual(0, exitCode);
        Assert.IsTrue(string.IsNullOrWhiteSpace(output), $"Expected no output but got: {output}");
    }

    /// <summary>
    ///     Test that log flag writes output to file.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_LogFlag_FlagProvided_WritesOutputToFile()
    {
        // Arrange: setup test conditions
        var logFile = Path.GetTempFileName();

        try
        {
            // Act: execute the operation being tested (with --version to produce known output)
            var exitCode = Runner.Run(
                out var _,
                null,
                "dotnet",
                _dllPath,
                "--log",
                logFile,
                "--version");

            // Assert: verify expected behavior
            Assert.AreEqual(0, exitCode);
            Assert.IsTrue(File.Exists(logFile), "Log file was not created");

            var logContent = File.ReadAllText(logFile);
            Assert.IsFalse(string.IsNullOrWhiteSpace(logContent), "Log file should contain output");
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }

    /// <summary>
    ///     Test that validate with results flag generates JUnit XML file.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_ValidateWithResults_XmlExtension_GeneratesJUnitFile()
    {
        // Arrange: setup test conditions
        var resultsFile = Path.Combine(Path.GetTempPath(), $"integration_test_{Guid.NewGuid()}.xml");

        try
        {
            // Act: run the tool with --validate and --results flags (JUnit XML file)
            var exitCode = Runner.Run(
                out var _,
                null,
                "dotnet",
                _dllPath,
                "--validate",
                "--results",
                resultsFile);

            // Assert: verify expected behavior
            Assert.AreEqual(0, exitCode);
            Assert.IsTrue(File.Exists(resultsFile), "Results file was not created");

            var xmlContent = File.ReadAllText(resultsFile);
            Assert.Contains("<testsuites", xmlContent);
        }
        finally
        {
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

    /// <summary>
    ///     Test that unknown argument returns error.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_UnknownArgument_InvalidFlag_ReturnsNonZeroExitCode()
    {
        // Act: run the tool with an unknown --unknown flag
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--unknown");

        // Assert: verify expected behavior
        Assert.AreNotEqual(0, exitCode);
        Assert.Contains("Error", output);
    }

    /// <summary>
    ///     Test that the tool installs NuGet packages from a packages.config file into the output directory.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_InstallPackages_ValidConfig_ExtractsPackageToOutputDirectory()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"integration_install_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            File.WriteAllText(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """);

            // Act: run the tool to install packages into the temp directory
            var exitCode = Runner.Run(
                out var output,
                null,
            "dotnet",
                _dllPath,
                configPath,
                "-o",
                tempDir);

            // Assert: verify the tool exits successfully and the package is extracted
            Assert.AreEqual(0, exitCode, $"Tool should succeed. Output: {output}");
            Assert.Contains("Installed", output, "Output should confirm package installation");

            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                $"Package folder should exist at {expectedFolder}");
            Assert.IsNotEmpty(Directory.GetFileSystemEntries(expectedFolder),
                "Package folder should contain extracted files");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that the tool uses {Id}/ folder naming when exclude-version flag is provided.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_InstallPackages_ExcludeVersionFlag_UsesFlatFolderNaming()
    {
        // Arrange: create a temporary directory with a packages.config file
        var tempDir = Path.Combine(Path.GetTempPath(), $"integration_excludeversion_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            File.WriteAllText(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """);

            // Act: run the tool with -x (exclude version) flag
            var exitCode = Runner.Run(
                out var output,
                null,
            "dotnet",
                _dllPath,
                configPath,
                "-o",
                tempDir,
                "-x");

            // Assert: package folder uses {Id}/ naming, not {Id}.{Version}/
            Assert.AreEqual(0, exitCode, $"Tool should succeed. Output: {output}");
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                $"Version-less package folder should exist at {expectedFolder}");
            var versionedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.IsFalse(Directory.Exists(versionedFolder),
                "Versioned package folder should not exist when -x flag is used");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that validate with depth flag adjusts heading depth of output.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_ValidateDepth_DepthFlagProvided_AdjustsHeadingDepth()
    {
        // Act: run with --validate and --depth 2
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--validate",
            "--depth",
            "2");

        // Assert: output contains level-2 heading
        Assert.AreEqual(0, exitCode, $"Tool should succeed. Output: {output}");
        Assert.Contains("## ", output, "Validation output should use depth-2 heading");
    }

    /// <summary>
    ///     Test that the tool skips packages whose output folder already exists.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_InstallPackages_ExistingFolder_SkipsInstallation()
    {
        // Arrange: create a temporary directory, pre-create the package output folder
        var tempDir = Path.Combine(Path.GetTempPath(), $"integration_skip_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            File.WriteAllText(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """);

            // Pre-create the output folder to simulate an already-installed package
            var existingFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Directory.CreateDirectory(existingFolder);

            // Act: run the tool - it should skip the already-existing folder
            var exitCode = Runner.Run(
                out var output,
                null,
            "dotnet",
                _dllPath,
                configPath,
                "-o",
                tempDir);

            // Assert: exit code is 0 and the tool reports skipping
            Assert.AreEqual(0, exitCode, $"Tool should succeed even when skipping. Output: {output}");
            Assert.Contains("Skipping", output, "Tool should report that the package was skipped");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that the tool installs packages into the current working directory when no -o flag is given.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_InstallPackages_DefaultOutputDirectory_InstallsToCurrentDirectory()
    {
        // Arrange: create a temporary directory to act as the working directory
        var tempDir = Path.Combine(Path.GetTempPath(), $"integration_cwd_test_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "packages.config");
            File.WriteAllText(configPath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <packages>
                  <package id="DemaConsulting.NuGet.Caching" version="1.0.0" />
                </packages>
                """);

            // Act: run with tempDir as the working directory so the tool defaults output there
            var exitCode = Runner.Run(
                out var output,
                tempDir,
                "dotnet",
                _dllPath,
                configPath);

            // Assert: tool exits successfully and the package is extracted to the working directory
            Assert.AreEqual(0, exitCode, $"Tool should succeed. Output: {output}");
            var expectedFolder = Path.Combine(tempDir, "DemaConsulting.NuGet.Caching.1.0.0");
            Assert.IsTrue(Directory.Exists(expectedFolder),
                $"Package folder should exist in the working directory at {expectedFolder}");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    /// <summary>
    ///     Test that the tool reports an error message for unrecognized arguments.
    /// </summary>
    [TestMethod]
    public void NuGetInstaller_UnknownArgument_InvalidFlag_ReportsErrorMessage()
    {
        // Act: run the tool with an unknown --unknown flag
        var exitCode = Runner.Run(
            out var output,
            null,
            "dotnet",
            _dllPath,
            "--unknown");

        // Assert: tool reports an error message identifying the unrecognized flag
        Assert.AreNotEqual(0, exitCode, "Tool should return non-zero exit code for unknown flag");
        Assert.Contains("--unknown", output, "Error message should identify the unrecognized flag");
    }
}

