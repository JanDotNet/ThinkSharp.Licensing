// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using ThinkSharp.Licensing.Signing;

namespace ThinkSharp.Licensing
{
    /// <summary>
    /// Entry point of fluent API for working with licenses.
    /// </summary>
    public static class Lic
  {
    /// <summary>
    /// Creates a new builder for creating signed license objects.
    /// </summary>
    public static IBuilder_Signer Builder => new LicBuilder();

    /// <summary>
    /// Creates a new key generator for creating private / public key pairs.
    /// </summary>
    public static IKeyGenerator KeyGenerator => new LicKeyGenerator();

    /// <summary>
    /// Creates a new verifier for verify license strings.
    /// </summary>
    public static IVerifier_Signer Verifier => new LicVerifier();
  }

  internal class LicVerifier : IVerifier_Signer, IVerifier_ApplicationCode, IVerifier_VerifyLoad
  {
    private ISigner mySigner;
    private string myApplicationCode = string.Empty;

    SignedLicense IVerifier_VerifyLoad.LoadAndVerify(string licenseString)
    {
      var license = SignedLicense.Deserialize(licenseString);
      // verify signature
      license.Verify(mySigner);
      // verify application code
      if (!SerialNumber.IsApplicationCodeValid(license.SerialNumber, myApplicationCode))
        throw new SignedLicenseException($"Application Code '{myApplicationCode}' is not valid for the license.");
      // verify hardware identifier
      if (!HardwareIdentifier.IsValidForCurrentComputer(license.HardwareIdentifier))
        throw new SignedLicenseException($"License has been activated for another computer.");
      // verify expiration date
      if (license.ExpirationDate < DateTime.UtcNow)
        throw new SignedLicenseException($"License has been expired since '{license.ExpirationDate}'.");
      return license;
    }

    IVerifier_VerifyLoad IVerifier_ApplicationCode.WithApplicationCode(string threeLetterApplicationCode)
    {
      myApplicationCode = SerialNumber.EnsureApplicationCodeIsValid(threeLetterApplicationCode);
      return this;
    }

    IVerifier_VerifyLoad IVerifier_ApplicationCode.WithoutApplicationCode()
    {
      return this;
    }

    IVerifier_ApplicationCode IVerifier_Signer.WithSigner(ISigner signer)
    {
      mySigner = signer ?? throw new ArgumentNullException(nameof(signer));
      return this;
    }
  }

  public interface IVerifier_Signer
  {
    /// <summary>
    /// Use the specified signer.
    /// </summary>
    /// <param name="signer">
    /// The <see cref="ISigner"/> implementation to use.
    /// </param>
    /// <returns></returns>
    IVerifier_ApplicationCode WithSigner(ISigner signer);
  }

  public interface IVerifier_ApplicationCode
  {
    /// <summary>
    /// Verify that the serial number has the specified application code.
    /// </summary>
    /// <param name="threeLetterApplicationCode">
    /// The application code to check.
    /// </param>
    /// <returns></returns>
    IVerifier_VerifyLoad WithApplicationCode(string threeLetterApplicationCode);

    /// <summary>
    /// The signed license was created without serial number.
    /// </summary>
    /// <returns></returns>
    IVerifier_VerifyLoad WithoutApplicationCode();
  }

  public interface IVerifier_VerifyLoad
  {
    /// <summary>
    /// Loads the specified license and verifies it.
    /// NOTE: If the license is not valid, an exception will be thrown.
    /// </summary>
    /// <param name="license">
    /// The serialized license string (either encrypted and base64 encoded or plain text)
    /// </param>
    /// <returns></returns>
    SignedLicense LoadAndVerify(string license);
  }
}
