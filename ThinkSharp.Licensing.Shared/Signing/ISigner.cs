// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.Licensing
{
    public interface ISigner
    {
        string Sign(string content);
        bool Verify(string content, string signature);
    }
}
