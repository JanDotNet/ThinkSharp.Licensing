// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Licensing.Helper;

namespace ThinkSharp.Licensing.Test.Helper
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void TestWrap_null() => TestHelper.AssertException<ArgumentNullException>(() => StringHelper.Wrap(null, 2));

        [TestMethod]
        public void TestWrap_negative_columns() => TestHelper.AssertException<ArgumentException>(() => StringHelper.Wrap("aby", -1));

        [TestMethod]
        public void TestWrap_0_columns() => TestHelper.AssertException<ArgumentException>(() => StringHelper.Wrap("aby", 0));

        [TestMethod]
        public void TestWrap_line_shorter_than_columns()
        {
            var result = StringHelper.Wrap("abc", 4);
            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void TestWrap_line_equal_to_columns()
        {
            var result = StringHelper.Wrap("abc", 3);
            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void TestWrap_line_length_3_column_length_2()
        {
            var result = StringHelper.Wrap("abc", 2);
            Assert.AreEqual(string.Join(Environment.NewLine, "ab", "c"), result);
        }

        [TestMethod]
        public void TestWrap_line_length_3_column_length_1()
        {
            var result = StringHelper.Wrap("abc", 1);
            Assert.AreEqual(string.Join(Environment.NewLine, "a", "b", "c"), result);
        }

        [TestMethod]
        public void TestWrap_line_length_55_column_length_10()
        {
            var result = StringHelper.Wrap(new string('*', 55), 10);
            Assert.AreEqual(string.Join(Environment.NewLine, 
                new string('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 5)), result);
        }

        [TestMethod]
        public void TestWrap_line_length_49_column_length_10()
        {
            var result = StringHelper.Wrap(new string('*', 49), 10);
            Assert.AreEqual(string.Join(Environment.NewLine,
                new string('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 9)), result);
        }

        [TestMethod]
        public void TestWrap_line_length_50_column_length_10()
        {
            var result = StringHelper.Wrap(new string('*', 50), 10);
            Assert.AreEqual(string.Join(Environment.NewLine,
                new string('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10)), result);
        }

        [TestMethod]
        public void TestWrap_line_length_51_column_length_10()
        {
            var result = StringHelper.Wrap(new string('*', 51), 10);
            Assert.AreEqual(string.Join(Environment.NewLine,
                new string('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 10),
                new String('*', 1)), result);
        }

        [TestMethod]
        public void TestUnWrap()
        {
            var result = string.Join(Environment.NewLine, "a", "b", "c").Unwrap();
            Assert.AreEqual("abc", result);
        }
    }
}
