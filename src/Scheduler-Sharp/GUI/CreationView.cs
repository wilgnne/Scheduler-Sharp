using System;
using System.Collections.Generic;
using Gtk;
using SchedulerSharp.Models;

namespace SchedulerSharp.GUI
{
    public class CreationView
    {
        public List<Process> Items { get; private set; }

        TreeView tree;
        ListStore processListStore;

        List<TreeViewColumn> columns;
        List<CellRendererText> cells;

        public CreationView(Container scrolledWindows)
        {
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

            scrolledWindows.ShowAll();
        }

        public void LoadItens (List<Process> processes)
        {
            NewProcessListStore();
            Items = processes;
            foreach (Process process in Items)
            {
                AddNewItem(process);
            }
        }

        private void NewProcessListStore()
        {
            processListStore = new ListStore(typeof(Process));
            tree.Model = processListStore;
        }

        // Adiciona um novo elemento a lista de Itens e ao Visualizador
        public void AddNewRamdomItem ()
        {
            Random r = new Random();
            Process newItem = new Process("PID: " + Items.Count.ToString(), r.Next(0, 15), r.Next(1, 15));
            Items.Add(newItem);
            AddNewItem(newItem);
        }

        private void AddNewItem (Process newItem)
        {
            processListStore.AppendValues(newItem);
        }

        // Remove o indice atualmente selecionado
        public void RemoveSelectedItem ()
        {
            tree.Selection.GetSelected(out TreeIter iter);
            Process process = (Process)processListStore.GetValue(iter, 0);
            processListStore.Remove(ref iter);
            Items.Remove(process);
        }

        // Edita a celula
        private void EditText (object o, EditedArgs args)
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

        // Desenha o texto da celula
        private void RenderText (TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
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
