using System;
using Gtk;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI
{
    public class CreationController
    {
        public ComboBox hist;
        public CreationView creationView;

        private List<string> historicStrings;
        private string historicBuffer = null;
        private string historicPath = "historic.json";

        public CreationController(ComboBox hist, Container conteinerCreationView)
        {
            this.hist = hist;
            creationView = new CreationView(conteinerCreationView);

            HistoricInitialize();
        }

        private void HistoricInitialize ()
        {
            string json = JsonController.OpenJson(historicPath);
            historicStrings = JsonController.JsonToList<string>(json);
            foreach (string dir in historicStrings)
            {
                hist.AppendText(dir);
            }
        }

        //Loop Error
        private void UpdateHistoric (string newPath)
        {
            historicBuffer = newPath;
            for (int i = historicStrings.Count -1; i >= 0; i--)
                hist.RemoveText(i);

            if (historicStrings.Contains(newPath))
            {
                historicStrings.Remove(newPath);
                historicStrings.Insert(0, newPath);

                foreach (string dir in historicStrings)
                {
                    hist.AppendText(dir);
                }
                hist.Active = 0;
            }
            else
            {
                historicStrings.Insert(0, newPath);

                foreach (string dir in historicStrings)
                {
                    hist.AppendText(dir);
                }
                hist.Active = 0;
            }

            string json = JsonController.ListToJson<string>(historicStrings);
            JsonController.SaveJson(json, historicPath);

        }

        public void SaveButtonEvent (string json, string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                JsonController.SaveJson(json, path);
                Console.WriteLine(path);
                UpdateHistoric(path);
            }
        }

        public void EditButtonEvent (string path)
        {
            string json = JsonController.OpenJson(path);
            creationView.LoadItens(JsonController.JsonToList<Process>(json));
            UpdateHistoric(path);
        }

        public void AddCreationEvent ()
        {
            creationView.AddNewRamdomItem();
        }

        public void RemoveCreationEvent()
        {
            creationView.RemoveSelectedItem();
        }

        public List<Process> GetItens ()
        {
            return creationView.Items;
        }

        public void OnChangeDirEntry()
        {
            if (historicBuffer != hist.ActiveText && 
                String.IsNullOrEmpty(hist.ActiveText) == false)
            {
                EditButtonEvent(hist.ActiveText);
            }
        }
    }
}
