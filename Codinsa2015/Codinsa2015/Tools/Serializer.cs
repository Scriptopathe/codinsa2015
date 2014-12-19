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
        public static void Serialize<T>(object obj, string output)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = null;
            try
            {
                stream = File.Open(output, FileMode.Create);
                serializer.Serialize(stream, obj);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

        }
        /// <summary>
        /// Deserializes an object from a file
        /// </summary>
        /// <param name="filename">Filename of the object to deserialize</param>
        /// <param name="create">If set to true : create the file if it does not exist.</param>
        public static T Deserialize<T>(string filename, bool create) where T : new()
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(T));

            FileStream Stream = null;
            T Object;
            if (!create)
            {
                Stream = File.Open(filename, FileMode.Open);
                try
                {
                    Object = (T)Serializer.Deserialize(Stream);
                }
                catch
                {
                    Object = default(T);
                }
                finally
                {
                    Stream.Close();
                }
            }
            else
            {
                try
                {
                    Stream = File.Open(filename, FileMode.Open);
                    Object = (T)Serializer.Deserialize(Stream);
                }
                catch
                {
                    Object = new T();
                    Serialize<T>(Object, filename);
                }
                finally
                {
                    // Creates a new file
                    if (Stream != null)
                        Stream.Close();
                }
            }
            return Object;
        }
        /// <summary>
        /// Deserializes an object from a file
        /// </summary>
        /// <param name="filename">Filename of the object to deserialize</param>
        public static T Deserialize<T>(string filename) where T : new()
        {
            return Deserialize<T>(filename, false);
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
