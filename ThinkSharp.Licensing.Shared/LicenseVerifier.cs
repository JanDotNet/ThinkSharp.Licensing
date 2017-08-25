// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.LicenseSource;
using ThinkSharp.Licensing;
using ThinkSharp.Signing;
using ThinkSharp.Signing.RSA;

namespace ThinkSharp
{
    public sealed class LicenseVerifier : ILicenseVerifier
    {
        private readonly ILicenseSource mySource;
        private readonly ISigner mySigner;

        public LicenseVerifier(ILicenseSource source, byte[] publicKey)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (publicKey == null)
                throw new ArgumentNullException(nameof(publicKey));

            mySource = source;
            mySigner = new RsaSigner(publicKey);
        }

        public SignedLicense GetLicense()
        {
            var licenseStr = mySource.Read();
            if (string.IsNullOrEmpty(licenseStr))
                return null;

            var license = SignedLicense.Deserialize(licenseStr);
            license.Verify(mySigner);
            return license;
        }
    }
}
