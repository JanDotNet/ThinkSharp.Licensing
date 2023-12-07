// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Licensing.Test.Signing;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class SignedLicenseTest
    {
        [TestMethod]
        public void TestInitialization()
        {
            var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), null);
            AssertDefaultPropertiesAreValid(file);
            Assert.AreEqual(0, file.Properties.Count);
        }

        [TestMethod]
        public void TestSerialization()
        {
            var license = new SignedLicense(HardwareIdentifier.NoHardwareIdentifier, "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), CreateProperties());
            license.Sign(new LengthSigner());
            var licPlainText = license.SerializeAsPlainText();
            var licEncrypted = license.Serialize();

            var licensePlainText = SignedLicense.Deserialize(licPlainText);
            var licenseEncrypted = SignedLicense.Deserialize(licEncrypted);

            Assert.AreEqual(licensePlainText.SerializeAsPlainText(), licenseEncrypted.SerializeAsPlainText());
            Assert.AreEqual(licensePlainText.Serialize(), licenseEncrypted.Serialize());
        }

        [TestMethod]
        public void TestInitialization_WithProperties()
        {
            var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), CreateProperties());
            AssertDefaultPropertiesAreValid(file);
            AssertPropertiesAreValid(file);
        }

        [TestMethod]
        public void TestInitialization_WithProperties_WithColone()
        {
            var properties = new Dictionary<string, string>();
            properties.Add("Pro:p1", "Val1");
            properties.Add("Prop2", "Val2");
            try
            {
                var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), properties);
                Assert.Fail("FormatException expected.");
            }
            catch (FormatException) { }
        }

        [TestMethod]
        public void TestSigning_WithoutProperties()
        {
            var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), null);
            file.Sign(new LengthSigner());
            var content = file.Serialize();
            var newFile = SignedLicense.Deserialize(content);

            AssertDefaultPropertiesAreValid(newFile);
            Assert.AreEqual(0, newFile.Properties.Count);
            newFile.Verify(new LengthSigner());

            try
            {
                newFile.Verify(new DoubleLengthSigner());
                Assert.Fail("LicenseException expected");
            }
            catch (SignedLicenseException) { }
        }

        [TestMethod]
        public void TestSigning_WithProperties()
        {
            var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), CreateProperties());
            file.Sign(new LengthSigner());
            var content = file.Serialize();
            var newFile = SignedLicense.Deserialize(content);

            AssertDefaultPropertiesAreValid(newFile);
            AssertPropertiesAreValid(file);
            newFile.Verify(new LengthSigner());

            try
            {
                newFile.Verify(new DoubleLengthSigner());
                Assert.Fail("LicenseException expected");
            }
            catch (SignedLicenseException) { }
        }

        [TestMethod]
        public void TestSigning_WithProperties_DifferentCultures()
        {
            var cultureDE = new CultureInfo("de");
            var cultureEN = new CultureInfo("en");

            
            Thread.CurrentThread.CurrentCulture = cultureDE;
            var file = new SignedLicense("HardwareID", "SerialNumber", DateTime.Now, DateTime.Now + TimeSpan.FromDays(10), CreateProperties());
            file.Sign(new HashCodeSigner());
            var content = file.Serialize();
            Thread.CurrentThread.CurrentCulture = cultureEN;
            var newFile = SignedLicense.Deserialize(content);
            newFile.Verify(new HashCodeSigner());
        }

        [TestMethod]
        public void TestHasXXXProperties_True()
        {
            var key = "BwIAAACkAABSU0EyAAQAAAEAAQALrzxPFiBp4EN5aeLtZZ4sTvcfYn+fSpmxJvhSUxP/9fm+uaWwJ+n7+jc4Zf2tB+WDulTJo5ryauHgKjx5MHMmWNLr77mD3ws11BC61VDt65fIY4DLsvn49ZYajJy3oUwDvyEsnGArUH3IUhCTv/OWbHovmb69Xlg90mEcsIhOm2WKki+1cc7ZeBANtR57SMLv3qDH+DQqTxBb4UOHmJs4YfrMjqOEXBg0pDLT4HHzIz7WVu9ltKJdQZn626aGdMivhKQqxiJj3YsdFiLgM4BZk9ZGCxI2AJLp9Q/IRwqGKg4T0NlNWbqH1P5Zvq2nxVxSQEI/ARpUK1C8CIsnXVfGyRNp5nkFlM05O2HXhwLWhoHn5Dm76FMz5mClkFaRe8pK113sZK0Tw6sEVCrFMXeaiXSGK9xZifvnioOq9jRgp1fCpc5vLSE4VFGZ2vl89hrsfPKAIDIx5vXgNfOGjJXoDfHUkurih1qBG5Aiie5bD8e+LPVQ7jkM9CzFHRC756n1yAwSLUiv2cpbXD/YhZuHOOIljWjgcHKV9b9eyQXUilC8CQOE/1JLUHWLskhRH6NKRo1HVPxsuZpKLfkWEqti6TF8A4gllaJdjJEqq226EaXaRvP2RoqxjRBUKeT/NBN4focrEmjQpMKTAhMmpIutsXEqLbcXbR+0gBbvdIoEcEBbgizGJK8NJtWnli6qh4EEuaqtBYXkP0Io/bZJsc+WuWpHn9lXWIy/cPDTik+uEgbPF5MZEcmLVKJEsdnpcEc=";

            var lic = Lic.Builder
                .WithRsaPrivateKey(key)
                .WithHardwareIdentifier("HardwareIdentifier")
                .WithSerialNumber(SerialNumber.Create("GSA"))
                .ExpiresIn(TimeSpan.FromDays(100))
                .WithProperty("Name", "Bill Gates")
                .WithProperty("Company", "Microsoft")
                .SignAndCreate();

            Assert.IsTrue(lic.HasExpirationDate);
            Assert.IsTrue(lic.HasHardwareIdentifier);
            Assert.IsTrue(lic.HasSerialNumber);
        }

        [TestMethod]
        public void TestHasXXXProperties_False()
        {
            var key = "BwIAAACkAABSU0EyAAQAAAEAAQALrzxPFiBp4EN5aeLtZZ4sTvcfYn+fSpmxJvhSUxP/9fm+uaWwJ+n7+jc4Zf2tB+WDulTJo5ryauHgKjx5MHMmWNLr77mD3ws11BC61VDt65fIY4DLsvn49ZYajJy3oUwDvyEsnGArUH3IUhCTv/OWbHovmb69Xlg90mEcsIhOm2WKki+1cc7ZeBANtR57SMLv3qDH+DQqTxBb4UOHmJs4YfrMjqOEXBg0pDLT4HHzIz7WVu9ltKJdQZn626aGdMivhKQqxiJj3YsdFiLgM4BZk9ZGCxI2AJLp9Q/IRwqGKg4T0NlNWbqH1P5Zvq2nxVxSQEI/ARpUK1C8CIsnXVfGyRNp5nkFlM05O2HXhwLWhoHn5Dm76FMz5mClkFaRe8pK113sZK0Tw6sEVCrFMXeaiXSGK9xZifvnioOq9jRgp1fCpc5vLSE4VFGZ2vl89hrsfPKAIDIx5vXgNfOGjJXoDfHUkurih1qBG5Aiie5bD8e+LPVQ7jkM9CzFHRC756n1yAwSLUiv2cpbXD/YhZuHOOIljWjgcHKV9b9eyQXUilC8CQOE/1JLUHWLskhRH6NKRo1HVPxsuZpKLfkWEqti6TF8A4gllaJdjJEqq226EaXaRvP2RoqxjRBUKeT/NBN4focrEmjQpMKTAhMmpIutsXEqLbcXbR+0gBbvdIoEcEBbgizGJK8NJtWnli6qh4EEuaqtBYXkP0Io/bZJsc+WuWpHn9lXWIy/cPDTik+uEgbPF5MZEcmLVKJEsdnpcEc=";

            var lic = Lic.Builder
                .WithRsaPrivateKey(key)
                .WithoutHardwareIdentifier()
                .WithoutSerialNumber()
                .WithoutExpiration()
                .WithProperty("Name", "Bill Gates")
                .WithProperty("Company", "Microsoft")
                .SignAndCreate();

            Assert.IsFalse(lic.HasExpirationDate);
            Assert.IsFalse(lic.HasHardwareIdentifier);
            Assert.IsFalse(lic.HasSerialNumber);
        }


        private static void AssertDefaultPropertiesAreValid(SignedLicense file)
        {
            Assert.AreEqual("HardwareID", file.HardwareIdentifier);
            Assert.AreEqual(DateTime.UtcNow.Date, file.IssueDate.Date);
            Assert.AreEqual("SerialNumber", file.SerialNumber);
        }

        private static Dictionary<string, string> CreateProperties()
        {
            var properties = new Dictionary<string, string>();
            properties.Add("Prop1", "Val1");
            properties.Add("Prop2", "Val2");
            return properties;
        }

        private static void AssertPropertiesAreValid(SignedLicense file)
        {
            Assert.AreEqual(2, file.Properties.Count);
            Assert.AreEqual("Val1", file.Properties["Prop1"]);
            Assert.AreEqual("Val2", file.Properties["Prop2"]);
        }
    }
}
