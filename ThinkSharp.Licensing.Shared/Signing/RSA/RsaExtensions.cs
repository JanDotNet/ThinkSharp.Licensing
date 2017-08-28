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
        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, string base64EncodedCsbBlobKey)
        {
            return signer.WithRsaPrivateKey(Convert.FromBase64String(base64EncodedCsbBlobKey));
        }

        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, byte[] csbBlobKey)
        {
            var rsaSigner = new RsaSigner(csbBlobKey);
            signer.WithSigner(rsaSigner);
            return signer as IBuilder_HardwareIdentifier;
        }

        public static IBuilder_HardwareIdentifier WithRsaPrivateKey(this IBuilder_Signer signer, RSAParameters rsaParameters)
        {
            var rsaSigner = new RsaSigner(rsaParameters);
            signer.WithSigner(rsaSigner);
            return signer as IBuilder_HardwareIdentifier;
        }

        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, string base64EncodedCsbBlobKey)
        {
            return signer.WithRsaPublicKey(Convert.FromBase64String(base64EncodedCsbBlobKey));
        }

        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, byte[] csbBlobKey)
        {
            var rsaSigner = new RsaSigner(csbBlobKey);
            signer.WithSigner(rsaSigner);
            return signer as IVerifier_ApplicationCode;
        }

        public static IVerifier_ApplicationCode WithRsaPublicKey(this IVerifier_Signer signer, RSAParameters rsaParameters)
        {
            var rsaSigner = new RsaSigner(rsaParameters);
            signer.WithSigner(rsaSigner);
            return signer as IVerifier_ApplicationCode;
        }

        public static SigningKeyPair GenerateRsaKeyPair(this IKeyGenerator keyGenerator)
        {
            var cp = new RSACryptoServiceProvider();
            var privateKey = Convert.ToBase64String(cp.ExportCspBlob(true));
            var publicKey = Convert.ToBase64String(cp.ExportCspBlob(false));

            return new SigningKeyPair(publicKey, privateKey);
        }
    }
}
