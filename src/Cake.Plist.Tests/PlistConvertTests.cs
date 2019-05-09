using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Cake.Core;
using Xunit;

namespace Cake.Plist.Tests
{
    public sealed class PlistConvertTests
    {
        [Theory]
        [InlineData("<true />", true)]
        [InlineData("<false />", false)]
        [InlineData("<string>Hallo</string>", "Hallo")]
        [InlineData("<integer>5</integer>", 5)]
        [InlineData("<real>5.1234</real>", 5.1234)]
        public void CanDeserializePlistEntry(string value, object expected)
        {
            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert
            Assert.Equal(expected, item);
        }

        [Theory]
        [InlineData(true, "<true />")]
        [InlineData(false, "<false />")]
        [InlineData("Hallo", "<string>Hallo</string>")]
        [InlineData(5, "<integer>5</integer>")]
        [InlineData(5.1234, "<real>5.1234</real>")]
        public void CanSerializePlistEntry(object value, string expected)
        {
            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanDeserializeArray()
        {
            // Arrange
            const string value = "<array><string>Test1</string><string>Test2</string></array>";
            var expected = new[] {"Test1", "Test2"};

            // Act
            var element = XElement.Parse(value);
            var item = PlistConvert.Deserialize(element);

            // Assert
            Assert.Equal(expected, item);
        }

        [Fact]
        public void CanDeserializeDateTime()
        {
            // Arrange
            const string value = "<date>2012-09-27</date>";
            var expected = new DateTime(2012,09,27);

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
            var value = Resources.Info_plist.NormalizeLineEndings();

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

        [Fact]
        public void CanDeserializeData()
        {
            // Arrange
            string value;
            value = "<data>oqO0</data>";
            var expected = new byte[] {0xA2, 0xA3, 0xB4};

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
        public void CanDeserializeEmptyArray()
        {
            // Arrange
            const string value = "<array />";
            var expected = new object[0];

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
        public void CanSerializeDateTime()
        {
            // Arrange
            const string expected = "<date>2012-09-27T10:25:13.000Z</date>";
            var value = new DateTime(2012, 09, 27, 10,25,13);

            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanSerializeDateTimeOffset()
        {
            // Arrange
            const string expected = "<date>2012-09-27T10:25:13.000Z</date>";
            var value = new DateTimeOffset(2012, 09, 27, 12, 25, 13, TimeSpan.FromHours(2));

            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanSerializeArray()
        {
            // Arrange
            var value = new[] {"Test1", "Test2"};
            const string expected = "<array><string>Test1</string><string>Test2</string></array>";

            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void CanSerializeComplexPlist()
        {
            // Arrange
            var fileContent = Resources.SerializeResult_plist.NormalizeLineEndings();
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
        public void CanSerializeData()
        {
            // Arrange
            var value = new byte[] {0xA2, 0xA3, 0xB4};
            const string expected = "<data>oqO0</data>";

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
        public void CanSerializeEmptyArray()
        {
            // Arrange
            var value = new object[0];
            const string expected = "<array />";

            // Act
            var item = PlistConvert.Serialize(value);

            // Assert
            Assert.Equal(expected, item.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void SerializerAddDocHeader()
        {
            var doc = PlistConvert.SerializeDocument(new Dictionary<string, object> {{"k1", 1}});

            string result;

            using (var sw = new MemoryStream())
            using (var strw = new StreamWriter(sw))
            {
                doc.Save(strw, SaveOptions.DisableFormatting);
                result = Encoding.UTF8.GetString(sw.ToArray());
            }


            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">",
                result);
        }
    }
}