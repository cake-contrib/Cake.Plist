# Cake.HockeyApp Addin 

![AppVeyor master branch](https://img.shields.io/appveyor/ci/reicheltp/cake-hockeyapp.svg)
![nuget pre release](https://img.shields.io/nuget/vpre/Cake.HockeyApp.svg)

This Addin for the Cake Build Automation System allows you to deploy your package to HockeyApp. More about Cake at http://cakebuild.net

## Use the addin

To use the HockeyApp in your cake file simply import it and define a publish task.
```cake
#addin "Cake.HockeyApp"

Task("deploy")
    .Does(() => 
    {
        UploadToHockeyApp( pathToYourPackageFile, new HockeyAppUploadSettings 
        {
            AppId = appIdFromHockeyApp,
            Version = "1.0.160901.1",
            ShortVersion = "1.0-beta2",
            Notes = "Uploaded via continuous integration."
        });
    });
```

The available parameters for the upload settings are descripted here: http://support.hockeyapp.net/kb/api/api-versions#upload-version

`AppId`, `ApiToken` and `Version` are **required parameters** you have to set. 

>   Do not checkin the HockeyApp API Token into your source control. Either use `HockeyAppUploadSettings.ApiToken` or the `HOCKEYAPP_API_TOKEN` environment variable.

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