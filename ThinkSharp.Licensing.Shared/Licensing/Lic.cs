using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.Licensing;

namespace ThinkSharp.Licensing
{    public class Test
    {
        public void Test123()
        {           
            var signedLicense = Lic.Builder
                .WithRsaPrivateKey("privateKey")
                .WithHardwareIdentifier(HardwareIdentifier.ForCurrentComputer()) // WithoutHardwareIdentifier
                .WithSerialNumber(SerialNumber.Create("ABV"))                    // WithoutSerialNumber
                .ExpiresIn(TimeSpan.FromDays(12))                                // WithoutExpiration
                .WithProperty("test", "test")
                .SignAndCreate();

            signedLicense = Lic.Verifier
                .WithRsaPublicKey("privateKey")
                .WithApplicationCode("ABC")
                .LoadAndVerify("LicenseString");
        }
    }
    public static class Lic
    {
        public static IBuilder_Signer Builder => new LicBuilder();
        public static IKeyGenerator KeyGenerator => new LicKeyGenerator();

        public static IVerifier_Signer Verifier => new LicVerifier();
    }

    internal class LicVerifier : IVerifier_Signer, IVerifier_ApplicationCode, IVerifier_VerifyLoad
    {
        private ISigner mySigner;
        private string myApplicationCode = string.Empty;

        SignedLicense IVerifier_VerifyLoad.LoadAndVerify(string licenseString)
        {
            var license = SignedLicense.Deserialize(licenseString);
            // verfiy signature
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
        IVerifier_ApplicationCode WithSigner(ISigner signer);
    }

    public interface IVerifier_ApplicationCode
    {
        IVerifier_VerifyLoad WithApplicationCode(string threeLetterApplicationCode);
        IVerifier_VerifyLoad WithoutApplicationCode();
    }

    public interface IVerifier_VerifyLoad
    {
        SignedLicense LoadAndVerify(string license);
    }
}
