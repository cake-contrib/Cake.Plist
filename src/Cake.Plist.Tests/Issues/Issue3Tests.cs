using Cake.Core;
using Cake.Testing;
using NSubstitute;

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
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("Data/Issue3_Info.plist").SetContent(Resources.Issues3_Info_plist.NormalizeLineEndings());
            var context = Substitute.For<ICakeContext>();
            context.FileSystem.Returns(fileSystem);
            context.Environment.Returns(environment);

            var plist = context.DeserializePlist("./Data/Issue3_Info.plist");

            // Assert
            Assert.NotNull(plist);
        }
    }
}