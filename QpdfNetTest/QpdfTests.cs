using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPdfNet;
using QPdfNet.Enums;

namespace QpdfNetTest
{
    [TestClass]
    public class QpdfTests
    {
#pragma warning disable CS8618
        private string _testFolder;
#pragma warning restore CS8618

        [TestInitialize]
        public void Initialize()
        {
            var directory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            directory.Create();
            _testFolder = directory.FullName;
        }

        [TestCleanup]
        public void Cleanup()
        {
            var directory = new DirectoryInfo(_testFolder);
            if (directory.Exists)
                directory.Delete(true);
        }

        [TestMethod]
        public void TestInputOutput()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job
                .InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(string.IsNullOrEmpty(output));
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestInputOutputWithParameters()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job
                .InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .WarningExit0()
                .Verbose()
                .NoWarn()
                .DeterministicId()
                .AllowWeakCrypto()
                .SuppressRecovery()
                .IgnoreXrefStreams()
                .Linearize()
                .NoOriginalObjectIds()
                .CompressStreams(true)
                .DecodeLevel()
                .StreamData()
                .RecompressFlate()
                .CompressionLevel(9)
                .NormalizeContent(true)
                .ObjectStreams(ObjectStreams.Generate)
                .PreserveUnreferenced()
                .PreserveUnreferencedResources()
                .NewlineBeforeEndstream()
                .CoalesceContents()
                .ExternalizeInlineImages()
                .IiMinBytes()
                .Collate()
                .FlattenRotation()
                .FlattenAnnotations(FlattenAnnotations.All)
                .Rotate(Rotation.Rotate0)
                .GenerateAppearances()
                .OptimizeImages()
                .OiMinWidth()
                .OiMinHeight()
                .OiMinArea()
                .KeepInlineImages()
                .RemovePageLabels()
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(string.IsNullOrEmpty(output));
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestPages()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .Pages(".", "1")
                .Pages("d:\\acrobat_8_help.pdf", "1-4")
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestEncryption40Bit()
        {
            var outputFile = Path.Combine(_testFolder, "output_encryption_40_bit.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .Encrypt("user", "owner", new Encryption40Bit(false))
                .Linearize()
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestEncryption128Bit()
        {
            var outputFile = Path.Combine(_testFolder, "output_encryption_256_bit.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .Encrypt("user", "owner", new Encryption128Bit(false, false))
                .Linearize()
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestEncryption256Bit()
        {
            var outputFile = Path.Combine(_testFolder, "output_encryption_256_bit.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .OutputFile(outputFile)
                .Encrypt("user", "owner", new Encryption256Bit(true, true, true, true, true, true, Modify.None, Print.None))
                .Linearize()
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void SplitPages()
        {
            var outputFile = Path.Combine(_testFolder, "%d.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "acrobat_8_help.pdf"))
                .OutputFile(outputFile)
                .SplitPages(100)
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
        }
    }
}