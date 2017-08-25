// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using ThinkSharp.Helper;

namespace ThinkSharp.LicenseSource
{
    internal class LicenseFile : ILicenseSource
    {
        private const string LICENSE_FILE = "LICENSE";

        private readonly FileInfo myLicenseFileCurrentUser;
        private readonly FileInfo myLicenseFileAllUsers;

        public LicenseFile(string company, string product)
        {
            company.IsValidFolderName();
            product.IsValidFolderName();

            var path = GetfilePath(company, product, Environment.SpecialFolder.CommonApplicationData);
            myLicenseFileAllUsers = new FileInfo(path);

            path = GetfilePath(company, product, Environment.SpecialFolder.ApplicationData);
            myLicenseFileCurrentUser = new FileInfo(path);
        }

        private static string GetfilePath(string company, string product, Environment.SpecialFolder specialFolder)
            => Path.Combine(Environment.GetFolderPath(specialFolder), company, product, LICENSE_FILE);

        public void Write(string content)
        {
            try
            {
                WriteAllText(myLicenseFileAllUsers, content);
            }
            catch
            {
                // may fail for security reasons -> ignore
            }

            WriteAllText(myLicenseFileCurrentUser, content);
        }

        public string Read()
        {
            // Try to use the current user's license file if exists
            var license = ReadAllText(myLicenseFileCurrentUser);
            if (license != null) return license;

            // otherwise try the 'all users' license file
            return ReadAllText(myLicenseFileAllUsers);
        }

        private string ReadAllText(FileInfo file)
        {
            if (!file.Exists)
                return null;

            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        private void WriteAllText(FileInfo file, string text)
        {
            // ensure that the directory exists.
            Directory.CreateDirectory(file.DirectoryName);

            using (var stream = file.OpenWrite())
            using (var writer = new StreamWriter(stream))
                writer.Write(text);
        }
    }
}
