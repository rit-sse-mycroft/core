using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mycroft.Tests
{
    [TestClass]
    public class TestTlsServer
    {
        /// <summary>
        /// Tests that the OpenSSL format is stripped correctly - colons are removed
        /// and case is preserved
        /// </summary>
        [TestMethod]
        public void TestFormatCertificateThumbprint_OpenSSLFormat()
        {
            var original = "17:31:F2:7B:31:35:49:DE:04:CA:19:87:F9:EB:C2:C7:87:42:0B:75";
            string thumbprint = TlsServer.FormatCertificateThumbprint(original);
            Assert.AreEqual("1731F27B313549DE04CA1987F9EBC2C787420B75", thumbprint);
        }

        /// <summary>
        /// Tests that the Windows certificate viewer format is stripped correctly.
        /// Removes weird hidden characters and spaces and makes everything uppercase.
        /// </summary>
        [TestMethod]
        public void TestFormatCertificateThumbprint_WindowsFormat()
        {
            var original = "‎17 31 f2 7b 31 35 49 de 04 ca 19 87 f9 eb c2 c7 87 42 0b 75";
            string thumbprint = TlsServer.FormatCertificateThumbprint(original);
            Assert.AreEqual("1731F27B313549DE04CA1987F9EBC2C787420B75", thumbprint);
        }


        /// <summary>
        /// Tests that a correctly formatted string is preserved
        /// </summary>
        [TestMethod]
        public void TestFormatCertificateThumbprint_AlreadyFormatted()
        {
            var original = "‎1731F27B313549DE04CA1987F9EBC2C787420B75";
            string thumbprint = TlsServer.FormatCertificateThumbprint(original);
            Assert.AreEqual("1731F27B313549DE04CA1987F9EBC2C787420B75", thumbprint);
        }

    }
}
