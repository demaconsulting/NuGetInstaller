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

using DemaConsulting.NuGetInstaller.Cli;
using DemaConsulting.NuGetInstaller.SelfTest;

namespace DemaConsulting.NuGetInstaller.Tests.SelfTest;

/// <summary>
///     Unit tests for the Validation class.
/// </summary>
[TestClass]
public class ValidationTests
{
    /// <summary>
    ///     Test that Run throws ArgumentNullException when context is null.
    /// </summary>
    [TestMethod]
    public void Validation_Run_NullContext_ThrowsArgumentNullException()
    {
        // Arrange: setup test conditions
        // No setup required — null is the input under test.

        // Act & Assert: invoke Run with null context and verify ArgumentNullException is thrown
        Assert.ThrowsExactly<ArgumentNullException>(() => Validation.Run(null!));
    }

    /// <summary>
    ///     Test that Run prints a summary containing total, passed, and failed counts.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithSilentContext_PrintsSummary()
    {
        // Arrange: setup unique log file path to capture silent context output
        var logFile = Path.Combine(Path.GetTempPath(), $"validation_test_{Guid.NewGuid()}.log");
        try
        {
            using (var context = Context.Create(["--silent", "--log", logFile]))
            {
                // Act: run validation with silent context and log file
                Validation.Run(context);
            }

            // Assert: verify summary lines are written to log file
            var logContent = File.ReadAllText(logFile);
            Assert.Contains("Total Tests:", logContent);
            Assert.Contains("Passed:", logContent);
            Assert.Contains("Failed:", logContent);
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
    ///     Test that Run exits with code zero when all self-validation tests pass.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithSilentContext_ExitCodeIsZero()
    {
        // Arrange: create silent context for validation run
        using var context = Context.Create(["--silent"]);

        // Act: run validation with silent context
        Validation.Run(context);

        // Assert: verify exit code is zero indicating successful validation
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that Run writes a valid TRX file when the results path ends with .trx.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithTrxResultsFile_WritesTrxFile()
    {
        // Arrange: setup TRX file path for test results output
        var trxFile = Path.Combine(Path.GetTempPath(), $"validation_test_{Guid.NewGuid()}.trx");
        try
        {
            using var context = Context.Create(["--silent", "--results", trxFile]);

            // Act: run validation with TRX results output
            Validation.Run(context);

            // Assert: verify TRX file is created with expected content
            Assert.IsTrue(File.Exists(trxFile));
            var content = File.ReadAllText(trxFile);
            Assert.Contains("<TestRun", content);
        }
        finally
        {
            if (File.Exists(trxFile))
            {
                File.Delete(trxFile);
            }
        }
    }

    /// <summary>
    ///     Test that Run writes a valid JUnit XML file when the results path ends with .xml.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithXmlResultsFile_WritesXmlFile()
    {
        // Arrange: setup XML file path for JUnit results output
        var xmlFile = Path.Combine(Path.GetTempPath(), $"validation_test_{Guid.NewGuid()}.xml");
        try
        {
            using var context = Context.Create(["--silent", "--results", xmlFile]);

            // Act: run validation with XML results output  
            Validation.Run(context);

            // Assert: verify XML file is created with JUnit format content
            Assert.IsTrue(File.Exists(xmlFile));
            var content = File.ReadAllText(xmlFile);
            Assert.Contains("<testsuites", content);
        }
        finally
        {
            if (File.Exists(xmlFile))
            {
                File.Delete(xmlFile);
            }
        }
    }

    /// <summary>
    ///     Test that Run does not write a results file when the extension is unsupported.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithUnsupportedResultsFormat_DoesNotWriteFile()
    {
        // Arrange: setup unsupported file extension for results output
        var jsonFile = Path.Combine(Path.GetTempPath(), $"validation_test_{Guid.NewGuid()}.json");
        try
        {
            using var context = Context.Create(["--silent", "--results", jsonFile]);

            // Act: run validation with unsupported results file extension
            Validation.Run(context);

            // Assert: verify no file is created for unsupported format
            Assert.IsFalse(File.Exists(jsonFile));
        }
        finally
        {
            if (File.Exists(jsonFile))
            {
                File.Delete(jsonFile);
            }
        }
    }

    /// <summary>
    ///     Test that Run writes output containing the install-package self-test result.
    /// </summary>
    [TestMethod]
    public void Validation_Run_WithSilentContext_PrintsInstallPackageTestResult()
    {
        // Arrange: setup unique log file path to capture silent context output
        var logFile = Path.Combine(Path.GetTempPath(), $"validation_test_{Guid.NewGuid()}.log");
        try
        {
            using (var context = Context.Create(["--silent", "--log", logFile]))
            {
                // Act: run validation with silent context and log file
                Validation.Run(context);
            }

            // Assert: verify install-package test result is written to log file
            var logContent = File.ReadAllText(logFile);
            Assert.Contains("NuGetInstaller_InstallPackage", logContent,
                "Validation output should include the install-package self-test result");
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}

