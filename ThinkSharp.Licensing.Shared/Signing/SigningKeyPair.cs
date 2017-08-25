// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkSharp.Signing
{
    public class SigningKeyPair
    {
        public SigningKeyPair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public string PublicKey { get; }
        public string PrivateKey { get; }
    }
}
