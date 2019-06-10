using System;
using Gtk;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.CreationInterface
{
    public partial class CreationController
    {
        private List<string> historicStrings;
        private string historicBuffer = null;
        private string historicPath = "historic.json";

        /// <summary>
        /// Inicializa o historico, lendo os endereços contidos no arquivo json
        /// </summary>
        private void HistoricInitialize()
        {
            JsonController.SaveJson("[]", historicPath);
            string json = null;
            if (JsonController.OpenJson(historicPath, ref json))
            {
                historicStrings = JsonController.JsonToList<string>(json);

                foreach(ComboBox box in historicBoxs)
                {
                    for (int i = historicStrings.Count - 1; i >= 0; i--)
                    {
                        box.RemoveText(i);
                    }
                    foreach (string dir in historicStrings)
                    {
                        box.AppendText(dir);
                    }
                }

                foreach (string dir in historicStrings)
                {
                    foreach (ComboBox box in historicBoxs)
                    {
                        box.AppendText(dir);
                    }
                }
            }
            else
            {
                JsonController.SaveJson("[]", historicPath);
                HistoricInitialize();
            }
        }

        /// <summary>
        /// Atualizar historico
        /// </summary>
        /// <param name="newPath">Endereçõ do novo arquivo.</param>
        /// /// <param name="box">ComboBox a ser atualizada.</param>
        private void UpdateHistoric(ComboBox box, string newPath)
        {
            historicBuffer = newPath;
            box.Active = -1;
            for (int i = historicStrings.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("Removendo");
                box.RemoveText(i);
            }

            if (historicStrings.Contains(newPath))
            {
                historicStrings.Remove(newPath);
                historicStrings.Insert(0, newPath);

                foreach (string dir in historicStrings)
                {
                    box.AppendText(dir);
                }
                box.Active = 0;
            }
            else if (string.IsNullOrEmpty(newPath) == false)
            {
                historicStrings.Insert(0, newPath);

                foreach (string dir in historicStrings)
                {
                    box.AppendText(dir);
                }
                box.Active = 0;
            }

            string json = JsonController.ListToJson(historicStrings);
            JsonController.SaveJson(json, historicPath);
        }

        /// <summary>
        /// Atualizar todas as ComboBox de historico
        /// </summary>
        /// <param name="newPath">Endereçõ do novo arquivo.</param>
        private void UpdateAllHistoric(string newPath)
        {
            foreach (ComboBox box in historicBoxs)
            {
                UpdateHistoric(box, newPath);
            }
        }


    }
}
