namespace Cake.Plist.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.IO;
    using Xunit;

    public class PlistAliasTests
    {
        [Fact]
        public void Deserialize_missing_file_throws_exception()
        {
            var file = new FilePath("it-dont-exist.plist");

            Assert.Throws<FileNotFoundException>(() => PlistAliases.DeserializePlist(null, file));
        }

        [Fact]
        public void Read_write_compare()
        {
            // Arrange
            var expected = PlistAliases.DeserializePlist(null, new FilePath("Data/Info.plist"));

            // Act
            PlistAliases.SerializePlist(null, new FilePath("Info_COPY.plist"), expected);
            var dataCopy = PlistAliases.DeserializePlist(null, new FilePath("Info_COPY.plist"));

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