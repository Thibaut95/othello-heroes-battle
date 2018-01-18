using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace OthelloHeroesBattle
{
    static class ToolsOthello
    {
        public static void SerializeObject<T>(T serializableObject)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "HEROES files (*.heroes)|*.heroes";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                if (saveFileDialog1.FileName != "")
                { 
                    try
                    {        
                            IFormatter formatter = new BinaryFormatter();
                            Stream stream = new FileStream(saveFileDialog1.FileName,
                                                     FileMode.Create,
                                                     FileAccess.Write, FileShare.None);
                            formatter.Serialize(stream, serializableObject);
                            stream.Close();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    } 
                }   
            }
            if (serializableObject == null) { return; }

        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "HEROES files (*.heroes)|*.heroes";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if (openFileDialog1.FileName!="")
                    {
                        
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(openFileDialog1.FileName,
                                                    FileMode.Open,
                                                    FileAccess.Read,
                                                    FileShare.Read);
                        try
                        {
                            return (T)formatter.Deserialize(stream);
                        }
                        catch
                        {
                            return default(T);
                        }
                        stream.Close();    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            return default(T);
        }

    }
}
