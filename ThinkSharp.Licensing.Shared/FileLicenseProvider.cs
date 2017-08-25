using System;
using System.ComponentModel;
using ThinkSharp.LicenseSource;
using ThinkSharp.Licensing;

namespace ThinkSharp
{
    public abstract class FileLicenseProvider<TLicense> : LicenseProvider where TLicense : License
    {
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            var source = new LicenseContextSource(context, type, LicenseSource);
            var licenseManager = new LicenseVerifier(source, PublicKey);

            var license = licenseManager.GetLicense();
            return CreateLicense(license);
        }

        protected abstract byte[] PublicKey { get; }

        protected abstract ILicenseSource LicenseSource { get; }

        protected abstract TLicense CreateLicense(SignedLicense singedLicense);
    }
}
