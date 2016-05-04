namespace Cake.Plist
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    internal class PlistConvert
    {
        /// <summary>
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static dynamic Deserialize(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "plist":
                    return Deserialize(element.Elements().First());
                case "string":
                    return element.Value;
                case "real":
                    return double.Parse(element.Value, CultureInfo.InvariantCulture);
                case "integer":
                    return int.Parse(element.Value, CultureInfo.InvariantCulture);
                case "true":
                    return true;
                case "false":
                    return false;
                case "date":
                    return DateTime.Parse(element.Value, CultureInfo.InvariantCulture);
                case "data":
                    return Convert.FromBase64String(element.Value);
                case "array":
                {
                    if (!element.HasElements)
                        return new object[0];

                    var rawArray = element.Elements().Select(Deserialize).ToArray();

                    var type = rawArray[0].GetType();
                    if (rawArray.Any(val => val.GetType() != type))
                        return rawArray;

                    var typedArray = Array.CreateInstance(type, rawArray.Length);
                    rawArray.CopyTo(typedArray, 0);

                    return typedArray;
                }
                case "dict":
                {
                    var dictionary = new Dictionary<string, object>();

                    var inner = element.Elements().ToArray();
                    for (var idx = 0; idx < inner.Length; idx++)
                    {
                        var key = inner[idx];
                        if (key.Name.LocalName != "key")
                            throw new Exception("Even items need to be keys");

                        idx++;
                        dictionary[key.Value] = Deserialize(inner[idx]);
                    }

                    return dictionary;
                }
                default:
                    return null;
            }
        }

        public static XDocument SerializeDocument(object item)
        {
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
            doc.AddFirst(new XDocumentType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null));

            var plist = new XElement("plist");
            plist.SetAttributeValue("version", "1.0");
            plist.Add(Serialize(item));
            doc.Add(plist);

            return doc;
        }

        public static XElement Serialize(object item)
        {
            if (item is string)
            {
                return new XElement("string", item);
            }

            if (item is double || item is float || item is decimal)
            {
                return new XElement("real", Convert.ToString(item, CultureInfo.InvariantCulture));
            }

            if (item is int || item is long)
            {
                return new XElement("integer", Convert.ToString(item, CultureInfo.InvariantCulture));
            }

            if (item is bool && (item as bool?) == true)
            {
                return new XElement("true");
            }

            if (item is bool && (item as bool?) == false)
            {
                return new XElement("false");
            }

            if (item is DateTime)
            {
                return new XElement("date", ((DateTime) item).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }

            if (item is DateTimeOffset)
            {
                return new XElement("date", ((DateTimeOffset)item).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }

            var bytes = item as byte[];
            if (bytes != null)
            {
                return new XElement("data", Convert.ToBase64String(bytes));
            }

            var dictionary = item as IDictionary;
            if (dictionary != null)
            {
                var dict = new XElement("dict");

                var enumerator = dictionary.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    dict.Add(new XElement("key", enumerator.Key));
                    dict.Add(Serialize(enumerator.Value));
                }

                return dict;
            }

            var enumerable = item as IEnumerable;
            if (enumerable != null)
            {
                var array = new XElement("array");

                foreach (var itm in enumerable)
                {
                    array.Add(Serialize(itm));
                }

                return array;
            }

            return null;
        }
    }
}