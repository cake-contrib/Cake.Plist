namespace Cake.Plist.Tests.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Xunit;

    public sealed class PlistConvertFixtures
    {

        [Theory]
        [InlineData("<true />", true)]
        [InlineData("<false />", false)]
        [InlineData("<string>Hallo</string>", "Hallo")]
        [InlineData("<integer>5</integer>", 5)]
        [InlineData("<real>5.1234</real>", 5.1234)]
        [InlineData("<array />", new object[0])]
        [InlineData("<data>oqO0</data>", new byte[] { 0xA2, 0xA3, 0xB4 })]
        [InlineData("<array><string>Test1</string><string>Test2</string></array>", new [] {"Test1", "Test2"})]

        public void CanDeserializePlistEntry(string value, object expected)
        {
            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert
            Assert.Equal(expected, item);
        }

        [Fact]
        public void CanDeserializePlistDateTime()
        {
            // Arrange
            var value = "<date>2016-05-03T11:40:00+00:00</date>";
            var expected = new DateTimeOffset(2016, 05, 03, 11, 40, 00, TimeSpan.Zero);

            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert
            Assert.Equal(expected, item);
        }

        [Fact]
        public void CanDeserializeDict()
        {
            // Arrange
            var value = "<dict><key>k1</key><string>v1</string><key>k2</key><integer>3</integer></dict>";

            var expected = new Dictionary<string, object>
            {
                {"k1", "v1"},
                {"k2", 3}
            };

            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert
            Assert.Equal(expected, item);
        }

        [Fact]
        public void CanDeserializeComplexPlist()
        {
            // Arrange
            var value = File.ReadAllText("Data/Info.plist");

            var expected = new Dictionary<string, object>
            {
                {"CFBundleShortVersionString", "1.1.3"},
                {"CFBundleVersion", "1.1.3+Branch.master.Sha.41e4b956b644d0a30e8aa28a6cb6509d99e81031"},
                {"LSRequiresIPhoneOS", true},
                {"MinimumOSVersion", "8.0"},
                {"UIDeviceFamily", new[] {1, 2}},
                {"UILaunchStoryboardName", "LaunchScreen"},
                {"UIRequiredDeviceCapabilities", new[] {"armv7"}},
                {
                    "UISupportedInterfaceOrientations",
                    new[] {"UIInterfaceOrientationPortrait", "UIInterfaceOrientationPortraitUpsideDown"}
                },
                {
                    "UISupportedInterfaceOrientations~ipad",
                    new[] {"UIInterfaceOrientationPortrait", "UIInterfaceOrientationPortraitUpsideDown"}
                },
                {"CFBundleName", "Demo App"},
                {"CFBundleIdentifier", "com.democom.demo"},
                {"CFBundleDisplayName", "Demo"},
                {"NSLocationAlwaysUsageDescription", "This app needs your current location."},
                {"UIBackgroundModes", new[] {"location"}},
                {
                    "CFBundleIconFiles", new[]
                    {
                        "Icon-72@2x.png",
                        "Icon-72.png",
                        "Icon-60@2x.png",
                        "Icon-76.png",
                        "Icon-76@2x.png",
                        "Icon-Small-40.png",
                        "Icon-Small-40@2x.png"
                    }
                },
                {"UIStatusBarHidden", true},
                {"UIStatusBarHidden~ipad", true}
            };

            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert

            var a1 = expected.ToArray();
            var a2 = ((Dictionary<string, object>) item).ToArray();

            // Assert.Equal is not working correct on dictionary. Therefore we iterate 
            for (var i = 0; i < a1.Length; i++)
            {
                Assert.Equal(a1[i].Key, a2[i].Key);

                Assert.Equal(a1[i].Value, a2[i].Value);
            }
        }

        [Theory]
        [InlineData(true, "<true />")]
        [InlineData(false, "<false />")]
        [InlineData("Hallo", "<string>Hallo</string>")]
        [InlineData(5, "<integer>5</integer>")]
        [InlineData(5.1234, "<real>5.1234</real>")]
        [InlineData(new object[0], "<array />")]
        [InlineData(new byte[] { 0xA2, 0xA3, 0xB4 }, "<data>oqO0</data>")]
        [InlineData(new[] { "Test1", "Test2" }, "<array><string>Test1</string><string>Test2</string></array>")]

        public void CanSerializePlistEntry(object value, string expected)
        {
            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanSerializeDict()
        {
            // Arrange
            var expected = "<dict><key>k1</key><string>v1</string><key>k2</key><integer>3</integer></dict>";

            var value = new Dictionary<string, object>
            {
                {"k1", "v1"},
                {"k2", 3}
            };

            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanSerializeComplexPlist()
        {
            // Arrange
            var fileContent = File.ReadAllText("Data/SerializeResult.plist");
            var expected = XElement.Parse(fileContent).ToString(SaveOptions.DisableFormatting);

            var value = new Dictionary<string, object>
            {
                {"CFBundleShortVersionString", "1.1.3"},
                {"CFBundleVersion", "1.1.3+Branch.master.Sha.41e4b956b644d0a30e8aa28a6cb6509d99e81031"},
                {"LSRequiresIPhoneOS", true},
                {"MinimumOSVersion", "8.0"},
                {"UIDeviceFamily", new[] {1, 2}},
                {"UILaunchStoryboardName", "LaunchScreen"},
                {"UIRequiredDeviceCapabilities", new[] {"armv7"}},
                {
                    "UISupportedInterfaceOrientations",
                    new[] {"UIInterfaceOrientationPortrait", "UIInterfaceOrientationPortraitUpsideDown"}
                },
                {
                    "UISupportedInterfaceOrientations~ipad",
                    new[] {"UIInterfaceOrientationPortrait", "UIInterfaceOrientationPortraitUpsideDown"}
                },
                {"CFBundleName", "Demo App"},
                {"CFBundleIdentifier", "com.democom.demo"},
                {"CFBundleDisplayName", "Demo"},
                {"NSLocationAlwaysUsageDescription", "This app needs your current location."},
                {"UIBackgroundModes", new[] {"location"}},
                {
                    "CFBundleIconFiles", new[]
                    {
                        "Icon-72@2x.png",
                        "Icon-72.png",
                        "Icon-60@2x.png",
                        "Icon-76.png",
                        "Icon-76@2x.png",
                        "Icon-Small-40.png",
                        "Icon-Small-40@2x.png"
                    }
                },
                {"UIStatusBarHidden", true},
                {"UIStatusBarHidden~ipad", true}
            };

            // Act
            var doc = PlistConvert.SerializeDocument(value);
            doc.FirstNode.Remove();

            // Assert
            Assert.Equal(expected, doc.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void SerializerAddDocHeader()
        {
            var doc = PlistConvert.SerializeDocument(new Dictionary<string, object> { {"k1", 1} });

            string result;

            using (var sw = new MemoryStream())
            using (var strw = new StreamWriter(sw))
            {
                doc.Save(strw, SaveOptions.DisableFormatting);
                result = Encoding.UTF8.GetString(sw.ToArray());
            }


            Assert.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">", result);
        }
    }
}

