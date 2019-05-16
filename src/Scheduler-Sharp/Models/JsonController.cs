using System;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SchedulerSharp.Models
{
    public static class JsonController
    {
        public static string ListToJson<T> (List<T> list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public static List<T> JsonToList<T> (string json)
        {
            List<T> l = JsonConvert.DeserializeObject<List<T>>(json);

            return l;
        }

        public static bool SaveJson (string json, string path)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(path, false, Encoding.UTF8))
                {
                    outputFile.WriteLine(json);
                }
            }
            catch (IOException e) //Caso ocora um erro, mostrar uma mensagem
            {
                Console.WriteLine("Erro ao salvar o arquivo");
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static string OpenJson (string path)
        {
            string text = "";
            if (File.Exists(path))
            {
                try
                {   // Abrindo arquivo usando o stream reader.
                    using (StreamReader sr = new StreamReader(path))
                    {
                        // Lendo o arquivo e o salvando na string text
                        text = sr.ReadToEnd();
                    }
                }
                catch (IOException e) //Caso ocora um erro, mostrar uma mensagem
                {
                    Console.WriteLine("Erro ao abrir o arquivo");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                SaveJson("[]", path);
                OpenJson(path);
            }

            return text;
        }
    }
}
