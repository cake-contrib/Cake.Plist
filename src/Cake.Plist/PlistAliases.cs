namespace Cake.Plist
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using Core;
    using Core.IO;

    /// <summary>
    ///     Contains functionality to work with plist files.
    /// </summary>
    public static class PlistAliases
    {
        public static dynamic Parse(this ICakeContext context, FilePath file)
        {
            using (var stream = File.OpenRead(file.FullPath))
            {
                var reader = XmlReader.Create(stream);
                var document = XNode.ReadFrom(reader) as XDocument;

                return PlistConvert.Deserialize(document.Root);
            }
        }
    }
}