using System;
using System.Collections.Generic;
using Gtk;

namespace SchedulerSharp.GUI
{
    public class CreationView
    {
        TreeView tree;
        ListStore processListStore;

        public List<ProcessItem> Items { get; private set; }

        public CreationView(Container scrolledWindows)
        {
            Items = new List<ProcessItem>
            {
                new ProcessItem("process", 0, 5),
                new ProcessItem("process", 5, 10)
            };

            tree = new Gtk.TreeView();
            scrolledWindows.Add(tree);

            // Criando a coluna Nome
            TreeViewColumn nameColumn = new TreeViewColumn
            {
                Title = "Nome",
                Resizable = true
            };

            // Criando a coluna Tempo de chegada
            TreeViewColumn arrivalTimeColumn = new TreeViewColumn
            {
                Title = "Tempo de Chegada",
                Resizable = true
            };

            // Criando a coluna Tempo de chegada
            TreeViewColumn runtimeColumn = new TreeViewColumn
            {
                Title = "Tempo de execução",
                Resizable = true
            };

            // Criando celula de renderização de nome
            CellRendererText nameCell = new CellRendererText();
            nameColumn.PackStart(nameCell, true);

            // Criando celula de renderização de tempo de chegada
            CellRendererText arrivalTimeCell = new CellRendererText();
            arrivalTimeColumn.PackStart(arrivalTimeCell, true);

            //Criando celula de renderização de tempo de execução
            CellRendererText runtimeCell = new CellRendererText();
            runtimeColumn.PackStart(runtimeCell, true);

            // Criando e incerindo elementos no modelo de visualização
            processListStore = new ListStore(typeof(ProcessItem));
            foreach (ProcessItem process in Items)
            {
                processListStore.AppendValues(process);
            }

            // Configurando funções de renderizalção de celulas
            nameColumn.SetCellDataFunc(nameCell, new TreeCellDataFunc(RenderName));
            arrivalTimeColumn.SetCellDataFunc(arrivalTimeCell, new TreeCellDataFunc(RenderArrivalTime));
            runtimeColumn.SetCellDataFunc(runtimeCell, new TreeCellDataFunc(RenderRuntime));

            tree.Model = processListStore;

            //Adicionando colunas ao TreeView
            tree.AppendColumn(nameColumn);
            tree.AppendColumn(arrivalTimeColumn);
            tree.AppendColumn(runtimeColumn);

            // Configurando celulas como editaveis
            nameCell.Editable = true;
            nameCell.Edited += NameCell_Edited;

            arrivalTimeCell.Editable = true;
            arrivalTimeCell.Edited += ArrivalTimeCell_Edited;

            runtimeCell.Editable = true;
            runtimeCell.Edited += RuntimeCelll_Edited;

            scrolledWindows.ShowAll();
        }

        public void AddNewItem ()
        {
            ProcessItem newItem = new ProcessItem();
            Items.Add(newItem);
            processListStore.AppendValues(newItem);
        }

        public void RemoveSelectedItem ()
        {
            tree.Selection.GetSelected(out TreeIter iter);
            ProcessItem process = (ProcessItem)processListStore.GetValue(iter, 0);
            processListStore.Remove(ref iter);
            Items.Remove(process);
        }

        private void NameCell_Edited(object o, Gtk.EditedArgs args)
        {
            processListStore.GetIter(out TreeIter iter, new Gtk.TreePath(args.Path));

            ProcessItem process = (ProcessItem)processListStore.GetValue(iter, 0);
            process.name = (string)args.NewText;

            foreach (ProcessItem item in Items)
            {
                Console.WriteLine(item);
            }
        }

        private void ArrivalTimeCell_Edited(object o, Gtk.EditedArgs args)
        {
            processListStore.GetIter(out TreeIter iter, new Gtk.TreePath(args.Path));

            ProcessItem process = (ProcessItem)processListStore.GetValue(iter, 0);

            int.TryParse(args.NewText, out process.arrivalTime);
        }

        private void RuntimeCelll_Edited(object o, Gtk.EditedArgs args)
        {
            processListStore.GetIter(out TreeIter iter, new Gtk.TreePath(args.Path));

            ProcessItem process = (ProcessItem)processListStore.GetValue(iter, 0);
            int.TryParse(args.NewText, out process.runtime);
        }

        private void RenderName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            ProcessItem song = (ProcessItem)model.GetValue(iter, 0);
            (cell as Gtk.CellRendererText).Text = song.name;
        }

        private void RenderArrivalTime(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            ProcessItem song = (ProcessItem)model.GetValue(iter, 0);
            (cell as Gtk.CellRendererText).Text = song.arrivalTime.ToString();
        }

        private void RenderRuntime(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            ProcessItem song = (ProcessItem)model.GetValue(iter, 0);
            (cell as Gtk.CellRendererText).Text = song.runtime.ToString();
        }
    }


    public class ProcessItem
    {
        public ProcessItem(string name, int arrivalTime, int runtime)
        {
            this.name = name;
            this.arrivalTime = arrivalTime;
            this.runtime = runtime;
        }

        public ProcessItem()
        { }

        public override string ToString()
        {
            string arrival = this.arrivalTime.ToString();
            string runtime = this.runtime.ToString();
            return "Nome: " + name + ", ArrivalTime: " + arrival + ", Runtime: " + runtime;
        }

        public string name = "Processo";
        public int arrivalTime;
        public int runtime;
    }
}
