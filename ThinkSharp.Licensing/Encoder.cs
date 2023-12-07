// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace ThinkSharp.Licensing
{
    public class Encoder
    {
        private readonly string myValidCharacters;
        private readonly int myEncodingLength;

        public Encoder() : this(Constants.ValidCharacters, -1)
        {
        }

        public Encoder(int encodingLength) : this(Constants.ValidCharacters, encodingLength)
        {
        }

        public Encoder(string validCharacters) : this(validCharacters, -1)
        {
        }

        public Encoder(string validCharacters, int encodingLength)
        {
            myValidCharacters = validCharacters;
            myEncodingLength = encodingLength;
        }

        public string Encode(string input)
        {
            return Encode(Encoding.UTF8.GetBytes(input), myEncodingLength < 0 ? input.Length : myEncodingLength);
        }

        public string Encode(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return Encode(bytes, myEncodingLength < 0 ? bytes.Length : myEncodingLength);
        }

        private string Encode(byte[] bytes, int length)
        {
            var random = new Random(99);

            if (length < 0)
                throw new ArgumentException("length must be a positive number");

            var buffer = new byte[length];
            random.NextBytes(buffer);

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] ^= bytes[i%bytes.Length];

            var builder = new StringBuilder();
            foreach (var b in buffer)
                builder.Append(myValidCharacters[b % myValidCharacters.Length]);
            return builder.ToString();
        }
    }
}
