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
            Assert.AreEqual(DateTime.UtcNow.Date, license.IssueDate);
            Assert.AreEqual(2, license.Properties.Count);
            Assert.AreEqual("Value1", license.Properties["Prop1"]);
            Assert.AreEqual("Value2", license.Properties["Prop2"]);

            var expectedLicensePlain =   "ABC" + Environment.NewLine +
                                        "CDE" + Environment.NewLine +
                                        "08/25/2017 00:00:00" + Environment.NewLine +
                                        "01/01/2100 00:00:00" + Environment.NewLine +
                                        "Prop1:Value1" + Environment.NewLine +
                                        "Prop2:Value2" + Environment.NewLine +
                                        "aUrG4WI6BNeJareyXD3TLjF2jiDE3O/77X8A9dtu3KBgQxAMUm/L5LYp9PY0gotXk2VZCTIr6Skrg/Kx4RxzHPBU9D3OE6poT112EPAgw/zFyAxdyZc42KN86QPVIZsOBF87sXa+YLqx+iou7JIU6NnIWJJNvKXxRUA3U1Q3YhU=";

            Assert.AreEqual(expectedLicensePlain, license.Serialize(true));

            var expectedLicenseEncrypted = "YW9PJitCZGgBIRE5Dx85BBMxERosGxE7EB02GxEMKh09BBEwDx89GxEhEB02GxE7EB0BIXFzT109EXdgTFhpGiwLcF9jWxM7dkxgXkQzLSdtflNGFHpFHWNPRWdtWUR4eGk/f21rZh9mQmVEE2IjHBZZGGw1T1V0E2ZOTHB5YWBZRg5NFWFVWxhReR1rRFVZSx9acWJVaV86eEpzRwJHUxVTWFdEe2NUGWk/ZGQ3UEJYGhAzZX1NTFYuWmt1alllWXdvHxNKbhU6enFXaXd/ZGNHGBp/c0AqeWF9UwpoT1g7YWhUFmNiYnZLamN6YHl5cnhNGHQwcR5VQ3Q8";
            Assert.AreEqual(expectedLicenseEncrypted, license.Serialize());
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
            Assert.AreEqual(DateTime.UtcNow.Date, license.IssueDate);
            Assert.AreEqual(0, license.Properties.Count);

            var expectedLicensePlain = "NO_HARDWARE_ID" + Environment.NewLine +
                                       "NO_SERIAL_NO" + Environment.NewLine +
                                       "08/25/2017 00:00:00" + Environment.NewLine +
                                       "12/31/9999 23:59:59" + Environment.NewLine +
                                       "WNy5DustX6XP+U8LpDD1oOsTFkDFipn7gwgHAoYG2lqw/I45jFb1+nUmDo0+BOz5p87dl/KDcLlxk0AVAS82QSQtbapx0LPEG+KoVt7sdlssNOsBN8MaT0jVpCOhQFEpb8wJeakAuiTO/hH2COlVOAuSgEgxv0/k3PmCyceCry8=";

            Assert.AreEqual(expectedLicensePlain, license.Serialize(true));

            var expectedLicenseEncrypted = "bmJTY2BTZHpNeWReaWkBIW9Of35JeWhAbHJCZCwLEBUjGRQuEh09HAExEBc8GxsxECAGGhMuExwjEhg4GQ0+GBs0GRc5EiwLd2N1HmV0U1lUHXlRC3g0Z1FFZBxjZFJVZkZIbUhxThprXEZJYUJVbBNtUVojYhU0SmtuGgpvdUBIRBEqYmJ2HlE5F0lgBGpFQ2FgU0oxYXtNeBkzcX5dX0NgUFU8Z3FEZwZHRHd1F15oR1JybmJ/aW85bUxYG0tXUG5DQ3BHZV1uE1ZLRUxnalRodGIjQ2kzY2JgfW5AVX5rbkZ5Vh0jQBJRTW51SERCUlQ0Fg==";
            Assert.AreEqual(expectedLicenseEncrypted, license.Serialize());
        }
    }
}
