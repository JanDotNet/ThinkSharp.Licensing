// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class SerialNumberTest
    {
        private static readonly Regex ValidNumbersRegex = new Regex("^([A-Z]|[0-9])*$");


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreate_Null() => SerialNumber.Create(null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreate__DE() => SerialNumber.Create(" DE");

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreate_DE2() => SerialNumber.Create("DE2");

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreate_DEFE() => SerialNumber.Create("DEFE");       

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateEmpty_Null() => SerialNumber.CreateEmpty(null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateEmpty__DE() => SerialNumber.CreateEmpty(" DE");

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateEmpty_DE2() => SerialNumber.CreateEmpty("DE2");

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateEmpty_DEFE() => SerialNumber.CreateEmpty("DEFE");

        [TestMethod]
        public void TestCreateEmpty_ABC()
        {           
            var serial = SerialNumber.CreateEmpty("ABC");
            Assert.AreEqual("SNABC-0000-0000-0000-6LUX", serial);
                
        }

        [TestMethod]
        public void TestBuild()
        {
            var serialNumber = SerialNumber.Create("ABC");
            Assert.AreEqual(25, serialNumber.Length);

            var splitted = serialNumber.Split('-');
            Assert.AreEqual(5, splitted.Length);
            Assert.AreEqual("SNABC", splitted[0]);
            Assert.AreEqual(4, splitted[1].Length);
            Assert.AreEqual(4, splitted[2].Length);
            Assert.AreEqual(4, splitted[3].Length);
            Assert.AreEqual(4, splitted[4].Length);
            for (int i = 1; i < splitted.Length; i++)
                Assert.IsTrue(ValidNumbersRegex.IsMatch(splitted[i]), $"Splitted part '{splitted[i]}' is not valid.");
        }

        [TestMethod]
        public void TestBuild_Fragment_6239()
        {
            Assert.IsTrue(ValidNumbersRegex.IsMatch("6234"));
        }

        [TestMethod]
        public void Test_VerifyCheckSum()
        {
            var serialNumber = SerialNumber.Create("ABC");
            Assert.IsTrue(SerialNumber.IsCheckSumValid(serialNumber));
            var modified = "D" + new string(serialNumber.Skip(1).ToArray());
            Assert.IsFalse(SerialNumber.IsCheckSumValid(modified));
            Assert.IsFalse(SerialNumber.IsCheckSumValid(modified + "A"));
            TestHelper.AssertException<ArgumentNullException>(() => SerialNumber.IsCheckSumValid(null));
            Assert.IsFalse(SerialNumber.IsCheckSumValid(""));
            Assert.IsFalse(SerialNumber.IsCheckSumValid("ABCDER"));
        }
    }
}
