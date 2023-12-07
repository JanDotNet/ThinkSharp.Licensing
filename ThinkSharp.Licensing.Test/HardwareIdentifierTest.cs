// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
{
    [TestClass]
    public class HardwareIdentifierTest
    {
        [TestMethod]
        public void TestArePartialEqual_OneTrue() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-123", "ABC-345"));
        [TestMethod]
        public void TestArePartialEqual_OneFalse() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-123", "DDD-345"));

        [TestMethod]
        public void TestArePartialEqual_TwoTrue1() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-123", "ABC-DDD-345"));
        [TestMethod]
        public void TestArePartialEqual_TwoTrue2() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-123", "ABC-DEF-123"));
        [TestMethod]
        public void TestArePartialEqual_TwoFalse() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-123", "CCC-DDD-345"));

        [TestMethod]
        public void TestArePartialEqual_ThreeTrue1() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-123", "ABC-DEF-GHI-123"));
        [TestMethod]
        public void TestArePartialEqual_ThreeTrue2() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-123", "ABC-DEF-GGG-345"));
        [TestMethod]
        public void TestArePartialEqual_ThreeFalse1() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-123", "ABC-DDD-GGG-345"));
        [TestMethod]
        public void TestArePartialEqual_ThreeFalse2() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-123", "AAA-DDD-GGG-345"));

        [TestMethod]
        public void TestArePartialEqual_FourTrue1() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-123", "ABC-DEF-GHI-JKL-123"));
        [TestMethod]
        public void TestArePartialEqual_FourTrue2() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-123", "ABC-DEF-GGG-JJJ-345"));
        [TestMethod]
        public void TestArePartialEqual_FourFalse1() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-123", "ABC-DDD-GGG-JJJ-345"));
        [TestMethod]
        public void TestArePartialEqual_FourFalse2() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-123", "AAA-DDD-GGG-JJJ-345"));

        [TestMethod]
        public void TestArePartialEqual_FiveTrue1() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-MNO-123", "ABC-DEF-GHI-JKL-MNO-123"));
        [TestMethod]
        public void TestArePartialEqual_FiveTrue2() => Assert.IsTrue(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-MNO-123", "ABC-DEF-GHI-JJJ-MMM-123"));
        [TestMethod]
        public void TestArePartialEqual_FiveFalse1() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-MNO-123", "ABC-DEF-GGG-JJJ-MMM-123"));
        [TestMethod]
        public void TestArePartialEqual_FiveFalse2() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-MNO-123", "AAA-DDD-GGG-JJJ-MMM-123"));


        [TestMethod]
        public void TestArePartialEqual_DifferentLength() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("ABC-DEF-GHI-JKL-123", "ABC-DEF-GHI-JKL"));

        [TestMethod]
        public void TestArePartialEqual_Empty() => Assert.IsFalse(HardwareIdentifier.ArePartialEqual("", ""));
    }
}
