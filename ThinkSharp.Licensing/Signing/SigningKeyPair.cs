// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace ThinkSharp.Licensing.Signing
{
    /// <summary>
    /// Represents a public/private key pair.
    /// </summary>
    public class SigningKeyPair
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="publicKey">
        /// The public key.
        /// </param>
        /// <param name="privateKey">
        /// The private key.
        /// </param>
        public SigningKeyPair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        /// <summary>
        /// Gets the public key.
        /// </summary>
        public string PublicKey { get; }
        /// <summary>
        /// Gets the private key.
        /// </summary>
        public string PrivateKey { get; }
    }
}
