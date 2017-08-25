using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThinkSharp.Licensing
{
    // Serial Number Format:
    // SNAPP-2543-3564-3456-3457
    // SN{AppCode}-{Fragment}-{Fragment}-{Fragment}-{CheckSum}
    public class SerialNumber
    {
        private static readonly Regex ApplicationCodeValidationRegex = new Regex("^[A-Z]{3}$", RegexOptions.Compiled);
        private static readonly CheckSumAppender CheckSumAppender = new CheckSumAppender(Seperator, new CheckSum(FragmentSize));
        private static readonly Encoder FragmentEncoder = new Encoder(FragmentSize);
        private const int FragmentSize = 4;
        private const int FragmentCount = 3;
        private const string Seperator = "-";

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

        public static bool IsCheckSumValid(string serialNumber)
        {
            return CheckSumAppender.Verify(serialNumber);
        }

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

        internal static string NoSerialNumber = "NO_SERIAL_NO";
    }
}
