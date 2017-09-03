// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ThinkSharp.Licensing.Shared.Test
{
    [TestClass]
    public class LicDotNetFullTest
    {
        [TestMethod]
        public void TestVerifier_Full()
        {
            var expireDate = DateTime.Parse("2100-01-01", CultureInfo.InvariantCulture);
            var license = Lic.Builder
                .WithRsaPrivateKey(LicTest.PrivateKey)
                .WithHardwareIdentifier(HardwareIdentifier.ForCurrentComputer())
                .WithSerialNumber(SerialNumber.Create("ABC"))
                .ExpiresOn(expireDate)
                .WithProperty("Prop1", "Value1")
                .WithProperty("Prop2", "Value2")
                .SignAndCreate();

            var verifiedLicense = Lic.Verifier
                .WithRsaPublicKey(LicTest.PublicKey)
                .WithApplicationCode("ABC")
                .LoadAndVerify(license.Serialize());

            Assert.AreEqual(license.Serialize(), verifiedLicense.Serialize());

            verifiedLicense = Lic.Verifier
                .WithRsaPublicKey(LicTest.PublicKey)
                .WithApplicationCode("ABC")
                .LoadAndVerify(license.SerializeAsPlainText());

            Assert.AreEqual(license.Serialize(), verifiedLicense.Serialize());
        }

        [TestMethod]
        public void TestVerifier_Without()
        {
            var license = Lic.Builder
                .WithRsaPrivateKey(LicTest.PrivateKey)
                .WithoutHardwareIdentifier()
                .WithoutSerialNumber()
                .WithoutExpiration()
                .SignAndCreate();

            var verifiedLicense = Lic.Verifier
                .WithRsaPublicKey(LicTest.PublicKey)
                .WithoutApplicationCode()
                .LoadAndVerify(license.Serialize());

            Assert.AreEqual(license.Serialize(), verifiedLicense.Serialize());

            verifiedLicense = Lic.Verifier
                .WithRsaPublicKey(LicTest.PublicKey)
                .WithoutApplicationCode()
                .LoadAndVerify(license.SerializeAsPlainText());

            Assert.AreEqual(license.Serialize(), verifiedLicense.Serialize());
        }
    }
}
