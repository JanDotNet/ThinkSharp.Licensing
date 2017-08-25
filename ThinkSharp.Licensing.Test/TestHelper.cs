using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Common.Test
{
    public static class TestHelper
    {
        public static void AssertException<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
                Assert.Fail("Expected exceptin of type: " + typeof(TException).Name);
            }
            catch (TException) { }
        }
    }
}
