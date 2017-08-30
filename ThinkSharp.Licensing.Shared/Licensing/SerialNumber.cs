// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThinkSharp.Licensing
{
    /// <summary>
    /// Class that provides methods for creating and verifying serial numbers. Each serial number as the format
    /// SN{AppCode}-{Fragment}-{Fragment}-{Fragment}-{CheckSum} (e.g. SNAPP-2543-3564-3456-3457).
    /// {AppCode} consists of 3 upper case letters (A-Z).
    /// </summary>
    public static class SerialNumber
    {
        private static readonly Regex ApplicationCodeValidationRegex = new Regex("^[A-Z]{3}$", RegexOptions.Compiled);
        private static readonly CheckSumAppender CheckSumAppender = new CheckSumAppender(Seperator, new CheckSum(FragmentSize));
        private static readonly Encoder FragmentEncoder = new Encoder(FragmentSize);
        private const int FragmentSize = 4;
        private const int FragmentCount = 3;
        private const string Seperator = "-";

        /// <summary>
        /// Creates a serial number with the specified application code.
        /// </summary>
        /// <param name="threeLetterApplicationCode">
        /// The application code to use. The application code consists of 3 upper case letters (A-Z).
        /// </param>
        /// <returns>
        /// A serial number with the specified application code.
        /// </returns>
        public static string Create(string threeLetterApplicationCode)
        {
            threeLetterApplicationCode = EnsureApplicationCodeIsValid(threeLetterApplicationCode);

            var fragments = GetRandomFragments();
            var fragmentsString = "SN" + threeLetterApplicationCode + Seperator + string.Join(Seperator, fragments);
            return CheckSumAppender.Append(fragmentsString);
        }

        internal static string EnsureApplicationCodeIsValid(string threeLetterApplicationCode)
        {
            if (threeLetterApplicationCode == null)
                throw new ArgumentNullException(nameof(threeLetterApplicationCode));
            threeLetterApplicationCode = threeLetterApplicationCode.ToUpper();
            if (!ApplicationCodeValidationRegex.IsMatch(threeLetterApplicationCode))
                throw new ArgumentException("applicationCode has to consist of 3 capital letters.");
            return threeLetterApplicationCode;
        }

        /// <summary>
        /// Creates an empty serial number with the specified application code where all fragments are '0000'.
        /// </summary>
        /// <param name="threeLetterApplicationCode">
        /// The application code to use. The application code consists of 3 upper case letters (A-Z).
        /// </param>
        /// <returns>
        /// An empty serial number with the specified application code.
        /// </returns>
        public static string CreateEmpty(string threeLetterApplicationCode)
        {
            if (threeLetterApplicationCode == null)
                throw new ArgumentNullException("threeLetterApplicationCode");
            threeLetterApplicationCode = threeLetterApplicationCode.ToUpper();
            if (!ApplicationCodeValidationRegex.IsMatch(threeLetterApplicationCode))
                throw new ArgumentException("applicationCode has to consist of 3 capital letter.");

            var fragements = Enumerable.Range(0, FragmentCount).Select(c => new String('0', FragmentSize));
            var fragmentsString = "SN" + threeLetterApplicationCode + Seperator + string.Join(Seperator, fragements);
            return CheckSumAppender.Append(fragmentsString);
        }

        /// <summary>
        /// Validates the check sum of the specified serial number.
        /// </summary>
        /// <param name="serialNumber">
        /// The serial number to check.
        /// </param>
        /// <returns>
        /// true if the check sum is valid or the serial number is equal to <see cref="NoSerialNumber"/>; 
        /// otherwise false.
        /// </returns>
        public static bool IsCheckSumValid(string serialNumber)
        {
            return CheckSumAppender.Verify(serialNumber);
        }

        /// <summary>
        /// Checks if a serial number has the specified application code.
        /// </summary>
        /// <param name="serialNumber">
        /// The serial number to check.
        /// </param>
        /// <param name="applicationCode">
        /// The application code used for checking.
        /// </param>
        /// <returns>
        /// true if the serial number has the specified application code or if the serial number is equal to 
        /// <see cref="NoSerialNumber"/>; otherwise false.
        /// </returns>
        public static bool IsApplicationCodeValid(string serialNumber, string applicationCode)
        {
            if (serialNumber == NoSerialNumber)
                return true;
            applicationCode = EnsureApplicationCodeIsValid(applicationCode);
            return serialNumber.StartsWith("SN" + applicationCode);
        }

        private static IEnumerable<string> GetRandomFragments()
        {
            var random = new Random();
            for (int fragment = 0; fragment < FragmentCount; fragment++)
            {
                var bytes = new byte[FragmentSize];
                random.NextBytes(bytes);
                yield return FragmentEncoder.Encode(bytes);
            }
        }

        internal static readonly string NoSerialNumber = "NO_SERIAL_NO";
    }
}
