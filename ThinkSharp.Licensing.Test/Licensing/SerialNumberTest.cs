using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Common.Test;

namespace ThinkSharp.Licensing
{
    [TestClass]
    public class SerialNumberTest
    {
        private static readonly Regex ValidNumbersRegex = new Regex("([A-Z]|0-9)");

        [TestMethod]
        public void TestCreate()
        {
            try
            {
                var sn = SerialNumber.Create(null);
                Assert.Fail("ArgumentNullException expected.");
            } catch (ArgumentNullException)
            { }

            try
            {
                var sn = SerialNumber.Create(" DE");
                Assert.Fail("ArgumentException expected.");
            }
            catch (ArgumentException)
            { }

            try
            {
                var sn = SerialNumber.Create("DE2");
                Assert.Fail("ArgumentException expected.");
            }
            catch (ArgumentException)
            { }

            try
            {
                var sn = SerialNumber.Create("DEFE");
                Assert.Fail("ArgumentException expected.");
            }
            catch (ArgumentException)
            { }
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
                Assert.IsTrue(ValidNumbersRegex.IsMatch(splitted[i]));
        }

        [TestMethod]
        public void Test_VerifyCheckSum()
        {
            var serialNumber = SerialNumber.Create("ABC");
            Assert.IsTrue(SerialNumber.VerifyCheckSum(serialNumber));
            var modified = "D" + new string(serialNumber.Skip(1).ToArray());
            Assert.IsFalse(SerialNumber.VerifyCheckSum(modified));
            Assert.IsFalse(SerialNumber.VerifyCheckSum(modified + "A"));
            TestHelper.AssertException<ArgumentNullException>(() => SerialNumber.VerifyCheckSum(null));
            Assert.IsFalse(SerialNumber.VerifyCheckSum(""));
            Assert.IsFalse(SerialNumber.VerifyCheckSum("ABCDER"));
        }
    }
}
