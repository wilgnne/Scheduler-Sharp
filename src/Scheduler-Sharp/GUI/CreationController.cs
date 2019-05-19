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

        /// <summary>
        /// Inicializa uma nova instancia do <see cref="T:SchedulerSharp.GUI.CreationController"/> class.
        /// Classe responsavel por todo o controle da area de criação
        /// </summary>
        /// <param name="hist">Combobox referente a visualição do historico.</param>
        /// <param name="conteinerCreationView">Conteiner de criação.</param>
        public CreationController(ComboBox hist, Container conteinerCreationView)
        {
            this.hist = hist;
            creationView = new CreationView(conteinerCreationView);

            HistoricInitialize();
        }

        /// <summary>
        /// Inicializa o historico, lendo os endereços contidos no arquivo json
        /// </summary>
        private void HistoricInitialize ()
        {
            string json = null;
            JsonController.OpenJson(historicPath, ref json);
            historicStrings = JsonController.JsonToList<string>(json);
            foreach (string dir in historicStrings)
            {
                hist.AppendText(dir);
            }
        }

        /// <summary>
        /// Atualizar historico
        /// </summary>
        /// <param name="newPath">Endereçõ do novo arquivo.</param>
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
            else if (String.IsNullOrEmpty(newPath) == false)
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

        /// <summary>
        /// Evento do botão Salvar
        /// </summary>
        /// <param name="json">Json.</param>
        /// <param name="path">Path.</param>
        public void SaveButtonEvent (string json, string path)
        {
            if (JsonController.SaveJson(json, path))
            {
                Console.WriteLine(path);
                UpdateHistoric(path);
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
                UpdateHistoric(path);
            }
            else
            {
                historicStrings.Remove(path);
                UpdateHistoric(String.Empty);
            }
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
