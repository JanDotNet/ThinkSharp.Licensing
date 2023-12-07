// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using ThinkSharp.Licensing.Signing;

namespace ThinkSharp.Licensing
{
    internal class LicBuilder : IBuilder_Signer, IBuilder_HardwareIdentifier, IBuilder_SerialNumber, IBuilder_Expiration, IBuilder_Properties
    {
        private ISigner mySigner;
        private string myHardwareIdentifier = HardwareIdentifier.NoHardwareIdentifier;
        private string mySerialNumber = SerialNumber.NoSerialNumber;
        private Dictionary<string, string> myProperties = new Dictionary<string, string>();
        private DateTime myExpirationDate = DateTime.MaxValue;

        #region IBuilder_Signer

        IBuilder_HardwareIdentifier IBuilder_Signer.WithSigner(ISigner signer)
        {
            mySigner = signer ?? throw new ArgumentNullException(nameof(signer));
            return this as IBuilder_HardwareIdentifier;
        }

        #endregion

        #region IBuilder_HardwareIdentifier

        IBuilder_SerialNumber IBuilder_HardwareIdentifier.WithHardwareIdentifier(string hardwareIdentifier)
        {
            myHardwareIdentifier = hardwareIdentifier ?? string.Empty;
            return this as IBuilder_SerialNumber;
        }

        IBuilder_SerialNumber IBuilder_HardwareIdentifier.WithoutHardwareIdentifier()
        {
            return this as IBuilder_SerialNumber;
        }

        #endregion

        #region IBuilder_SerialNumber

        IBuilder_Expiration IBuilder_SerialNumber.WithSerialNumber(string serialNumber)
        {
            mySerialNumber = serialNumber;
            return this as IBuilder_Expiration;
        }

        IBuilder_Expiration IBuilder_SerialNumber.WithoutSerialNumber()
        {
            return this as IBuilder_Expiration;
        }

        #endregion

        #region IBuilder_Expiration

        IBuilder_Properties IBuilder_Expiration.ExpiresOn(DateTime dateTime)
        {
            myExpirationDate = dateTime;
            return this as IBuilder_Properties;
        }

        IBuilder_Properties IBuilder_Expiration.ExpiresIn(TimeSpan timeSpan)
        {
            myExpirationDate = DateTime.Now + timeSpan;
            return this as IBuilder_Properties;
        }

        IBuilder_Properties IBuilder_Expiration.WithoutExpiration()
        {
            return this as IBuilder_Properties;
        }

        #endregion

        #region IBuilder_Properties

        IBuilder_Properties IBuilder_Properties.WithProperty(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (key.Contains(":"))
                throw new ArgumentException("Character ':' is not allowed in property key.");
            myProperties.Add(key, value);
            return this as IBuilder_Properties;
        }

        SignedLicense IBuilder_Properties.SignAndCreate()
        {
            var license = new SignedLicense(myHardwareIdentifier, mySerialNumber, DateTime.Now, myExpirationDate, myProperties);
            license.Sign(mySigner);
            return license;
        }

        #endregion
    }

    public interface IBuilder_Signer
    {
        /// <summary>
        /// Use the specified <see cref="ISigner"/> implementation for signing.
        /// </summary>
        /// <param name="signer">
        /// The <see cref="ISigner"/> implementation to use for signing.
        /// </param>
        /// <returns></returns>
        IBuilder_HardwareIdentifier WithSigner(ISigner signer);
    }

    public interface IBuilder_HardwareIdentifier
    {
        /// <summary>
        /// Create the license with the specified hardware identifier.
        /// See also <see cref="HardwareIdentifier"/>.
        /// </summary>
        /// <param name="hardwareIdentifier">
        /// The hardware identifier to use.
        /// </param>
        /// <returns></returns>
        IBuilder_SerialNumber WithHardwareIdentifier(string hardwareIdentifier);
        /// <summary>
        /// Create the license without hardware identifier.
        /// </summary>
        /// <returns></returns>
        IBuilder_SerialNumber WithoutHardwareIdentifier();
    }

    public interface IBuilder_SerialNumber
    {
        /// <summary>
        /// Create the license with the specified serial number.
        /// See also <see cref="SerialNumber"/>.
        /// </summary>
        /// <param name="serialNumber">
        /// The serial number to use.
        /// </param>
        /// <returns></returns>
        IBuilder_Expiration WithSerialNumber(string serialNumber);
        /// <summary>
        /// Create the license without serial number.
        /// </summary>
        /// <returns></returns>
        IBuilder_Expiration WithoutSerialNumber();
    }

    public interface IBuilder_Expiration
    {
        /// <summary>
        /// The license expires on the specified date time.
        /// </summary>
        /// <param name="dateTime">
        /// The date when the license expires.
        /// </param>
        /// <returns></returns>
        IBuilder_Properties ExpiresOn(DateTime dateTime);
        /// <summary>
        /// The license expires after the specified time span.
        /// </summary>
        /// <param name="timeSpan">
        /// The period after which the license expires.
        /// </param>
        /// <returns></returns>
        IBuilder_Properties ExpiresIn(TimeSpan timeSpan);
        /// <summary>
        /// The license does not expire.
        /// </summary>
        /// <returns></returns>
        IBuilder_Properties WithoutExpiration();
    }

    public interface IBuilder_Properties
    {
        /// <summary>
        /// Adds the key value pair to the license information.
        /// </summary>
        /// <param name="key">
        /// The key to add. NOTE: The key must not contain ':'.
        /// </param>
        /// <param name="value">
        /// The value to add.
        /// </param>
        /// <returns></returns>
        IBuilder_Properties WithProperty(string key, string value);

        /// <summary>
        /// Signs the license data and creates the <see cref="SignedLicense"/> object.
        /// </summary>
        /// <returns>
        /// The signed <see cref="SignedLicense"/> object.
        /// </returns>
        SignedLicense SignAndCreate();
    }
}
