// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class EncoderTest
    {
        [TestMethod]
        public void Test_encoded_length()
        {
            var encoder = new Encoder();
            var input = "12345";
            var output = encoder.Encode(input);

            Assert.AreEqual(input.Length, output.Length);
        }

        [TestMethod]
        public void Test_multiple_encoding_produces_same_result()
        {
            var encoder = new Encoder();
            var input = "12345";
            var output1 = encoder.Encode(input);
            var output2 = encoder.Encode(input);

            Assert.AreEqual(output1, output2);
        }

        [TestMethod]
        public void Test_custom_length_100()
        {
            var encoder = new Encoder(100);
            var input = "12345";
            var output = encoder.Encode(input);

            Assert.AreEqual(100, output.Length);
        }

        [TestMethod]
        public void Test_custom_length_2()
        {
            var encoder = new Encoder(2);
            var input = "12345";
            var output = encoder.Encode(input);

            Assert.AreEqual(2, output.Length);
        }

        [TestMethod]
        public void Test_custom_character_set()
        {
            var encoder = new Encoder("41", 100);
            var input = "12345";
            var output = encoder.Encode(input);

            Assert.AreEqual(100, output.Length);
            foreach (var c in output)
                Assert.IsTrue("41".Contains(c.ToString()));
        }

        [TestMethod]
        public void Test_default_character_set()
        {
            var encoder = new Encoder(100);
            var input = "12345";
            var output = encoder.Encode(input);

            foreach (var c in output)
                Assert.IsTrue(Constants.ValidCharacters.Contains(c.ToString()));
        }

        [TestMethod]
        public void Test_negative_encoding_length()
        {
            var encode = new Encoder(-10);
            var result = encode.Encode(new byte[0]);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Test_custom_char_set()
        {
            var encode = new Encoder("abc");
            var result = encode.Encode("qbc");
            Assert.AreEqual(3, result.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_encode_string_null()
        {
            var encode = new Encoder(-10);
            var result = encode.Encode((string)null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_encode_bytes_null()
        {
            var encode = new Encoder(-10);
            var result = encode.Encode((byte[])null);
            Assert.AreEqual(string.Empty, result);
        }
    }
}
