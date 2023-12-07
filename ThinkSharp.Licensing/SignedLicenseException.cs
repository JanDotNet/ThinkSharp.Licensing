// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace ThinkSharp.Licensing
{
    public class SignedLicenseException : Exception
    {
        public SignedLicenseException(string message)
            : base(message)
        { }
    }
}
