// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp
{
    public class CheckSumAppender
    {
        private readonly string mySeparator;
        private readonly CheckSum myChecksum;

        public CheckSumAppender(string separator, CheckSum checksum)
        {
            mySeparator = separator ?? throw new ArgumentNullException(nameof(separator));
            myChecksum = checksum ?? throw new ArgumentNullException(nameof(checksum));
        }

        public string Append(string inputToAppendCheckSum)
        {
            var checkSum = GetCheckSum(inputToAppendCheckSum);
            return inputToAppendCheckSum + mySeparator + checkSum;
        }

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
