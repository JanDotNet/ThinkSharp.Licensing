// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class HardwareIdentifierDotNetFullTest
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
        public void Test_verify_null() => TestHelper.AssertException<ArgumentNullException>(() => HardwareIdentifier.IsValidForCurrentComputer(null));

        [TestMethod]
        public void Test_verify_empty_string()
        {
            Assert.IsFalse(HardwareIdentifier.IsValidForCurrentComputer(""));
        }

        [TestMethod]
        public void Test_verify_crap_string() => Assert.IsFalse(HardwareIdentifier.IsValidForCurrentComputer("dsfdsaf<df"));

        [TestMethod]
        public void Test_verify()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
            Assert.IsTrue(HardwareIdentifier.IsValidForCurrentComputer(hardwareID));
        }

        [TestMethod]
        public void TestVerify_OneMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
            var splitted = hardwareID.Split('-');
            var otherID = string.Format("1234567890-0987654321-3010203045-" + splitted[3] + "-DummycheckSum");
            Assert.IsFalse(HardwareIdentifier.IsValidForCurrentComputer(otherID));
        }

        [TestMethod]
        public void TestVerify_TwoMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();

            var splitted = hardwareID.Split('-');
            var otherID = string.Format("12345678-09876543-" + splitted[2] + "-" + splitted[3] + "-DummycheckSum");
            Assert.IsTrue(HardwareIdentifier.IsValidForCurrentComputer(otherID));
        }

        [TestMethod]
        public void TestVerify_ThreeMatchingParts()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();

            var splitted = hardwareID.Split('-');
            var otherID = string.Format("12345678-" + splitted[1] + "-" + splitted[2] + "-" + splitted[3] + "-DummycheckSum");
            Assert.IsTrue(HardwareIdentifier.IsValidForCurrentComputer(otherID));
        }

        [TestMethod]
        public void TestVerify_ZeroMatchingParts() => Assert.IsFalse(HardwareIdentifier.IsValidForCurrentComputer("12345678-09876543-09876543-12345678-0987"));
    }
}
