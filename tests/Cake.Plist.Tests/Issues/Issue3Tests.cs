namespace Cake.Plist.Tests.Issues
{
    using Core.IO;
    using Xunit;

    public class Issue3Tests
    {
        [Fact]
        public void CanDeserializePlist()
        {
            // Arrange
            var plist = PlistAliases.DeserializePlist(null, new FilePath("Data/Issue3_Info.plist"));

            // Assert
            Assert.NotNull(plist);
        }
    }
}