// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace ThinkSharp.Licensing.Signing
{
    /// <summary>
    /// Interface that abstracts a signer.
    /// </summary>
    public interface ISigner
    {
        /// <summary>
        /// Creates a signature for the specified content.
        /// </summary>
        /// <param name="content">
        /// The content to sign.
        /// </param>
        /// <returns>
        /// The signature of the specified content.
        /// </returns>
        string Sign(string content);

        /// <summary>
        /// Verifies, whether the signature is valid for the specified content.
        /// </summary>
        /// <param name="content">
        /// The content to check.
        /// </param>
        /// <param name="signature">
        /// The signature to check.
        /// </param>
        /// <returns>
        /// true if the signature is valid for the specified content; otherwise false.</returns>
        bool Verify(string content, string signature);
    }
}
