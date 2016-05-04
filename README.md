# Cake.Plist Addin 

[//]: # (![AppVeyor master branch](https://img.shields.io/appveyor/ci/reicheltp/cake-Plist.svg))
![nuget pre release](https://img.shields.io/nuget/vpre/Cake.Plist.svg)

This Addin for the Cake Build Automation System allows you to serialize and deserialize xml plists. More about Cake at http://cakebuild.net

## Use the addin

To use the Plist in your cake file simply import it and define a task. In the following example we are updating the Info.plist of our iOS project.
```cake
#addin "Cake.Plist"

Task("update-ios-version")
    .Does(() => 
    {
        var plist = File("./src/Demo/Info.plist");
        var data = DeserializePlist(plist);
        
        data["CFBundleShortVersionString"] = version.AssemblySemVer;
        data["CFBundleVersion"] = version.FullSemVer;
        
        SerializePlist(plist, data);
    });
```

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build restore
./build
```

On OSX/Linux run:
```bash
./build.sh restore
./build.sh
```

Run `pack` alias to create a nuget package.