// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using System.IO;

namespace ThinkSharp.LicenseSource
{
    internal class LicenseContextSource : ILicenseSource
    {
        private readonly LicenseContext myContext;
        private readonly Type myType;
        private readonly ILicenseSource myLicensesource;

        public LicenseContextSource(LicenseContext context, Type type, ILicenseSource licensesource)
        {
            myContext = context;
            myType = type;
            myLicensesource = licensesource;
        }

        public string Read()
        {
            string licenseStr = null;

            if (myContext != null)
                licenseStr = myContext.GetSavedLicenseKey(myType, null);
            if (licenseStr != null)
                return licenseStr;
            
            licenseStr = myLicensesource.Read();
            if (licenseStr != null)
                myContext?.SetSavedLicenseKey(myType, licenseStr);

            // return null because the license should applied only if loaded from context.
            return null;
        }
    }
}
