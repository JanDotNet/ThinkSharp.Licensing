// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using ThinkSharp.LicenseSource;
using ThinkSharp.Licensing;

namespace ThinkSharp
{
    public abstract class CustomLicenseProvider<TLicense> : LicenseProvider where TLicense : License
    {
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            var source = new LicenseContextSource(context, type, LicenseSource);
            var licenseManager = new LicenseVerifier(source, PublicKey);

            var license = licenseManager.GetLicense();
            return CreateLicense(license);
        }

        protected  abstract byte[] PublicKey { get; }

        protected abstract ILicenseSource LicenseSource{ get; }

        protected abstract TLicense CreateLicense(SignedLicense singedLicense);
    }
}
