using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
namespace Codinsa2015.Tools
{
    public class Serializer
    {
        /// <summary>
        /// Serializes an object to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="output"></param>
        public static string Serialize<T>(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                serializer.Serialize(stream, obj);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        /// <summary>
        /// Deserializes an object from a file
        /// </summary>
        /// <param name="filename">Filename of the object to deserialize</param>
        /// <param name="create">If set to true : create the file if it does not exist.</param>
        public static T Deserialize<T>(string file) where T : new()
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(T));

            T Object;

            Object = (T)Serializer.Deserialize(new StringReader(file));
           
            return Object;
        }

        /// <summary>
        /// Deserializes an object from a file.
        /// Same as Deserialize but with no new() constraint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T DeserializeNoConstraints<T>(string filename)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(T));
            FileStream Stream = File.Open(filename, FileMode.Open);
            T Object;
            Object = (T)Serializer.Deserialize(Stream);
            Stream.Close();
            return Object;
        }
    }
}
