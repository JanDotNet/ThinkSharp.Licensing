// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using ThinkSharp.Helper;

namespace ThinkSharp.Licensing
{
    public sealed class SignedLicense
    {
        //  .ctor
        // ////////////////////////////////////////////////////////////////////

        internal SignedLicense(string hardwareIdentifier, string serialNumber, DateTime issueDate, DateTime expirationDate, IDictionary<string, string> properties)
            : this(hardwareIdentifier, serialNumber, DateTime.UtcNow.Date, expirationDate, properties, null)
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

        public DateTime IssueDate { get; }
        public DateTime ExpirationDate { get; }
        public string SerialNumber { get; }
        internal string HardwareIdentifier { get; }
        private string Signature { get; set; }
        public IDictionary<string, string> Properties { get; }

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

        public string Serialize(bool plainText = false)
        {
            var sb = new StringBuilder();
            WriteLicenseProperties(sb);
            WriteSignature(sb);

            var licenseText = sb.ToString();

            if (plainText)
                return licenseText;

            return SignedLicenseEncryption.Encrypt(licenseText);
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
            catch (Exception ex)
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
