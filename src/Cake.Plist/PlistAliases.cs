namespace Cake.Plist
{
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using Core;
    using Core.Annotations;
    using Core.IO;

    /// <summary>
    ///     Contains functionality to work with plist files.
    /// </summary>
    [CakeAliasCategory("plist")]
    public static class PlistAliases
    {
        /// <summary>
        ///     Desierializes plist from xlm
        /// </summary>
        /// <param name="context"></param>
        /// <param name="xml">plist xml</param>
        /// <returns>deserialized plist as dynamic</returns>
        [CakeMethodAlias]
        public static dynamic DeserializePlist(this ICakeContext context, string xml)
        {
            return PlistConvert.Deserialize(XElement.Parse(xml));
        }


        /// <summary>
        ///     Deserializes a plist from file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file">xml plist file</param>
        /// <returns>deserialized plist as dynamic</returns>
        [CakeMethodAlias]
        public static dynamic DeserializePlist(this ICakeContext context, FilePath file)
        {
            using (var stream = File.OpenRead(file.FullPath))
            {
                var document = XDocument.Load(stream);

                return PlistConvert.Deserialize(document.Root);
            }
        }

        /// <summary>
        ///     Serializes a plist into spezified file.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file">target file</param>
        /// <param name="value">plist data</param>
        [CakeMethodAlias]
        public static void SerializePlist(this ICakeContext context, FilePath file, object value)
        {
            var doc = PlistConvert.SerializeDocument(value);

            string result;

            using (var sw = new MemoryStream())
            using (var strw = new StreamWriter(sw))
            {
                doc.Save(strw, SaveOptions.DisableFormatting);
                result = Encoding.UTF8.GetString(sw.ToArray());
            }

            File.WriteAllText(file.FullPath, result);
        }


        /// <summary>
        ///     Serializes plist as xml
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value">plist data</param>
        /// <returns>serialized plist xml</returns>
        [CakeMethodAlias]
        public static string SerializePlist(this ICakeContext context, object value)
        {
            return PlistConvert.Serialize(value).ToString();
        }
    }
}