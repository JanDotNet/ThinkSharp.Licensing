// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
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
