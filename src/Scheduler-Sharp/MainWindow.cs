using System;
using Gtk;
using OxyPlot.GtkSharp;
using System.Collections.Generic;

using SchedulerSharp.GUI;
using SchedulerSharp.Models;

public partial class MainWindow : Gtk.Window
{
    CreationController creationController;
    PlotInterface plot;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        creationController = new CreationController(new List<ComboBox> 
            {   dirCreatorEntry, 
                directoryEntry 
            },
                scrolledWindow);
        plot = new PlotInterface(plotBox);
    }

    protected List<PlotableProcess> Gerar ()
    {
        List<PlotableProcess> plotables = new List<PlotableProcess>();
        List<EscalonableProcess> escalonables = new List<EscalonableProcess>();

        foreach(Process process in creationController.GetItens())
        {
            EscalonableProcess eProcess = new EscalonableProcess(process);
            plotables.Add(new PlotableProcess(eProcess, 0));
        }

        return plotables;
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

    protected void SaveAsEvent (object sender, EventArgs e)
    {
        if(GTKUtils.ShowFileChooser(out string path,"Salvar Como...", "Salvar"))
            creationController.SaveAsEvent(path);
    }

    protected void SaveEvent(object sender, EventArgs e)
    {
        creationController.SaveEvent();
    }

    protected void EditButtonEvent(object sender, EventArgs e)
    {
        if(GTKUtils.ShowFileChooser(out string path,"Abrir...", "Selecionar"))
            creationController.EditButtonEvent(path);
    }

    protected void NewEvent (object sender, EventArgs e)
    {
        creationController.NewEvent();
    }

    protected void OnChangeDirEntry(object sender, EventArgs e)
    {
        creationController.OnChangeDirEntry((ComboBox) sender);
    }

    protected void PlayEvent(object sender, EventArgs e)
    {
        plot.UpdateData(Gerar());
    }

    protected void CloseEvent(object sender, EventArgs e)
    {
        Application.Quit();
    }
}
