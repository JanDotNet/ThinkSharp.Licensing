using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.Common.Test;

namespace ThinkSharp.Licensing
{
    [TestClass]
    public class HardwareIdentifierDotNetFullTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_hardware_identifier_structure()
        {
            var hardwareID = HardwareIdentifier.ForCurrentComputer();
        }
    }
}
