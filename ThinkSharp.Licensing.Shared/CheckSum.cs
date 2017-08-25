using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp
{
    public class CheckSum
    {
        private readonly char[] myValidCharacters;
        private readonly int myLenght;

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

        public CheckSum(int length)
            : this(Constants.ValidCharacters.ToCharArray(), length)
        { }

        public int Length => myLenght;

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
