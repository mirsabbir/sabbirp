using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Live_Menu_Point_Of_Sale
{
    public class DataSaver
    {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject)
        {
            var path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DataSaver)).Location);

            if (serializableObject == null) { return; }

            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(serializableObject);
                writer = new StreamWriter(path + "\\data.json");
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>()
        {
            var path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DataSaver)).Location);

            TextReader reader = null;
            try
            {
                reader = new StreamReader(path + "\\data.json");
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            catch(Exception ex)
            {
                return default(T);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
