using System;
using System.Collections.Generic;
using Gtk;
using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.CreationInterface
{
    /// <summary>
    /// Classe de criação dos nós de itens.
    /// </summary>
    public partial class CreationView
    {
        public List<Process> Items { get; private set; }

        /// <summary>
        /// Inicializar uma nova instancia de <see cref="T:SchedulerSharp.GUI.CreationView"/> .
        /// </summary>
        /// <param name="scrolledWindows">Conteiner onde a arvore de nós sera criada</param>
        public CreationView(Container scrolledWindows)
        {
            random = new Random();
            Items = new List<Process>();
            tree = new TreeView();
            scrolledWindows.Add(tree);

            // Criando e incerindo elementos no modelo de visualização
            NewProcessListStore();

            // Lista com o nome das colunas
            List<string> columsName = new List<string>
            {
                "Nome",
                "Tempo de Chegada",
                "Tempo de Execução"
            };
            ColumsCreation(columsName);

            scrolledWindows.ShowAll();
        }

        /// <summary>
        /// Carrega uma lista de itens para dentro do visualizador.
        /// </summary>
        /// <param name="processes">Itens a serem carregados.</param>
        public void LoadItens (List<Process> processes)
        {
            NewProcessListStore();
            Items = processes;
            foreach (Process process in Items)
            {
                AddNewItem(process);
            }
        }

        /// <summary>
        /// Adicionar um novo elemento randomico a lista de itens e ao visualizador.
        /// </summary>
        public void AddNewRamdomItem ()
        {
            Process newItem = new Process("PID_" + Items.Count.ToString(), random.Next(0, 100), random.Next(1, 50));
            Items.Add(newItem);
            AddNewItem(newItem);
        }

        /// <summary>
        /// Remover o elemento atualmente selecionado
        /// </summary>
        public void RemoveSelectedItem ()
        {
            tree.Selection.GetSelected(out TreeIter iter);
            Process process = (Process)processListStore.GetValue(iter, 0);
            processListStore.Remove(ref iter);
            Items.Remove(process);
        }
    }
}
