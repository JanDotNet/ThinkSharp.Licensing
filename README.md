# ThinkSharp.Licensing

[![Build status](https://ci.appveyor.com/api/projects/status/l3aagqmbfmgxwv3t?svg=true)](https://ci.appveyor.com/project/JanDotNet/thinksharp-licensing)
[![NuGet](https://img.shields.io/nuget/v/ThinkSharp.Licensing.svg)](https://www.nuget.org/packages/ThinkSharp.Licensing/) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.TXT)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)

## Introduction

**ThinkSharp.Licensing** is a simple library with fluent API for creating and verifying signed licenses. It provides the following functionallities:

* Creation / verification of hardware identifiers (Windows only)
* Creation / verification of serial numbers
* Creation / verification of signed licenses

## Installation

ThinkSharp.Licensing can be installed via [Nuget](https://www.nuget.org/packages/Thinksharp.Licensing)

      Install-Package ThinkSharp.Licensing 

## API Reference

### Singed License

A `SignedLicense` is class that encapsulates some license related information and a signature for verifying it. The license can be serialized / deserialized for storing it on the client. It has following public properties:

* **IssueDate:** date of the issuing (when the license was created)
* **ExpirationDate:** date of the expiration (may be `DateTime.MaxValue` for licenses without expiration)
* **SerialNumber** Optional: a serial number (See class `SerialNumber` below)
* **Properties** `IDictionary<string, string` with custom key value pairs

The static `Lic` class is the entry point for the fluent API that allows to work with signed licenses. It has the following static properties:

* **Lic.Builder:** Object for creating signed license objects
* **Lic.Verifyer:** Object for verifiying serialized signed licenses and deserialize it.
* **Lic.KeyGenerator:** Object for creating private/public key pairs to use for signing

#### Usage

**Create signed licenses**

    SignedLicense license = Lic.Builder
        .WithRsaPrivateKey(pk)                                           // .WithSigner(ISigner)
        .WithHardwareIdentifier(HardwareIdentifier.ForCurrentComputer()) // .WithoutHardwareIdentifier()
        .WithSerialNumber(SerialNumber.Create("ABC"))                    // .WithoutSerialNumber()
        .WithoutExpiration()                                             // .ExpiresIn(TimeSpan), .ExpiresOn(DateTime)
        .WithProperty("Name", "Bill Gates")
        .WithProperty("Company", "Microsoft")
	//... other key value pairs
        .SignAndCreate();
	
**Serialize License**

The `SignedLicense` can be serialized as encrypted base64 encoded string (default):

    var encryptedText = license.Serialize();
    
    // FW9JbxVRYW8hcWBVZ3VHbGosEBxfYhlMZmshHHUxGBVbHBksc3
    // 9EHywLc2NNaWIsE39YaAxFbXo7BhhSYxwhZmBJYSAGGxkuEhUj
    // GREwFw08GxsxEBc8GywLER8jGBAuGRQ1EgEzExc5Ehs0GSAGZU
    // BsRRdOQk1tAGptX0RyLSdPRExxQUN1EWxoQ19jWE5nVCAGahJm
    // Ek8/GhFwSwY7ehk3Sm4+cRk4EFh4GVkydFh0U0NURAZUWBVnbW
    // 9eXQ5JTWtgElI4cHxaBFtEQ2ZBGlFiSmR5bWsuEHRfAENAYx8+
    // U09vQmM+Tg5SakFmcmxKFWM9YQ4yR2NVSVdidUwnE1BuS0BLeX
    // tbU0tifnNDQ25teVZjcXl2H2pQVnk7QEBTC19FXFRGeGs6T1FX
    // SUR0YmprFmknHRA5VBpOeUdYHQ==

or as plain text string:

    var plainText = license.SerializePlainText();
    
    // 5BED5GAB-E5TGXKGK-01SI8MFF-7T099W78-SRH4
    // SNABC-3RTC-DMW7-9SC1-MAHA
    // 08/28/2017 00:00:00
    // 12/31/9999 23:59:59
    // Name:Bill Gates
    // Company:Microsoft
    // A3g2b310qk+7Q86jC2Z890ut2x3TuxxbUd+Xs4fMBRv/HmFl9s
    // 9PQV/zEcKM1pcjIuFJ/0YS+bAC22xnnbN2e/SJljYMK5N1J/3g
    // NYbvcUa+8qokmGRZZsfnURBcCaRwbQTz4KQvT7kaR+rIwuGXF6
    // dpViixIKj6D+618t7BRfY=
	
**Verify License**

For deserializing the license, the `Lic.Verifier` has to be used. If the license can not be deserialized hor has no valid signature, an exception is thrown.

    SignedLicense license = Lic.Verifier
		    .WithRsaPublicKey(publicKey)       // .WithSigner(ISigner)
		    .WithApplicationCode("ABC")        // .WithoutApplicationCode
		    .LoadAndVerify(licenseText);
		    
**Create public/private key Pair**

A public and private key pair can be generated using the `Lic.KeyGenerator` object:

    SigningKeyPair pair = Lic.KeyGenerator.GenerateRsaKeyPair();
    Console.WriteLine(pair.PrivateKey);
    Console.WriteLine(pair.PublicKey);

### Hardware Identifier

The hardware identifier is an identifier that derives from 4 characteristics of the computer's hardware (processor ID, serial number of BIOS and so on). The identifier may look like:

    5BED5GAB-E5TGXKGK-01SI8MFF-7T099W78-SRH4

Each characteristic is encoded in one of first 4 parts (8 charachters). The hardware identifier will be accepted if at least 2 of the 4 characteristics are equal. That ensures, that the license doesn't become invalid if e.g. the processor of the computer changed. The last part (4 characters) is a check sum that can be used to detect errors in the the hardware identifier. 

#### Usage

    // Create:
    string hardwareIdentifier = HardwareIdentifier.ForCurrentComputer();
    
    // Validate Checksum
    if (!HardwareIdentifier.IsCheckSumValid(hardwareIdentifier))
        Console.WriteLine("Entered hardware identifier has errors.");
        
    // Validate for current computer
    if (!HardwareIdentifier.IsValidForCurrentComputer(hardwareIdentifier))
        Console.WriteLine("Entered license is not valid for this computer.");
    
### Serial Number


A serial number is an identifier with an alpha-numeric application code (3 character), some random characters and a check sum. It looks like SNXXX-YYYY-YYYY-YYYY-ZZZ where XXX is the application code, YYYY is the random part and ZZZ is the check sum. E.g.: 

    SNABC-D156-KYJF-C4M5-1H96    

#### Usage

    // ABC = application code
    string serialNumber = SerialNumber.Create("ABC");
    
    // Validate CheckSum
    if (!SerialNumber.IsCheckSumValid(serialNumber))
        Console.WriteLine("Entered serial number is not valid.");
          
## License

ThinkSharp.Licensing is released under [The MIT license (MIT)](LICENSE.TXT)
    
## Donation
If you like ThinkSharp.Licensing and use it in your project(s), feel free to give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)
