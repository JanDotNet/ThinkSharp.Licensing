using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Activation.Test.Signing;

namespace ThinkSharp.Licensing
{
    [TestClass]
    public class SignedLicenseTest
    {
        [TestMethod]
        public void TestInitialization()
        {
            var file = new SignedLicense("Me", "My Company", "ActivationCode", null);
            AssertDefaultPropertiesAreValid(file);
            Assert.AreEqual(0, file.Properties.Count);
        }

        [TestMethod]
        public void TestInitialization_WithProperties()
        {
            var file = new SignedLicense("Me", "My Company", "ActivationCode", CreateProperties());
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
                var file = new SignedLicense("Me", "My Company", "ActivationCode", properties);
                Assert.Fail("FormatException expected.");
            }
            catch (FormatException) { }
        }

        [TestMethod]
        public void TestSigning_WithoutProperties()
        {
            var file = new SignedLicense("Me", "My Company", "ActivationCode", null);
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
            var file = new SignedLicense("Me", "My Company", "ActivationCode", CreateProperties());
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
            var file = new SignedLicense("Me", "My Company", "ActivationCode", CreateProperties());
            file.Sign(new HashCodeSigner());
            var content = file.Serialize();
            Thread.CurrentThread.CurrentCulture = cultureEN;
            var newFile = SignedLicense.Deserialize(content);
            newFile.Verify(new HashCodeSigner());
        }


        private static void AssertDefaultPropertiesAreValid(SignedLicense file)
        {
            Assert.AreEqual("My Company", file.Company);
            Assert.AreEqual(DateTime.UtcNow.Date, file.IssueDate);
            Assert.AreEqual("Me", file.Licensee);
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
