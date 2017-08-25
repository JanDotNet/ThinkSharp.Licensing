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
