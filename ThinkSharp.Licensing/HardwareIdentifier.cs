// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.Licensing
{
    /// <summary>
    /// Interface that abstracts an OS specific implementation for creating characteristics of the current computer.
    /// </summary>
    public interface IComputerCharacteristics
    {
        /// <summary>
        /// Gets a list of characteristics for the current computer.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetCharacteristicsForCurrentComputer();
    }

    /// <summary>
    /// Class that provides methods for generating / verifying hardware identifiers.
    /// </summary>
    public static class HardwareIdentifier
    {        
        private static readonly CheckSumAppender CheckSumAppender = new CheckSumAppender(Separator, new CheckSum(4));
        private static readonly HashAlgorithm MD5Algorithm = MD5.Create();
        private static readonly Encoder Encoder = new Encoder(PartSize);

        private const int PartSize = 8;
        private const string Separator = "-";
        private static IComputerCharacteristics theComputerCharacteristics = null;

        static HardwareIdentifier()
        {
#if NET6_0_OR_GREATER
            if (OperatingSystem.IsWindows())
#endif
            theComputerCharacteristics = new WindowsComputerCharacteristics();
        }

        /// <summary>
        /// Sets the <see cref="IComputerCharacteristics"/> implementation to use.
        /// </summary>
        /// <remarks>
        /// Not that the implementation depends on the actual OS. If no implementation is set, some methods like
        /// <see cref="ForCurrentComputer"/> and <see cref="IsValidForCurrentComputer(string)"/> throw an exception.
        /// </remarks>
        /// <param name="computerCharacteristics">
        /// An implementation of <see cref="IComputerCharacteristics"/>.
        /// </param>
        public static void SetComputerCharacteristics(IComputerCharacteristics computerCharacteristics)
        {
            theComputerCharacteristics = computerCharacteristics;
        }

        /// <summary>
        /// Gets the hardware identifier for the current computer.
        /// NOTE: <see cref="SetComputerCharacteristics(IComputerCharacteristics)"/> must be set before using this method.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        /// Thrown if <see cref="SetComputerCharacteristics(IComputerCharacteristics)"/> was not set before.
        /// </exception>
        public static string ForCurrentComputer()
        {
            if (theComputerCharacteristics == null)
                throw new NotSupportedException("No ComputerCharacteristics service is set. Please use 'HardwareIdentifier.SetComputerCharacteristics' to set a plattform specific implementation of 'IComputerCharacteristics'.");

            var encodedCharacteristics = new List<string>();
            foreach (var characteristic in theComputerCharacteristics.GetCharacteristicsForCurrentComputer())
            {

                var bytes = Encoding.UTF8.GetBytes(characteristic);
                bytes = MD5Algorithm.ComputeHash(bytes);

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
        /// <param name="hardwareIdentifier1">
        /// The first hardware identifier to compare.
        /// </param>
        /// <param name="hardwareIdentifier2">
        /// The second hardware identifier to compare.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if hardwareIdentifier1 or hardwareIdentifier2 is null.
        /// </exception>
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
            var charactaristicsCount = splitted1.Length - 1; // last number is the check sum
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
        /// true if the hardware identifier has a valid check sum or the hardware 
        /// identifier is equal to <see cref="NoHardwareIdentifier"/>; otherwise false.
        /// </returns>
        public static bool IsCheckSumValid(string hardwareIdentifier)
        {
            if (hardwareIdentifier == NoHardwareIdentifier)
                return true;
            return CheckSumAppender.Verify(hardwareIdentifier);
        }

        internal static readonly string NoHardwareIdentifier = "NO_HARDWARE_ID";
    }
}
