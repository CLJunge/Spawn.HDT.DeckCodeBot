#region Using
using System;
using System.IO;
using System.Xml.Serialization;
#endregion

namespace Spawn.HDT.DeckCodeBot
{
    public static class FileHelper
    {
        #region Write
        public static void Write<T>(string strPath, T value) where T : class, new()
        {
            try
            {
                if (File.Exists(strPath))
                    File.Delete(strPath);

                using (StreamWriter writer = new StreamWriter(strPath))
                    new XmlSerializer(typeof(T)).Serialize(writer, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured while writing to file '{strPath}': {ex.Message}");
            }
        }
        #endregion

        #region Read
        public static T Read<T>(string strPath) where T : class, new()
        {
            T retVal = null;

            try
            {
                if (File.Exists(strPath))
                {
                    using (StreamReader reader = new StreamReader(strPath))
                        retVal = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured while reading from file '{strPath}': {ex.Message}");
            }

            if (retVal == null)
                retVal = new T();

            return retVal;
        }
        #endregion
    }
}