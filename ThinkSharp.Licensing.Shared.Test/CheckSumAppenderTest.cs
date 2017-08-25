using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Common.Test
{
    [TestClass]
    public class CheckSumAppenderTest
    {
        [TestMethod]
        public void Test_separator_is_null() => TestHelper.AssertException<ArgumentNullException>(() => new CheckSumAppender(null, new CheckSum(10)));

        [TestMethod]
        public void Test_checksum_is_null() => TestHelper.AssertException<ArgumentNullException>(() => new CheckSumAppender("", null));

        [TestMethod]
        public void Test_with_separator() => AssertCheckSum("-", "ABCDE");

        [TestMethod]
        public void Test_without_separator() => AssertCheckSum("", "ABCDE");

        [TestMethod]
        public void Test_all_input_is_equal_to_separator_and_checksum() => AssertCheckSum("A", "AAAA");

        [TestMethod]
        public void Test_with_separator_in_input() => AssertCheckSum("-", "A-C-E");

        [TestMethod]
        public void Test_verify_input_null()
        {
            var checkSum = new CheckSum(new[] { 'A' }, 3);
            var appender = new CheckSumAppender("-", checkSum);
            TestHelper.AssertException<ArgumentNullException>(() => appender.Verify(null));
        }

        [TestMethod]
        public void Test_verify_input_length_less_than_checksum()
        {
            var checkSum = new CheckSum(new[] { 'A' }, 3);
            var appender = new CheckSumAppender("-", checkSum);
            Assert.IsFalse(appender.Verify(""));
            Assert.IsFalse(appender.Verify("A"));
            Assert.IsFalse(appender.Verify("AA"));
            Assert.IsFalse(appender.Verify("AAA"));
        }

        [TestMethod]
        public void Test_verify_is_false()
        {
            var checkSum = new CheckSum(10);
            var appender = new CheckSumAppender("-", checkSum);
            var input = appender.Append("ABCDEFGHIJKLMNOPQRSTUVW");
            Assert.IsTrue(appender.Verify(input));
            Assert.IsFalse(appender.Verify("A" + input));
            Assert.IsFalse(appender.Verify(input + "A"));
            
            Assert.IsFalse(appender.Verify("B" + new String(input.Skip(1).ToArray())));
        }

        private static void AssertCheckSum(string separator, string input)
        {
            var checkSum = new CheckSum(new[] { 'A' }, 3);
            var appender = new CheckSumAppender(separator, checkSum);
            var expected = input + separator + "AAA";
            Assert.AreEqual(expected, appender.Append(input));
            Assert.IsTrue(appender.Verify(expected));
        }
    }
}
