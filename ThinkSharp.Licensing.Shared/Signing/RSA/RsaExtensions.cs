// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Security.Cryptography;
using ThinkSharp.Signing;
using ThinkSharp.Signing.RSA;

namespace ThinkSharp.Licensing
{
    public static class RsaExtensions
    {
        /// <summary>
        /// Uses RSA algorithm for signing the license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="base64EncodedCsbBlobKey">
        /// The base64 encoded CSB BLOB key that contains the public and private key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, string base64EncodedCsbBlobKey)
        {
            return signer.WithRsaPrivateKey(Convert.FromBase64String(base64EncodedCsbBlobKey));
        }

        /// <summary>
        /// Uses RSA algorithm for signing the license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="csbBlobKey">
        /// The CSB BLOB key that contains the public and private key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, byte[] csbBlobKey)
        {
            var rsaSigner = new RsaSigner(csbBlobKey);
            signer.WithSigner(rsaSigner);
            return signer as IBuilder_HardwareIdentifier;
        }

        /// <summary>
        /// Uses RSA algorithm for signing the license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="rsaParameters">
        /// The <see cref="RSAParameters" /> object that contains the public and private key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, RSAParameters rsaParameters)
        {
            var rsaSigner = new RsaSigner(rsaParameters);
            signer.WithSigner(rsaSigner);
            return signer as IBuilder_HardwareIdentifier;
        }

        /// <summary>
        /// Uses RSA algorithm for verifying the signature of a signed license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="base64EncodedCsbBlobKey">
        /// The base64 encoded CSB BLOB key that contains the public key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, string base64EncodedCsbBlobKey)
        {
            return signer.WithRsaPublicKey(Convert.FromBase64String(base64EncodedCsbBlobKey));
        }

        /// <summary>
        /// Uses RSA algorithm for verifying the signature of a signed license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="csbBlobKey">
        /// The CSB BLOB key that contains the public key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, byte[] csbBlobKey)
        {
            var rsaSigner = new RsaSigner(csbBlobKey);
            signer.WithSigner(rsaSigner);
            return signer as IVerifier_ApplicationCode;
        }

        /// <summary>
        /// Uses RSA algorithm for verifying the signature of a signed license with the specified private key.
        /// </summary>
        /// <param name="signer">
        /// The fluent API object to extend.
        /// </param>
        /// <param name="rsaParameters">
        /// The <see cref="RSAParameters" /> object that contains the public key.
        /// </param>
        /// <returns>
        /// The next fluent API object.
        /// </returns>
        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, RSAParameters rsaParameters)
        {
            var rsaSigner = new RsaSigner(rsaParameters);
            signer.WithSigner(rsaSigner);
            return signer as IVerifier_ApplicationCode;
        }

        /// <summary>
        /// Creates a <see cref="SigningKeyPair"/> object that contains a newly created public / private key pair for RSA signing.
        /// </summary>
        /// <param name="keyGenerator">
        /// The fluent API object to extend.
        /// </param>
        /// <returns>
        /// A newly created public / private key pair for RSA signing.
        /// </returns>
        public static SigningKeyPair GenerateRsaKeyPair(this IKeyGenerator keyGenerator)
        {
            var cp = new RSACryptoServiceProvider();
            var privateKey = Convert.ToBase64String(cp.ExportCspBlob(true));
            var publicKey = Convert.ToBase64String(cp.ExportCspBlob(false));

            return new SigningKeyPair(publicKey, privateKey);
        }
    }
}
