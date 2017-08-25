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
