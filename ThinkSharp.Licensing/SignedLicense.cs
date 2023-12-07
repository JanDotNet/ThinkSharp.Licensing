// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using ThinkSharp.Licensing.Helper;
using ThinkSharp.Licensing.Signing;

namespace ThinkSharp.Licensing
{
    /// <summary>
    /// Class that encapsulates some license related information and a signature for verifying it
    /// </summary>
    public sealed class SignedLicense
    {
        //  .ctor
        // ////////////////////////////////////////////////////////////////////

        internal SignedLicense(string hardwareIdentifier, string serialNumber, DateTime issueDate, DateTime expirationDate, IDictionary<string, string> properties)
            : this(hardwareIdentifier, serialNumber, issueDate, expirationDate, properties, null)
        {  }

        private SignedLicense(string hardwareIdentifier, string serialNumber, DateTime issueDate, DateTime expirationDate, IDictionary<string, string> properties, string signature)
        {
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            HardwareIdentifier = hardwareIdentifier ?? "";
            SerialNumber = serialNumber ?? "";
            Signature = signature;
            var dict = properties ?? new Dictionary<string, string>();
            if (dict.Keys.Any(key => key.Contains(":")))
                throw new FormatException("Character ':' is not allowed in property key.");

            Properties = new ReadOnlyDictionary<string, string>(dict);
        }

        //  Properties
        // ////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Date of the issuing (when the license was created)
        /// </summary>
        public DateTime IssueDate { get; }
        /// <summary>
        /// Date of the expiration (may be <see cref="DateTime.MaxValue"/> for licenses without expiration)
        /// </summary>
        public DateTime ExpirationDate { get; }
        /// <summary>
        /// Optional: A serial number (See also <see cref="SerialNumber"/>)
        /// </summary>
        public string SerialNumber { get; }
        /// <summary>
        /// Optional: A hardware identifier (See also <see cref="HardwareIdentifier"/>)
        /// </summary>
        internal string HardwareIdentifier { get; }
        private string Signature { get; set; }
        /// <summary>
        /// List of custom key value pairs that are part of the license.
        /// </summary>
        public IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Gets a value that indicates if the license has a serial number.
        /// </summary>
        public bool HasSerialNumber => SerialNumber != Licensing.SerialNumber.NoSerialNumber;

        /// <summary>
        /// Gets a value that indicates if the license has a hardware identifier.
        /// </summary>
        public bool HasHardwareIdentifier => HardwareIdentifier != Licensing.HardwareIdentifier.NoHardwareIdentifier;

        /// <summary>
        /// Gets a value that indicates if the license expires.
        /// </summary>
        public bool HasExpirationDate => ExpirationDate != DateTime.MaxValue;

        //  Methods
        // ////////////////////////////////////////////////////////////////////

        // - Load
        internal static SignedLicense Deserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException($"'{nameof(content)}' must not null or empty.");
            var firstLine = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).First();
            var isEncrypted = !Licensing.HardwareIdentifier.IsCheckSumValid(firstLine);
            if (isEncrypted)
            {
                content = content.Unwrap();
                content = SignedLicenseEncryption.Dencrypt(content);
            }
            var lines = (content ?? "").Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 4)
                ThrowInvalidFormatException();

            return ReadLicenseFile(lines);
        }

        /// <summary>
        /// Serializes the license as plain text (license information are readable by humans).
        /// </summary>
        /// <returns></returns>
        public string SerializeAsPlainText()
        {
            var sb = new StringBuilder();
            WriteLicenseProperties(sb);
            WriteSignature(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Serializes the license as encrypted base64 encoded text.
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return SignedLicenseEncryption.Encrypt(SerializeAsPlainText());
        }

        private static SignedLicense ReadLicenseFile(string[] lines)
        {
            try
            {
                var index = 0;
                var hardwareIdentifier = lines[index++];
                var serialNumber = lines[index++];
                var issueDate = DateTime.Parse(lines[index++], CultureInfo.InvariantCulture);
                var expirationDate = DateTime.Parse(lines[index++], CultureInfo.InvariantCulture);
                var signature = lines.Last();

                var properties = new Dictionary<string, string>();
                foreach (var line in lines.Skip(index).Take(lines.Length - (index + 1)))
                {
                    var pair = GetKeyValuePair(line);
                    properties.Add(pair.Key, pair.Value);
                }

                return new SignedLicense(hardwareIdentifier, serialNumber, issueDate, expirationDate, properties, signature);
            }
            catch
            {
                ThrowInvalidFormatException();
                return null;
            }
        }

        internal void Verify(ISigner signer)
        {
            var sb = new StringBuilder();

            WriteLicenseProperties(sb);

            if (!signer.Verify(sb.ToString(), Signature))
                ThrowInvalidSignatureException();
        }

        internal void Sign(ISigner signer)
        {
            var sb = new StringBuilder();

            WriteLicenseProperties(sb);

            Signature = signer.Sign(sb.ToString());
        }

        private void WriteSignature(StringBuilder sb)
        {
            if (string.IsNullOrEmpty(Signature))
                ThrowNotSignedException();
            sb.Append(Signature); // note: Append because it is the last line
        }

        private void WriteLicenseProperties(StringBuilder sb)
        {
            sb.AppendLine(HardwareIdentifier);
            sb.AppendLine(SerialNumber);
            sb.AppendLine(IssueDate.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine(ExpirationDate.ToString(CultureInfo.InvariantCulture));
            foreach (var property in Properties)
                sb.AppendLine(property.Key + ":" + property.Value);
        }

        private static KeyValuePair<string, string> GetKeyValuePair(string line)
        {
            var index = line.IndexOf(':');
            if (index < 0)
                ThrowInvalidFormatException();
            var key = line.Substring(0, index);
            var value = line.Substring(index + 1);
            return new KeyValuePair<string, string>(key, value);
        }

        // - Throw helper
        private static void ThrowInvalidFormatException()
        {
            var msg = "License file has not a valid format.";
            throw new SignedLicenseException(msg);
        }

        private static void ThrowNotSignedException()
        {
            var msg = "License file is not signed.";
            throw new SignedLicenseException(msg);
        }

        private static void ThrowInvalidSignatureException()
        {
            var msg = "Signature of license file is not valid.";
            throw new SignedLicenseException(msg);
        }
    }
}
