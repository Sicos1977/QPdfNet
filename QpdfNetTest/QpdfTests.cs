using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPdfNet;
using QPdfNet.Enums;

namespace QpdfNetTest
{
    [TestClass]
    public class QpdfTests
    {
        [TestMethod]
        public void TestEncryption()
        {
            var job = new Job();
            var result = job.InputFile(@"d:\test.pdf")
                .OutputFile(@"d:\output.pdf")
                .Encrypt("user", "owner", new Encryption256bit())
                .WithLinearize()
                .Run();

            Assert.AreEqual(ExitCodes.Success, result);
        }
    }
}