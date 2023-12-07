// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace ThinkSharp.Licensing
{
    /// <summary>
    /// The CheckSumAppender can be used to append the check sum to a string and verify strings which have a checksum appended.
    /// </summary>
    public class CheckSumAppender
    {
        private readonly string mySeparator;
        private readonly CheckSum myChecksum;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="separator">
        /// The separator that separates the check sum from the string.
        /// </param>
        /// <param name="checksum">
        /// <see cref="CheckSum"/> to use for creating the check sum.
        /// </param>
        public CheckSumAppender(string separator, CheckSum checksum)
        {
            mySeparator = separator ?? throw new ArgumentNullException(nameof(separator));
            myChecksum = checksum ?? throw new ArgumentNullException(nameof(checksum));
        }

        /// <summary>
        /// Appends the check sum to the specified string.
        /// </summary>
        /// <param name="inputToAppendCheckSum">
        /// The string to append the check sum to.
        /// </param>
        /// <returns>
        /// The specified string + the separator + the check sum.
        /// </returns>
        public string Append(string inputToAppendCheckSum)
        {
            var checkSum = GetCheckSum(inputToAppendCheckSum);
            return inputToAppendCheckSum + mySeparator + checkSum;
        }

        /// <summary>
        /// Verifies if the specified string (which includes the check sum) is valid.
        /// </summary>
        /// <param name="inputWithCheckSumToVerify">
        /// The string + separator + check sum.
        /// </param>
        /// <returns>
        /// True if the check sum is valid; otherwise false.
        /// </returns>
        public bool Verify(string inputWithCheckSumToVerify)
        {
            if (inputWithCheckSumToVerify == null)
                throw new ArgumentNullException(nameof(inputWithCheckSumToVerify));

            var inputLength = inputWithCheckSumToVerify.Length - myChecksum.Length - mySeparator.Length;
            if (inputLength <= 0)
                return false;

            var input = inputWithCheckSumToVerify.Substring(0, inputLength);
            var checkSum = GetCheckSum(input);
            return inputWithCheckSumToVerify == (input + mySeparator + checkSum);
        }

        private string GetCheckSum(string inputToAppendCheckSum)
        {
            var bytes = Encoding.UTF8.GetBytes(inputToAppendCheckSum);
            var checkSum = myChecksum.Create(bytes);
            return checkSum;
        }
    }
}
