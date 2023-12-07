// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace ThinkSharp.Licensing
{
    internal class SignedLicenseEncryption
    {
        public static string Encrypt(string license)
        {
            var confusingBytes = new byte[] { 32, 45, 12, 43, 33, 1 };
            var bytes = Encoding.UTF8.GetBytes(license);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= confusingBytes[i % confusingBytes.Length];
            }
            return Convert.ToBase64String(bytes);
        }

        public static string Dencrypt(string input)
        {
            var confusingBytes = new byte[] { 32, 45, 12, 43, 33, 1 };
            var bytes = Convert.FromBase64String(input);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= confusingBytes[i % confusingBytes.Length];
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
