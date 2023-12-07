// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Management;
using System.Runtime.Versioning;
using System.Text;

namespace ThinkSharp.Licensing
{
#if NET6_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    internal class WindowsComputerCharacteristics : IComputerCharacteristics
    {
        public IEnumerable<string> GetCharacteristicsForCurrentComputer()
        {
            yield return GetValue("ProcessorID", "Win32_Processor");
            yield return GetValue("SerialNumber", "Win32_BIOS");
            yield return GetValue("SerialNumber", "Win32_BaseBoard");
            yield return GetValue("SerialNumber", "Win32_PhysicalMedia");
        }

        private static string GetValue(string property, string type)
        {
            var sb = new StringBuilder();
            try
            {
                var searcher = new ManagementObjectSearcher($"select {property} from {type}");
                foreach (var share in searcher.Get())
                    foreach (PropertyData pc in share.Properties)
                        sb.Append(pc.Value);
            }
            catch
            {
                sb.Append(property).Append(type);
            }
            return sb.ToString();
        }
    }
}
