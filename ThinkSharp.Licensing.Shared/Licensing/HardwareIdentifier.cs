using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.Licensing
{
    public interface IComputerCharacteristics
    {
        IEnumerable<string> GetCharacteristicsForCurrentComputer();
    }
    public static class HardwareIdentifier
    {        
        private static readonly CheckSumAppender CheckSumAppender = new CheckSumAppender(Separator, new CheckSum(4));
        private static readonly HashAlgorithm MD5 = new MD5CryptoServiceProvider();
        private static readonly Encoder Encoder = new Encoder(PartSize);

        private const int PartSize = 8;
        private const string Separator = "-";
        private static IComputerCharacteristics theComputerCharacteristics = null;

        static HardwareIdentifier()
        {
            var interfaceType = typeof(IComputerCharacteristics);
            var currentAssembly = typeof(HardwareIdentifier).Assembly;
            var type = currentAssembly.GetTypes().Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass).FirstOrDefault();

            if (type != null)
                theComputerCharacteristics = (IComputerCharacteristics)Activator.CreateInstance(type);
        }

        public static void SetComputerCharacteristics(IComputerCharacteristics creator)
        {
            theComputerCharacteristics = creator;
        }

        /// <summary>
        /// Gets the hardware identifier for the current computer.
        /// NOTE: <see cref="SetComputerCharacteristics(IComputerCharacteristics)"/> must be set befor using this method.
        /// </summary>
        /// <returns></returns>
        public static string ForCurrentComputer()
        {
            if (theComputerCharacteristics == null)
                throw new NotSupportedException("Creating hardware id for current computer is not supported for DotNetStandard. Please use 'HardwareIdentifier.SetComputerCharacteristics' to set a plattform specific implementation of 'IComputerCharacteristics'.");

            var encodedCharacteristics = new List<string>();
            foreach (var characteristic in theComputerCharacteristics.GetCharacteristicsForCurrentComputer())
            {

                var bytes = Encoding.UTF8.GetBytes(characteristic);
                bytes = MD5.ComputeHash(bytes);

                encodedCharacteristics.Add(Encoder.Encode(bytes));
            }

            var hardwareKey = string.Join(Separator, encodedCharacteristics);
            return CheckSumAppender.Append(hardwareKey);
        }


        /// <summary>
        /// Checks if at least 2 of 4 hardware components are valid.
        /// Note: CheckSum is ignored by this check.
        /// </summary>
        /// <param name="hardwareIdentifier">
        /// The hardware identifier to check
        /// </param>
        /// <returns>
        /// True if at least 2 of 4 hardware components are valid.
        /// </returns>
        public static bool IsValidForCurrentComputer(string hardwareIdentifier)
        {
            if (hardwareIdentifier == NoHardwareIdentifier)
                return true;
            return ArePartialEqual(hardwareIdentifier, ForCurrentComputer());
        }

        /// <summary>
        /// Returns true if at least 2 of the sub codes are equal.
        /// </summary>
        /// <param name="hardwareIdentifier1"></param>
        /// <param name="hardwareIdentifier2"></param>
        /// <returns></returns>
        public static bool ArePartialEqual(string hardwareIdentifier1, string hardwareIdentifier2)
        {
            if (hardwareIdentifier1 == null)
                throw new ArgumentNullException(nameof(hardwareIdentifier1));
            if (hardwareIdentifier2 == null)
                throw new ArgumentNullException(nameof(hardwareIdentifier2));

            var splitted1 = hardwareIdentifier1.Split(new [] { Separator }, StringSplitOptions.None).ToArray();
            var splitted2 = hardwareIdentifier2.Split(new [] { Separator }, StringSplitOptions.None).ToArray();

            if (splitted1.Length != splitted2.Length)
                return false;

            var validCharactaristicsCount = 0.0;
            var charactaristicsCount = splitted1.Length - 1; // last number is the check num
            for (int i = 0; i < charactaristicsCount; i++)
                validCharactaristicsCount += (splitted1[i] == splitted2[i] ? 1.0 : 0.0);

            var validCharactaristicsRatio = charactaristicsCount / validCharactaristicsCount;

            return validCharactaristicsRatio <= 2.1;
        }

        /// <summary>
        /// Checks if the check sum is valid.
        /// </summary>
        /// <param name="hardwareIdentifier">
        /// The hardware identifier to check.
        /// </param>
        /// <returns>
        /// true if the hardware identifier has a valid checksum; otherwise false.
        /// </returns>
        public static bool IsCheckSumValid(string hardwareIdentifier)
        {
            if (hardwareIdentifier == NoHardwareIdentifier)
                return true;
            return CheckSumAppender.Verify(hardwareIdentifier);
        }

        internal static string NoHardwareIdentifier = "NO_HARDWARE_ID";
    }
}
