using System;
using ThinkSharp.LicenseSource;

namespace ThinkSharp.Licensing.Shared.LicenseSource
{
    public class InMemoryLicenseSource : ILicenseSource
    {
        private readonly string myLicense;

        public InMemoryLicenseSource(string license)
        {
            myLicense = license;
        }

        public string Read()
        {
            return myLicense;
        }
    }
}
