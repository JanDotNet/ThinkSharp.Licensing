// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class CheckSumTest
    {
        [TestMethod]
        public void Test_valid_characters_is_null() => TestHelper.AssertException<ArgumentNullException>(() => new CheckSum(null, 10));

        [TestMethod]
        public void Test_valid_characters_is_empty() => TestHelper.AssertException<ArgumentException>(() => new CheckSum(new char[0], 10));


        [TestMethod]
        public void Test_length_is_negativ() => TestHelper.AssertException<ArgumentException>(() => new CheckSum(new[] { 'A' }, -10));

        [TestMethod]
        public void Test_length_is_zero() => TestHelper.AssertException<ArgumentException>(() => new CheckSum(new[] { 'A' }, 0));

        [TestMethod]
        public void Test_SingleChar()
        {
            var checksum = new CheckSum(new[] { 'A' }, 10);
            Assert.AreEqual(new string('A', 10), checksum.Create(new byte[] {4}));
        }

        [TestMethod]
        public void Test_two_chars_length_one()
        {
            var checksum = new CheckSum(new[] { 'A', 'B' }, 1);
            Assert.AreEqual("B", checksum.Create(new byte[] { 4, 10, 34, 3, 24 }));
        }

        [TestMethod]
        public void Test_two_chars_length_two()
        {
            var checksum = new CheckSum(new[] { 'A', 'B' }, 2);
            Assert.AreEqual("AB", checksum.Create(new byte[] { 4, 10, 34, 3, 24 }));
        }

        [TestMethod]
        public void Test_single_char_length_one()
        {
            var checksum = new CheckSum(new[] { 'A' }, 1);
            Assert.AreEqual("A", checksum.Create(new byte[] { 4, 10, 34, 3, 24 }));
        }

        [TestMethod]
        public void Test_single_char_length_two()
        {
            var checksum = new CheckSum(new[] { 'A' }, 2);
            Assert.AreEqual("AA", checksum.Create(new byte[] { 4, 10, 34, 3, 24 }));
        }

        [TestMethod]
        public void Test_byte_array_length_equal_to_check_sum_length()
        {
            var checksum = new CheckSum(new[] { 'A', 'B', 'C' }, 3);
            Assert.AreEqual("ABC", checksum.Create(new byte[] { 0, 1, 2 }));
        }

        [TestMethod]
        public void Test_byte_array_length_less_than_check_sum_length()
        {
            var checksum = new CheckSum(new[] { 'A', 'B', 'C' }, 6);
            Assert.AreEqual("ABCABC", checksum.Create(new byte[] { 0, 1, 2 }));
        }

        [TestMethod]
        public void Test_byte_array_length_greater_than_check_sum_length()
        {
            var checksum = new CheckSum(new[] { 'A', 'B', 'C' }, 2);
            Assert.AreEqual("AB", checksum.Create(new byte[] { 1, 1, 1 }));
        }
    }
}
