using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Plist.Tests
{
    public class PlistAliasTests
    {
        [WindowsFact]
        public void Deserialize_missing_file_throws_exception()
        {
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var context = Substitute.For<ICakeContext>();
            context.FileSystem.Returns(fileSystem);
            context.Environment.Returns(environment);

            Assert.Throws<FileNotFoundException>(() => PlistAliases.DeserializePlist(context, "./file-doesnt-exist"));
        }

        [WindowsFact]
        public void Read_write_compare()
        {
            // Arrange
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("Data/Info.plist").SetContent(Resources.Info_plist.NormalizeLineEndings());
            fileSystem.CreateFile("Info_COPY.plist");
            var context = Substitute.For<ICakeContext>();
            context.FileSystem.Returns(fileSystem);
            context.Environment.Returns(environment);
            var expected = context.DeserializePlist("./Data/Info.plist");

            // Act
            PlistAliases.SerializePlist(context, "./Info_COPY.plist", expected);
            var dataCopy = context.DeserializePlist("./Info_COPY.plist");

            // Assert
            var a1 = ((Dictionary<string, object>) expected).ToArray();
            var a2 = ((Dictionary<string, object>) dataCopy).ToArray();

            // Assert.Equal is not working correct on dictionary. Therefore we iterate
            for (var i = 0; i < a1.Length; i++)
            {
                Assert.Equal(a1[i].Key, a2[i].Key);

                Assert.Equal(a1[i].Value, a2[i].Value);
            }
        }
    }
}