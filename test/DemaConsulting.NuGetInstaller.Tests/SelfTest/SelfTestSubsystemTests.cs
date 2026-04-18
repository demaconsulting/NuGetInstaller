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

namespace DemaConsulting.NuGetInstaller.Tests;

/// <summary>
///     Subsystem tests for the SelfTest subsystem covering Validation workflows.
/// </summary>
[TestClass]
public class SelfTestSubsystemTests
{
    /// <summary>
    ///     Test that self-test subsystem can run validation workflow without result files.
    /// </summary>
    [TestMethod]
    public void SelfTestSubsystem_ValidationWorkflow_NoResultFiles_CompletesSuccessfully()
    {
        // Arrange: command line arguments for validation in silent mode
        var args = new[] { "--validate", "--silent" };

        // Act: create context and run validation
        using var context = Context.Create(args);
        Validation.Run(context);

        // Assert: validation completes successfully with correct flags set
        Assert.IsTrue(context.Validate, "Context should have validate flag set");
        Assert.AreEqual(0, context.ExitCode, "Validation should complete successfully");
    }

    /// <summary>
    ///     Test that self-test subsystem can run validation workflow with TRX result file.
    /// </summary>
    [TestMethod]
    public void SelfTestSubsystem_ValidationWorkflow_WithTrxFile_GeneratesResults()
    {
        // Arrange: temporary TRX file path and validation command with results output
        var tempDir = Path.GetTempPath();
        var trxFile = Path.Combine(tempDir, $"test_{Guid.NewGuid()}.trx");
        var args = new[] { "--validate", "--silent", "--results", trxFile };

        try
        {
            // Act: create context and run validation with TRX output
            using var context = Context.Create(args);
            Validation.Run(context);

            // Assert: validation completes and generates TRX file with standard format
            Assert.IsTrue(context.Validate, "Context should have validate flag set");
            Assert.AreEqual(0, context.ExitCode, "Validation should complete successfully");
            Assert.IsTrue(File.Exists(trxFile), "TRX file should be generated");
            var trxContent = File.ReadAllText(trxFile);
            Assert.Contains("<TestRun", trxContent, "TRX file should contain standard TestRun element");
        }
        finally
        {
            // Cleanup
            if (File.Exists(trxFile))
            {
                File.Delete(trxFile);
            }
        }
    }

    /// <summary>
    ///     Test that self-test subsystem can run validation workflow with JUnit result file.
    /// </summary>
    [TestMethod]
    public void SelfTestSubsystem_ValidationWorkflow_WithJUnitFile_GeneratesResults()
    {
        // Arrange: temporary JUnit XML file path and validation command with results output
        var tempDir = Path.GetTempPath();
        var junitFile = Path.Combine(tempDir, $"test_{Guid.NewGuid()}.xml");
        var args = new[] { "--validate", "--silent", "--results", junitFile };

        try
        {
            // Act: create context and run validation with JUnit XML output
            using var context = Context.Create(args);
            Validation.Run(context);

            // Assert: validation completes and generates JUnit XML file with standard format
            Assert.IsTrue(context.Validate, "Context should have validate flag set");
            Assert.AreEqual(0, context.ExitCode, "Validation should complete successfully");
            Assert.IsTrue(File.Exists(junitFile), "JUnit file should be generated");
            var junitContent = File.ReadAllText(junitFile);
            Assert.Contains("<testsuites", junitContent, "JUnit file should contain standard testsuites element");
        }
        finally
        {
            // Cleanup
            if (File.Exists(junitFile))
            {
                File.Delete(junitFile);
            }
        }
    }

    /// <summary>
    ///     Test that self-test subsystem can run validation workflow with both result file formats.
    /// </summary>
    [TestMethod]
    public void SelfTestSubsystem_ValidationWorkflow_WithBothResultFiles_GeneratesBothResults()
    {
        // Arrange: setup validation arguments for both TRX and JUnit result file outputs
        var tempDir = Path.GetTempPath();
        var trxFile = Path.Combine(tempDir, $"test_{Guid.NewGuid()}.trx");
        var junitFile = Path.Combine(tempDir, $"test_{Guid.NewGuid()}.xml");
        var trxArgs = new[] { "--validate", "--silent", "--results", trxFile };
        var junitArgs = new[] { "--validate", "--silent", "--results", junitFile };

        try
        {
            // Act: run validation with TRX output
            using var trxContext = Context.Create(trxArgs);
            Validation.Run(trxContext);

            // Assert: verify validation completed and TRX result file was generated with standard format
            Assert.IsTrue(trxContext.Validate, "Context should have validate flag set for TRX run");
            Assert.AreEqual(0, trxContext.ExitCode, "Validation should complete successfully for TRX run");
            Assert.IsTrue(File.Exists(trxFile), "TRX file should be generated");
            var trxContent = File.ReadAllText(trxFile);
            Assert.Contains("<TestRun", trxContent, "TRX file should contain standard TestRun element");

            // Act: run validation with JUnit XML output
            using var junitContext = Context.Create(junitArgs);
            Validation.Run(junitContext);

            // Assert: verify validation completed and JUnit XML result file was generated with standard format
            Assert.IsTrue(junitContext.Validate, "Context should have validate flag set for JUnit run");
            Assert.AreEqual(0, junitContext.ExitCode, "Validation should complete successfully for JUnit run");
            Assert.IsTrue(File.Exists(junitFile), "JUnit file should be generated");
            var junitContent = File.ReadAllText(junitFile);
            Assert.Contains("<testsuites", junitContent, "JUnit file should contain standard testsuites element");
        }
        finally
        {
            // Cleanup
            if (File.Exists(trxFile))
            {
                File.Delete(trxFile);
            }

            if (File.Exists(junitFile))
            {
                File.Delete(junitFile);
            }
        }
    }
}

