#region Using
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
#endregion

namespace Spawn.HDT.DeckCodeBot
{
    public static class FileHelper
    {
        #region Write
        public static void Write<T>(string strPath, T value, bool blnEncrypt = false) where T : class, new()
        {
            try
            {
                if (File.Exists(strPath))
                    File.Delete(strPath);

                using (StreamWriter writer = new StreamWriter(strPath))
                    new XmlSerializer(typeof(T)).Serialize(writer, value);

                if (blnEncrypt)
                {
                    string strContent;

                    using (StreamReader reader = new StreamReader(File.OpenRead(strPath)))
                    {
                        strContent = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(strContent))
                    {
                        strContent = EncryptDecrypt(strContent, 256);

                        File.Delete(strPath);

                        using (StreamWriter writer = new StreamWriter(File.OpenWrite(strPath)))
                        {
                            writer.Write(strContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured while writing to file '{strPath}': {ex.Message}");
            }
        }
        #endregion

        #region Read
        public static T Read<T>(string strPath, bool blnDecrypt = false) where T : class, new()
        {
            T retVal = null;

            try
            {
                if (File.Exists(strPath))
                {
                    if (blnDecrypt)
                    {
                        string strContent;

                        using (StreamReader reader = new StreamReader(strPath))
                        {
                            strContent = reader.ReadToEnd();
                        }

                        if (!string.IsNullOrEmpty(strContent))
                        {
                            strContent = EncryptDecrypt(strContent, 256);

                            using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(strContent)))
                            {
                                retVal = (T)new XmlSerializer(typeof(T)).Deserialize(ms);
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(strPath))
                            retVal = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
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

        #region EncryptDecrypt
        // https://www.codingame.com/playgrounds/11117/simple-encryption-using-c-and-xor-technique
        private static string EncryptDecrypt(string strPlainText, int nKey)
        {
            StringBuilder sbInput = new StringBuilder(strPlainText);
            StringBuilder sbOutput = new StringBuilder(strPlainText.Length);
            char Textch;
            for (int iCount = 0; iCount < strPlainText.Length; iCount++)
            {
                Textch = sbInput[iCount];
                Textch = (char)(Textch ^ nKey);
                sbOutput.Append(Textch);
            }
            return sbOutput.ToString();
        }
        #endregion
    }
}