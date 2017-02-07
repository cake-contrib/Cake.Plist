# Build Script

To use the Plist in your cake file simply import it and define a task. In the following example we are updating the Info.plist of our iOS project.

```cake
#addin "Cake.Plist"

Task("update-ios-version")
    .Does(() =>
    {
        var plist = File("./src/Demo/Info.plist");
        dynamic data = DeserializePlist(plist);

        data["CFBundleShortVersionString"] = version.AssemblySemVer;
        data["CFBundleVersion"] = version.FullSemVer;

        SerializePlist(plist, data);
    });
```

:::{.alert .alert-info}
**IMPORTANT** You have to define the data variable explicied as `dynamic`. Otherwise Roslyn implies `object` which will follow in build error.
:::