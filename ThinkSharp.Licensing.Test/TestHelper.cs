// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkSharp.Licensing.Test
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
