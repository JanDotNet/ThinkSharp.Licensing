using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.Licensing
{
    public class SignedLicenseException : Exception
    {
        public SignedLicenseException(string message)
            : base(message)
        { }
    }
}
