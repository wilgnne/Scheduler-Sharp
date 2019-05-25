using System.Collections.Generic;
using Gtk;
using System;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.CreationInterface
{
    public partial class CreationView
    {
        TreeView tree;
        // Lista de processos para o TreeView
        ListStore processListStore;

        List<TreeViewColumn> columns;
        // Celulas das Colunas
        List<CellRendererText> cells;

        Random random;

        /// <summary>
        /// Criar um novo visualizador limpo.
        /// </summary>
        private void NewProcessListStore()
        {
            processListStore = new ListStore(typeof(Process));
            tree.Model = processListStore;
        }

        /// <summary>
        /// Adicionar um novo elemento a lista de itens e ao visualizador.
        /// </summary>
        /// <param name="newItem">Item a ser adicionado.</param>
        private void AddNewItem(Process newItem)
        {
            processListStore.AppendValues(newItem);
        }

        /// <summary>
        /// Criar instancias das colunas.
        /// </summary>
        /// <param name="columsName">Lista com os nomes para as colunas.</param>
        private void ColumsCreation (List<string> columsName)
        {
            // Criacao das colunas
            columns = new List<TreeViewColumn>();
            cells = new List<CellRendererText>();
            for (int i = 0; i < columsName.Count; i++)
            {
                TreeViewColumn column = new TreeViewColumn
                {// Instacia das colunas, alinhadas ao centro e empandidas
                    Title = columsName[i],
                    Alignment = 0.5f,
                    Expand = true
                };
                // Adicionando referencia a lista de colunas
                columns.Add(column);

                // Criando celula de renderização
                CellRendererText cell = new CellRendererText();
                column.PackStart(cell, true);

                // Adicionando referencia a lista de celulas
                cells.Add(cell);

                // Configurando funções de renderizalção de celulas
                column.SetCellDataFunc(cell, new TreeCellDataFunc(RenderText));

                //Adicionando colunas ao TreeView
                tree.AppendColumn(column);

                // Configurando celulas como editaveis
                cell.Editable = true;
                cell.Edited += EditText;
            }
        }

        /// <summary>
        /// Editar a celula de texto
        /// </summary>
        private void EditText(object o, EditedArgs args)
        {
            // Obtendo referencia do processo apartir dos argumentos recebidos
            processListStore.GetIter(out TreeIter iter, new Gtk.TreePath(args.Path));
            Process process = (Process)processListStore.GetValue(iter, 0);

            // Obtendo referencia da coluna apartir da referencia da celula
            CellRendererText cell = (CellRendererText)o;
            TreeViewColumn column = columns[cells.IndexOf(cell)];
            switch (column.Title)
            {
                case "Nome":
                    process.Name = args.NewText;
                    break;
                case "Tempo de Chegada":
                    int.TryParse(args.NewText, out process.ArrivalTime);
                    break;
                case "Tempo de Execução":
                    int.TryParse(args.NewText, out process.Runtime);
                    break;
                default:
                    (cell as CellRendererText).Text = "Coluna Indefinida";
                    break;
            }
        }

        /// <summary>
        /// Desenhar a celula de texto
        /// </summary>
        private void RenderText(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            // Obtendo a referencia do processo apartir dos argumentos recebidos
            Process process = (Process)model.GetValue(iter, 0);
            switch (column.Title)
            {
                case "Nome":
                    (cell as CellRendererText).Text = process.Name;
                    break;
                case "Tempo de Chegada":
                    (cell as CellRendererText).Text = process.ArrivalTime.ToString();
                    break;
                case "Tempo de Execução":
                    (cell as CellRendererText).Text = process.Runtime.ToString();
                    break;
                default:
                    (cell as CellRendererText).Text = "Coluna Indefinida";
                    break;
            }
        }
    }
}
