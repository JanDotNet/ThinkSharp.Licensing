// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using ThinkSharp.Licensing.Signing;

namespace ThinkSharp.Licensing.Test.Signing
{
    public class LengthSigner : ISigner
    {
        public string Sign(string content)
        {
            return content.Length.ToString();
        }

        public bool Verify(string content, string signature)
        {
            return content.Length == int.Parse(signature);
        }
    }

    public class DoubleLengthSigner : ISigner
    {
        public string Sign(string content)
        {
            return (content.Length * 2).ToString();
        }

        public bool Verify(string content, string signature)
        {
            return (content.Length * 2) == int.Parse(signature);
        }
    }

    public class HashCodeSigner : ISigner
    {
        public string Sign(string content)
        {
            int hash = 0;
            foreach (var c in content)
                hash += (int)c;
            return hash.ToString();
        }

        public bool Verify(string content, string signature)
        {
            int hash = 0;
            foreach (var c in content)
                hash += (int)c;

            return hash == int.Parse(signature);
        }
    }
}
