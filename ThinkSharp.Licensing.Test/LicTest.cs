// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class LicTest
    {
        // Private
        internal static string PrivateKey = "BwIAAACkAABSU0EyAAQAAAEAAQA9D9kxlsh1N5KPmhOsJj/QM9iZVaho5OzEG0gyO8s0Giycx7ttegjugLE1NY7Gw5FPJvSqrlRiBp9iNCsD9/NUJIa65mwfTsShzoce+v5tRLJrd4osZZ3WFA/e4oSk9BgCJNUWIShj1HKD4Lk1YqGWtaMZnx/uNLe8QZ4FGYkKvOWDl4FaLViZBbGfLBxMoMpPGQVmSbJtlOoqjyQr0J9stuuJCs564uTzXqJU9/ytInlFYGEDOpYanlkio4x38Px5WAF4+EPvplW6IszdwsR+Sd6hkSqwu8IPzkZwU6PsyvPF2tLQgomB4LVh/6gJcDpNCXJLWXm4GG+YuHpiCFG+6fPFbd0vcDc5Y4ByAtUADFQ0q2kdI+8K5znNJBd9xeuTF9mJLFKbvENJ58F+DPCtWLEWf5tYZXicZUfTa/tnmzFQKx1lc3wHYh5DyQttkmvsN4bXrp0whYU1S8eiI22H5meA5C6CiJZKSXWEGAAA2bpSgRt5ltS1WIR+wVGZ5iFkwnrkQldBZxecWCpj5HQexyDPcpsJipSotRllkNP8K8TubP8YafQGntQjeozZ4mOz2+V3f7GuC5DX229fGIv54ULq7ftWS3sqLSYzf1DJbN//bQkZQCAP5s5UXlx3n6G68cmkTaSE4ZP2slleqF6CEAaMS99jh2deYslQu1Jp395XkYoqnkzWiAIuoIYIaUxMn5+HWdqS6gqyGVHdbnWvlncdcoest/0waxkeSC86QqZ77QFzLaKSfDnYpHZ2t1o=";

        // Public
        internal static string PublicKey = "BgIAAACkAABSU0ExAAQAAAEAAQA9D9kxlsh1N5KPmhOsJj/QM9iZVaho5OzEG0gyO8s0Giycx7ttegjugLE1NY7Gw5FPJvSqrlRiBp9iNCsD9/NUJIa65mwfTsShzoce+v5tRLJrd4osZZ3WFA/e4oSk9BgCJNUWIShj1HKD4Lk1YqGWtaMZnx/uNLe8QZ4FGYkKvA==";

        [TestMethod]
        public void TestBuilder_Full()
        {
            var expireDate = DateTime.Parse("2100-01-01", CultureInfo.InvariantCulture);
            var license = Lic.Builder
                .WithRsaPrivateKey(PrivateKey)
                .WithHardwareIdentifier("ABC")
                .WithSerialNumber("CDE")
                .ExpiresOn(expireDate)
                .WithProperty("Prop1", "Value1")
                .WithProperty("Prop2", "Value2")
                .SignAndCreate();

            Assert.IsNotNull(license);
            Assert.AreEqual("ABC", license.HardwareIdentifier);
            Assert.AreEqual("CDE", license.SerialNumber);
            Assert.AreEqual(expireDate, license.ExpirationDate);
            Assert.AreEqual(DateTime.UtcNow.Date, license.IssueDate.Date);
            Assert.AreEqual(2, license.Properties.Count);
            Assert.AreEqual("Value1", license.Properties["Prop1"]);
            Assert.AreEqual("Value2", license.Properties["Prop2"]);
        }

        [TestMethod]
        public void TestBuilder_Without()
        {
            var license = Lic.Builder
                .WithRsaPrivateKey("BwIAAACkAABSU0EyAAQAAAEAAQA9D9kxlsh1N5KPmhOsJj/QM9iZVaho5OzEG0gyO8s0Giycx7ttegjugLE1NY7Gw5FPJvSqrlRiBp9iNCsD9/NUJIa65mwfTsShzoce+v5tRLJrd4osZZ3WFA/e4oSk9BgCJNUWIShj1HKD4Lk1YqGWtaMZnx/uNLe8QZ4FGYkKvOWDl4FaLViZBbGfLBxMoMpPGQVmSbJtlOoqjyQr0J9stuuJCs564uTzXqJU9/ytInlFYGEDOpYanlkio4x38Px5WAF4+EPvplW6IszdwsR+Sd6hkSqwu8IPzkZwU6PsyvPF2tLQgomB4LVh/6gJcDpNCXJLWXm4GG+YuHpiCFG+6fPFbd0vcDc5Y4ByAtUADFQ0q2kdI+8K5znNJBd9xeuTF9mJLFKbvENJ58F+DPCtWLEWf5tYZXicZUfTa/tnmzFQKx1lc3wHYh5DyQttkmvsN4bXrp0whYU1S8eiI22H5meA5C6CiJZKSXWEGAAA2bpSgRt5ltS1WIR+wVGZ5iFkwnrkQldBZxecWCpj5HQexyDPcpsJipSotRllkNP8K8TubP8YafQGntQjeozZ4mOz2+V3f7GuC5DX229fGIv54ULq7ftWS3sqLSYzf1DJbN//bQkZQCAP5s5UXlx3n6G68cmkTaSE4ZP2slleqF6CEAaMS99jh2deYslQu1Jp395XkYoqnkzWiAIuoIYIaUxMn5+HWdqS6gqyGVHdbnWvlncdcoest/0waxkeSC86QqZ77QFzLaKSfDnYpHZ2t1o=")
                .WithoutHardwareIdentifier()
                .WithoutSerialNumber()
                .WithoutExpiration()
                .SignAndCreate();

            Assert.IsNotNull(license);
            Assert.AreEqual(HardwareIdentifier.NoHardwareIdentifier, license.HardwareIdentifier);
            Assert.AreEqual(SerialNumber.NoSerialNumber, license.SerialNumber);
            Assert.AreEqual(DateTime.MaxValue, license.ExpirationDate);
            Assert.AreEqual(DateTime.UtcNow.Date, license.IssueDate.Date);
            Assert.AreEqual(0, license.Properties.Count);
        }

        [TestMethod]
        public void TestSerialize_Without()
        {
            var expectedLicensePlain = HardwareIdentifier.NoHardwareIdentifier + Environment.NewLine +
                                       SerialNumber.NoSerialNumber + Environment.NewLine +
                                       "08/25/2017 00:00:00" + Environment.NewLine +
                                       "12/31/9999 23:59:59" + Environment.NewLine +
                                       "WNy5DustX6XP+U8LpDD1oOsTFkDFipn7gwgHAoYG2lqw/I45jFb1+nUmDo0+BOz5p87dl/KDcLlxk0AVAS82QSQtbapx0LPEG+KoVt7sdlssNOsBN8MaT0jVpCOhQFEpb8wJeakAuiTO/hH2COlVOAuSgEgxv0/k3PmCyceCry8=";

            var expectedLicenseEncrypted = "bmJTY2BTZHpNeWReaWkBIW9Of35JeWhAbHJCZCwLEBUjGRQuEh09HAExEBc8GxsxECAGGhMuExwjEhg4GQ0+GBs0GRc5EiwLd2N1HmV0U1lUHXlRC3g0Z1FFZBxjZFJVZkZIbUhxThprXEZJYUJVbBNtUVojYhU0SmtuGgpvdUBIRBEqYmJ2HlE5F0lgBGpFQ2FgU0oxYXtNeBkzcX5dX0NgUFU8Z3FEZwZHRHd1F15oR1JybmJ/aW85bUxYG0tXUG5DQ3BHZV1uE1ZLRUxnalRodGIjQ2kzY2JgfW5AVX5rbkZ5Vh0jQBJRTW51SERCUlQ0Fg==";

            AssertSerializationWorks(expectedLicensePlain, expectedLicenseEncrypted, expectedLicensePlain);
            AssertSerializationWorks(expectedLicensePlain, expectedLicenseEncrypted, expectedLicenseEncrypted);
        }

        private static void AssertSerializationWorks(string expectedLicensePlain, string expectedLicenseEncrypted, string licenseText)
        {
            var license = Lic.Verifier
                .WithRsaPublicKey(PublicKey)
                .WithoutApplicationCode()
                .LoadAndVerify(licenseText);

            var plainText = license.SerializeAsPlainText();
            var encrypted = license.Serialize();

            Assert.AreEqual(expectedLicensePlain, plainText);
            Assert.AreEqual(expectedLicenseEncrypted, encrypted);
        }
    }
}
