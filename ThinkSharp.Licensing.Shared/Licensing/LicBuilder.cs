using System;
using System.Collections.Generic;
using System.Text;

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
        IBuilder_HardwareIdentifier WithSigner(ISigner signer);
    }

    public interface IBuilder_HardwareIdentifier
    {
        IBuilder_SerialNumber WithHardwareIdentifier(string hardwareKey);
        IBuilder_SerialNumber WithoutHardwareIdentifier();
    }

    public interface IBuilder_SerialNumber
    {
        IBuilder_Expiration WithSerialNumber(string serialNumber);
        IBuilder_Expiration WithoutSerialNumber();
    }

    public interface IBuilder_Expiration
    {
        IBuilder_Properties ExpiresOn(DateTime dateTime);
        IBuilder_Properties ExpiresIn(TimeSpan timeSpan);
        IBuilder_Properties WithoutExpiration();
    }

    public interface IBuilder_Properties
    {
        IBuilder_Properties WithProperty(string key, string value);
        SignedLicense SignAndCreate();
    }
}
