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
        public void TestEmpty()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.Empty()
                .OutputFile(outputFile)
                .Run(out _);

            Assert.AreEqual(new FileInfo(outputFile).Length, 315);
            Assert.AreEqual(ExitCode.Success, result);
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
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void TestIsEncrypted()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .IsEncrypted()
                .RunIsEncrypted(out _);

            Assert.AreEqual(ExitCodeIsEncrypted.Encrypted, result);
        }

        [TestMethod]
        public void TestRequiresPassword()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .RequiresPassword()
                .RunRequiresPassword(out var output);

            Assert.AreEqual(ExitCodeRequiresPassword.OtherPasswordRequired, result);
        }

        [TestMethod]
        public void TestCheck()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .Check()
                .Run(out var output);

            Assert.AreEqual(output.Length, 1032);
            Assert.AreEqual(ExitCode.WarningsWereFoundFileProcessed, result);
        }

        [TestMethod]
        public void TestShowEncryption1()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
                .ShowEncryption()
                .Run(out var output);

            Assert.AreEqual(output, "File is not encrypted");
            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestShowEncryption2()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .Password("owner")
                .ShowEncryption()
                .Run(out var output);

            Assert.IsTrue(output.Contains("Supplied password is owner password"));
            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestShowEncryptionKey()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .Password("owner")
                .ShowEncryption()
                .ShowEncryptionKey()
                .Run(out var output);

            Assert.IsTrue(output.Contains("Encryption key = bc4d3ad2b43dbe372b02a4291cc7ee11413d33e87eb348d3364d5420312f1645"));
            Assert.AreEqual(ExitCode.Success, result);
        }

        // TODO: Add test from CheckLinearization

        [TestMethod]
        public void TestQdf()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "20_pages.pdf"))
                .OutputFile(outputFile)
                .Qdf()
                .Run(out _);

            Assert.AreEqual(new FileInfo(outputFile).Length, 144930);
            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestPages()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "20_pages.pdf"))
                .OutputFile(outputFile)
                .Pages(".", "1-5,9,15")
                .Run(out var output);

            job = new Job();
            job.InputFile(outputFile).ShowNPages().Run(out output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.AreEqual(output, "7");
        }

        [TestMethod]
        public void TestCollate()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "20_pages.pdf"))
                .OutputFile(outputFile)
                .Pages(".", "1-20")
                .Pages(Path.Combine(Path.Combine("TestFiles", "acrobat_8_help.pdf")), "1-20")
                .Collate(2)
                .Run(out var output);

            job = new Job();
            job.InputFile(outputFile).ShowNPages().Run(out output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.AreEqual(output, "40");
        }

        [TestMethod]
        public void TestShowNPages()
        {
            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "20_pages.pdf"))
                .ShowNPages().Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.AreEqual(output, "20");
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
                .Run(out _);

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
                .Run(out _);

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
        public void TestPasswordUser()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .OutputFile(outputFile)
                .Password("user")
                .Run(out var output);

            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestPasswordOwner()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .OutputFile(outputFile)
                .Password("owner")
                .Run(out _);

            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestDecrypt()
        {
            var outputFile = Path.Combine(_testFolder, "output.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
                .OutputFile(outputFile)
                .Password("owner")
                .Decrypt()
                .Run(out _);

            Assert.AreEqual(ExitCode.Success, result);
        }

        [TestMethod]
        public void TestSplitPages()
        {
            var outputFile = Path.Combine(_testFolder, "%d.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "acrobat_8_help.pdf"))
                .OutputFile(outputFile)
                .SplitPages(100)
                .Run(out _);

            Assert.AreEqual(ExitCode.Success, result);
            Assert.IsTrue(File.Exists(Path.Combine(_testFolder, "001-100.pdf")));
            Assert.IsTrue(File.Exists(Path.Combine(_testFolder, "101-200.pdf")));
            Assert.IsTrue(File.Exists(Path.Combine(_testFolder, "201-300.pdf")));
            Assert.IsTrue(File.Exists(Path.Combine(_testFolder, "301-400.pdf")));
            Assert.IsTrue(File.Exists(Path.Combine(_testFolder, "401-404.pdf")));
        }

        [TestMethod]
        public void TestOverlay()
        {
            var outputFile = Path.Combine(_testFolder, "%d.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "acrobat_8_help.pdf"))
                .OutputFile(outputFile)
                .SplitPages(100)
                .Run(out _);
        }

        [TestMethod]
        public void TestUnderlay()
        {
            var outputFile = Path.Combine(_testFolder, "%d.pdf");

            var job = new Job();
            var result = job.InputFile(Path.Combine("TestFiles", "acrobat_8_help.pdf"))
                .OutputFile(outputFile)
                .SplitPages(100)
                .Run(out _);
        }

    }
}