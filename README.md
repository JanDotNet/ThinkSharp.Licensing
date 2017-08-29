# ThinkSharp.Licensing

[![Build status](https://ci.appveyor.com/api/projects/status/l3aagqmbfmgxwv3t?svg=true)](https://ci.appveyor.com/project/JanDotNet/thinksharp-licensing)
[![NuGet](https://img.shields.io/nuget/v/ThinkSharp.Licensing.svg)](https://www.nuget.org/packages/ThinkSharp.Licensing/) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.TXT)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/JanDotNet)

## Introduction

**ThinkSharp.Licensing** is a simple library with fluent API for creating and verifying signed licenses. It provides the following functionallities:

* Creation / verification of hardware identifiers (Windows only)
* Creation / verification of serial numbers
* Creation / verification of signing of licenses

## Hardware Identifier

### Description

The hardware identifier is an identifier that derives from 4 characteristics of the computer's hardware (processor ID, serial number of BIOS and so on). The identifier may look like:

    5BED5GAB-E5TGXKGK-01SI8MFF-7T099W78-SRH4

Each characteristic is encoded in one of the 8-character parts. The hardware identifier will be accepted if at least 2 of the 4 characteristics are equal. That ensures, that the license doesn't become invalid if e.g. the processor of the computer changed. The last 4-character part is a check sum that can be used to validate the hardware identifier. 

### Usage

    // Create:
    string hardwareIdentifier = HardwareIdentifier.ForCurrentComputer();
    
    // Validate Checksum
    if (!HardwareIdentifier.IsCheckSumValid(hardwareIdentifier))
        Console.WriteLine("Entered hardware identifier is not valid.");
        
    // Validate for current computer
    if (!HardwareIdentifier.IsValidForCurrentComputer(hardwareIdentifier))
        Console.WriteLine("Entered license is not valid for this computer.");
    
## Serial Number

### Description

A serial number is random identifier with an 3 character alpha-numeric application code and a check sum. It looks like SNXXX-YYYY-YYYY-YYYY-ZZZ where XXX is the application code, YYYY is the random part and ZZZ is the check sum. E.g.: 

    SNABC-D156-KYJF-C4M5-1H96    

### Usage

    // ABC = application code
    string serialNumber = SerialNumber.Create("ABC");
    
    // Validate CheckSum
    if (!SerialNumber.IsCheckSumValid(serialNumber))
        Console.WriteLine("Entered serial number is not valid.");
        
## Signed License

## Create signed license

    SignedLicense license = Lic.Builder
        .WithRsaPrivateKey(pk)
        .WithHardwareIdentifier(HardwareIdentifier.ForCurrentComputer())
        .WithSerialNumber(SerialNumber.Create("ABC"))
        .WithoutExpiration()
        .WithProperty("Name", "Bill Gates")
        .WithProperty("Company", "Microsoft")
        .SignAndCreate();

## Serialize license

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
    
    var plainText = license.Serialize(true);
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
    
## Verify License

    // throws exception if license is not valid
    SignedLicense license = Lic.Verifier
		    .WithRsaPublicKey(publicKey)
		    .WithApplicationCode("ABC")
		    .LoadAndVerify(licenseText);

## Create public/private key Pair

    SigningKeyPair pair = Lic.KeyGenerator.GenerateRsaKeyPair();
    Console.WriteLine(pair.PrivateKey);
    Console.WriteLine(pair.PublicKey);
