// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp
{
    /// <summary>
    /// Class for creating check sum for an array of bytes.
    /// </summary>
    public class CheckSum
    {
        private readonly char[] myValidCharacters;
        private readonly int myLenght;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="supportedCharacters">
        /// List of supported characters for the check sum.
        /// </param>
        /// <param name="length">
        /// The lenght of the check sum.
        /// </param>
        public CheckSum(char[] supportedCharacters, int length)
        {
            if (supportedCharacters == null)
                throw new ArgumentNullException(nameof(supportedCharacters));
            if (supportedCharacters.Length == 0)
                throw new ArgumentException(nameof(supportedCharacters));
            if (length <= 0)
                throw new ArgumentException(nameof(length));

            myValidCharacters = supportedCharacters.Distinct().ToArray();
            myLenght = length;
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// Check sum consists of the characters [A-Z] and [0-9].
        /// </summary>
        /// <param name="length">
        /// The length of the check sum.
        /// </param>
        public CheckSum(int length)
            : this(Constants.ValidCharacters.ToCharArray(), length)
        { }

        /// <summary>
        /// Gets the length of the check sum.
        /// </summary>
        public int Length => myLenght;

        /// <summary>
        /// Creates the check sum for the specified byte array.
        /// </summary>
        /// <param name="bytes">
        /// The byte array to create the check sum for.
        /// </param>
        /// <returns>
        /// A string that represents the check sum of the specified byte array.
        /// </returns>
        public string Create(byte[] bytes)
        {
            bytes = AdjustSize(bytes, myLenght);

            var checkSum = bytes.Select(ToCheckSumChar).ToArray();

            return new string(checkSum);
        }

        private char ToCheckSumChar(byte b)
        {
            return myValidCharacters[b % myValidCharacters.Length];
        }

        private static byte[] AdjustSize(byte[] bytes, int length)
        {
            if (bytes.Length > length)
                return ReduceSize(bytes, length);
            if (bytes.Length < length)
                return InflateSize(bytes, length);
            return bytes;
        }

        private static byte[] InflateSize(byte[] bytes, int length)
        {
            var result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] ^= bytes[i % bytes.Length];
            return result;
        }

        private static byte[] ReduceSize(byte[] bytes, int length)
        {
            var result = new byte[length];
            for (int i = 0; i < bytes.Length; i++)
                result[i % length] ^= bytes[i];
            return result;
        }
    }
}
