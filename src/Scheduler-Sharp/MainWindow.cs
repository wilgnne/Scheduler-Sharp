using System;
using Gtk;
using OxyPlot.GtkSharp;
using OxyPlot;
using OxyPlot.Series;

using SchedulerSharp.GUI;
using SchedulerSharp.Models;

public partial class MainWindow : Gtk.Window
{
    CreationController creationController;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        creationController = new CreationController(dirCreatorEntry, scrolledWindow);

        for (int i = 0; i < 2; i++)
        {
            CreatePlot(plotBox);
        }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void AddCreationEvent(object sender, EventArgs e)
    {
        creationController.AddCreationEvent();
    }

    protected void RemoveCreationEvent(object sender, EventArgs e)
    {
        creationController.RemoveCreationEvent();
    }

    protected string ShowSaveDialog(string Title, string confirmText)
    {
        FileFilter filter = new FileFilter
        {
            Name = "JSON"
        };
        filter.AddPattern("*.json");

        string path = null;
        FileChooserDialog saveDialog = new FileChooserDialog(Title, null, 
            FileChooserAction.Save, "Cancelar", ResponseType.Cancel, confirmText, 
            ResponseType.Accept)
        {
            Filter = filter
        };
        if (saveDialog.Run() == (int)Gtk.ResponseType.Accept)
        {
            path = saveDialog.Filename;

            if (saveDialog.Filter.Name == "JSON" && saveDialog.Filename.ToLower().
                Trim().EndsWith(".json", StringComparison.CurrentCulture) == false)
            {
                path = saveDialog.Filename + ".json";
            }
        }

        saveDialog.Destroy();
        Console.WriteLine("Path: " + path);
        return path;
    }

    protected void SaveButtonEvent(object sender, EventArgs e)
    {
        string path = ShowSaveDialog("Salvar como", "Salvar");
        string json = JsonController.ListToJson(creationController.GetItens());

        if (path != null)
            creationController.SaveButtonEvent(json, path);
    }

    protected void EditButtonEvent(object sender, EventArgs e)
    {
        string path = ShowSaveDialog("Editar arquivo", "Selecionar");
        if (path != null)
            creationController.EditButtonEvent(path);
    }

    protected void OnChangeDirEntry(object sender, EventArgs e)
    {
        creationController.OnChangeDirEntry();
    }




    protected void CreatePlot(Container container)
    {
        var plotView = new PlotView();
        container.Add(plotView);
        plotView.ShowAll();

        var myModel = new PlotModel { Title = "Example 1" };
        myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        plotView.Model = myModel;
    }
}
