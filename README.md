# ThinkSharp.Licensing
Simple library with fluent API for creating and verifying signed licenses

[![Build status](https://ci.appveyor.com/api/projects/status/l3aagqmbfmgxwv3t?svg=true)](https://ci.appveyor.com/project/JanDotNet/thinksharp-licensing)

# Usage

## Hardware Identifier

    // Create:
    // e.g. "5BED5GAB-E5TGXKGK-01SI8MFF-7T099W78-SRH4"
    string hardwareIdentifier = HardwareIdentifier.ForCurrentComputer();
    
    // Validate Checksum
    if (!HardwareIdentifier.IsCheckSumValid(hardwareIdentifier))
        Console.WriteLine("Entered hardware identifier is not valid.");
        
    // Validate for current computer
    if (!HardwareIdentifier.IsValidForCurrentComputer(hardwareIdentifier))
        Console.WriteLine("Your license is not valid for this computer.");
    
## Create Serial Number

    // e.g "SNABC-D156-KYJF-C4M5-1H96"
    string serialNumber = SerialNumber.Create("ABC");
    
    // Validate CheckSum
    if (!SerialNumber.IsCheckSumValid(serialNumber))
        Console.WriteLine("Entered serial number is not valid.");
        
## Create key value Pair

    SigningKeyPair pair = Lic.KeyGenerator.GenerateRsaKeyPair();
	  Console.WriteLine(pair.PrivateKey);
	  Console.WriteLine(pair.PublicKey);
    
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
    // 5BED4PAB-ZATGXKGK-01SI8MFF-7T088W78-SRH4
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
