// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace ThinkSharp.Licensing.Helper
{
    public static class StringHelper
    {
        public static string Wrap(this string singleLineString, int columns)
            => string.Join(Environment.NewLine, singleLineString.Split(columns));

        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (chunkSize < 1)
                throw new ArgumentException("'chunkSize' must be greater than 0.");

            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
        }

        public static string Unwrap(this string stringWithLineBreaks)
        {
            if (stringWithLineBreaks == null)
                throw new ArgumentNullException(nameof(stringWithLineBreaks));
            return stringWithLineBreaks.Replace(Environment.NewLine, "");
        }
    }
}
