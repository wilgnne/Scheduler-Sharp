using System;
using Gtk;
using OxyPlot.GtkSharp;
using System.Collections.Generic;

using SchedulerSharp.GUI;
using SchedulerSharp.GUI.PlotInterface;
using SchedulerSharp.GUI.CreationInterface;
using SchedulerSharp.Models;
using SchedulerSharp.Schedulers;

public partial class MainWindow : Gtk.Window
{
    public CreationController creationController;
    public PlotInterface plot;
    public PlotInterface escalonadPlot;

    public List<PlotableProcess> fcfs;
    public List<PlotableProcess> rr;
    public List<PlotableProcess> toPlot;

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();
        creationController = new CreationController(new List<ComboBox>
            {   dirCreatorEntry,
                directoryEntry
            },
                scrolledWindow);
        plot = new PlotInterface(plotBox);
        escalonadPlot = new PlotInterface(plotBox);

        progressRR.Fraction = 0.5;
    }

    protected List<PlotableProcess> Gerar()
    {
        List<PlotableProcess> plotables = new List<PlotableProcess>();
        List<EscalonableProcess> escalonables = new List<EscalonableProcess>();

        foreach (Process process in InsertionSort.InsertionSort_Processes(creationController.GetItens()))
        {
            EscalonableProcess eProcess = new EscalonableProcess(process);
            plotables.Add(new PlotableProcess(eProcess, 0));
        }

        return plotables;
    }

    protected List<PlotableProcess> GerarCompare()
    {
        List<PlotableProcess> plotables = new List<PlotableProcess>();
        List<EscalonableProcess> escalonables = new List<EscalonableProcess>();

        ProcessCompare compare = new ProcessCompare();

        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);

        foreach (Process process in li)
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

    protected void SaveAsEvent(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".json", "Salvar Como...", "Salvar"))
            creationController.SaveAsEvent(path);
    }

    protected void EditButtonEvent(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".json", "Abrir...", "Selecionar"))
            creationController.EditButtonEvent(path);
    }

    protected void AddCreationEvent(object sender, EventArgs e) => creationController.AddCreationEvent();

    protected void RemoveCreationEvent(object sender, EventArgs e) => creationController.RemoveCreationEvent();

    protected void SaveEvent(object sender, EventArgs e) => creationController.SaveEvent();

    protected void NewEvent(object sender, EventArgs e) => creationController.NewEvent();

    protected void OnChangeDirEntry(object sender, EventArgs e)
    {
        if (creationController.OnChangeDirEntry((ComboBox)sender))
        {
            toPlot = GerarCompare();
            PlotAnim fcfsAnim = new PlotAnim(ThreFCFS, "FCFS");
            fcfsAnim.StartAnim();
        }
    }

    protected void PlayEvent(object sender, EventArgs e)
    {
    }

    protected void CloseEvent(object sender, EventArgs e) => Application.Quit();

    protected void PauseEvent(object sender, EventArgs e) => plot.Pause();

    protected void ExportSVG(object sender, EventArgs e) => plot.ExportSVG();

    protected void ExportPNG(object sender, EventArgs e) => plot.ExportPNG();

    protected void Frame(object o, FrameEventArgs args)
    {
        Console.WriteLine("Frame");
    }

    protected override bool OnConfigureEvent(Gdk.EventConfigure args)
    {
        base.OnConfigureEvent(args);
        GetSize(out int w, out int h);
        plotBox.Position = w / 2;
        return true;
    }

    void ThreRR()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        rr = RR.Schedulering(li, 5);

        Application.Invoke((sender, e) =>  escalonadPlot.AnimateData(rr, true));
    }

    void ThreFCFS()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        fcfs = FCFS.Schedulering(li, progressFCFS);

        Application.Invoke((sender, e) => plot.AnimateData(fcfs, true));
    }

    protected void OnSelectScheduler(object sender, EventArgs e)
    {
        switch (((ComboBox)sender).ActiveText)
        {
            case "FCFS":
                break;
            default:
                break;
        }
    }
}
