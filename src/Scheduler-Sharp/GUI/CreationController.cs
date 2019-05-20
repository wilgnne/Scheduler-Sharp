using Gtk;
using System;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI
{
    public class CreationController
    {
        public List<ComboBox> historicBoxs;
        public CreationView creationView;

        private List<string> historicStrings;
        private string historicBuffer = null;
        private string historicPath = "historic.json";

        /// <summary>
        /// Inicializa uma nova instancia do <see cref="T:SchedulerSharp.GUI.CreationController"/> class.
        /// Classe responsavel por todo o controle da area de criação
        /// </summary>
        /// <param name="hist">Combobox referente a visualição do historico.</param>
        /// <param name="conteinerCreationView">Conteiner de criação.</param>
        public CreationController(List<ComboBox> comboBoxes,Container conteinerCreationView)
        {
            historicBoxs = comboBoxes;

            creationView = new CreationView(conteinerCreationView);
            HistoricInitialize();
        }

        /// <summary>
        /// Inicializa o historico, lendo os endereços contidos no arquivo json
        /// </summary>
        private void HistoricInitialize ()
        {
            string json = null;
            if(JsonController.OpenJson(historicPath, ref json))
            {
                historicStrings = JsonController.JsonToList<string>(json);
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
        private void UpdateHistoric (ComboBox box, string newPath)
        {
            historicBuffer = newPath;
            for (int i = historicStrings.Count -1; i >= 0; i--)
            {
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
            else if (String.IsNullOrEmpty(newPath) == false)
            {
                historicStrings.Insert(0, newPath);

                foreach (string dir in historicStrings)
                {
                    box.AppendText(dir);
                }
                box.Active = 0;
            }

            string json = JsonController.ListToJson<string>(historicStrings);
            JsonController.SaveJson(json, historicPath);

        }

        /// <summary>
        /// Atualizar todas as ComboBox de historico
        /// </summary>
        /// <param name="newPath">Endereçõ do novo arquivo.</param>
        private void UpdateAllHistoric(string newPath)
        {
            foreach(ComboBox box in historicBoxs)
            {
                UpdateHistoric(box, newPath);
            }
        }

        /// <summary>
        /// Evento do botão Salvar
        /// </summary>
        public void SaveEvent ()
        {
            if (String.IsNullOrEmpty(historicBuffer) == false)
            {
                string json = JsonController.ListToJson(GetItens());
                if (JsonController.SaveJson(json, historicBuffer))
                {
                    Console.WriteLine(historicBuffer);
                    UpdateAllHistoric(historicBuffer);
                }
            }
            else
            {
                if(GTKUtils.ShowFileChooser(out string path, "Salvar como..", "Selecionar"))
                    SaveAsEvent(path);
            }

        }

        /// <summary>
        /// Evento Salvar Como...
        /// </summary>
        /// <param name="path">Diretorio do arquivo.</param>
        public void SaveAsEvent (string path)
        {
            string json = JsonController.ListToJson(GetItens());
            if (JsonController.SaveJson(json, path))
            {
                Console.WriteLine(path);
                UpdateAllHistoric(path);
            }
        }

        /// <summary>
        /// Evento do botão Editar.
        /// </summary>
        /// <param name="path">Endereçõ do novo arquivo a ser editado</param>
        public void EditButtonEvent(string path)
        {
            string json = null;
            if (JsonController.OpenJson(path, ref json))
            {
                creationView.LoadItens(JsonController.JsonToList<Process>(json));
                UpdateAllHistoric(path);
            }
            else
            {
                historicStrings.Remove(path);
                UpdateAllHistoric(String.Empty);
            }
        }

        public void NewEvent()
        {
            creationView.LoadItens(new List<Process>());
            foreach(ComboBox box in historicBoxs)
            {
                box.Active = -1;
            }
            historicBuffer = null;
        }

        /// <summary>
        /// Evento do botão de Adicionar item
        /// </summary>
        public void AddCreationEvent ()
        {
            creationView.AddNewRamdomItem();
        }

        /// <summary>
        /// Evento do botão de remover o item selecionado
        /// </summary>
        public void RemoveCreationEvent()
        {
            creationView.RemoveSelectedItem();
        }

        /// <summary>
        /// Obter itens criados
        /// </summary>
        /// <returns>Os itens criandos</returns>
        public List<Process> GetItens ()
        {
            return creationView.Items;
        }

        /// <summary>
        /// Ao Alterar o diretorio
        /// </summary>
        public void OnChangeDirEntry(ComboBox sender)
        {
            if (historicBuffer != sender.ActiveText && 
                String.IsNullOrEmpty(sender.ActiveText) == false)
            {
                EditButtonEvent(sender.ActiveText);
            }
        }
    }
}
