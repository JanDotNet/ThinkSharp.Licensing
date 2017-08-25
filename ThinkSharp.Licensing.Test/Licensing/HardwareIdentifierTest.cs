using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Common.Test;

namespace ThinkSharp.Licensing
{
    [TestClass]
    public class HardwareIdentifierTest
    {
        [TestMethod]
        public void Test_hardware_identifier_structure()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
            var split = hardwareID.Split('-');
            Assert.AreEqual(5, split.Length);
            Assert.AreEqual(8, split[0].Length);
            Assert.AreEqual(8, split[1].Length);
            Assert.AreEqual(8, split[2].Length);
            Assert.AreEqual(8, split[3].Length);
            Assert.AreEqual(4, split[4].Length);
        }

        [TestMethod]
        public void Test_verify()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
            Assert.IsTrue(HardwareIdentifier.Verify(hardwareID));
        }

        [TestMethod]
        public void Test_verify_null() => TestHelper.AssertException<ArgumentNullException>(() => HardwareIdentifier.Verify(null));

        [TestMethod]
        public void Test_verify_empty_string()
        {
            Assert.IsFalse(HardwareIdentifier.Verify(""));
        }

        [TestMethod]
        public void Test_verify_crap_string() => Assert.IsFalse(HardwareIdentifier.Verify("dsfdsaf<df"));

        [TestMethod]
        public void TestVerify_ZeroMatchingParts() => Assert.IsFalse(HardwareIdentifier.Verify("12345678-09876543-09876543-12345678-0987"));

        [TestMethod]
        public void TestVerify_OneMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
            var splitted = hardwareID.Split('-');
            var otherID = string.Format("1234567890-0987654321-3010203045-" + splitted[3] + "-DummycheckSum");
            Assert.IsFalse(HardwareIdentifier.Verify(otherID));
        }

        [TestMethod]
        public void TestVerify_TwoMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();

            var splitted = hardwareID.Split('-');
            var otherID = string.Format("12345678-09876543-" + splitted[2] + "-" + splitted[3] + "-DummycheckSum");
            Assert.IsTrue(HardwareIdentifier.Verify(otherID));
        }

        [TestMethod]
        public void TestVerify_ThreeMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();

            var splitted = hardwareID.Split('-');
            var otherID = string.Format("12345678-" + splitted[1] + "-" + splitted[2] + "-" + splitted[3] + "-DummycheckSum");
            Assert.IsTrue(HardwareIdentifier.Verify(otherID));
        }
    }
}
