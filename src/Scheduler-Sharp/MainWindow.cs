using System;
using System.Collections.Generic;
using Gtk;
using SchedulerSharp.GUI;
using SchedulerSharp.GUI.CreationInterface;
using SchedulerSharp.GUI.PlotInterface;
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
    }

    protected List<PlotableProcess> GetProcessFromCreation()
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

    protected void CloseEvent(object sender, EventArgs e) => Application.Quit();

    protected void PauseEvent(object sender, EventArgs e) => plot.Pause();

    protected void ExportSVG(object sender, EventArgs e) => escalonadPlot.ExportSVG();

    protected void ExportPNG(object sender, EventArgs e) => escalonadPlot.ExportPNG();

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
        rr = RR.Schedulering(li, (int)quantumScale.Value, progressRR);

        Application.Invoke((sender, e) => OnSelectScheduler(SchedulerCombobox, e));
    }

    void ThreFCFS()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        fcfs = FCFS.Schedulering(li, progressFCFS);

        Application.Invoke((sender, e) => OnSelectScheduler(SchedulerCombobox, e));
    }

    protected void OnSelectScheduler(object sender, EventArgs e)
    {
        switch (((ComboBox)sender).ActiveText)
        {
            case "RR":
                if (rr != null)
                    escalonadPlot.AnimateData(rr, true, "Round Robin");
                break;
            case "FCFS":
                if (fcfs != null)
                    escalonadPlot.AnimateData(fcfs, true, "FCFS");
                break;
            default:
                break;
        }
    }

    protected void OnQuantumChange(object sender, EventArgs e)
    {
        PlotAnim rrAnim = new PlotAnim(ThreRR, "RR");
        rrAnim.StartAnim();
    }

    protected void OnChangeDirEntry(object sender, EventArgs e)
    {
        if (creationController.OnChangeDirEntry((ComboBox)sender))
        {
            Console.WriteLine("Atualizado com susses");
            toPlot = GetProcessFromCreation();
            plot.AnimateData(toPlot, false, "Entrada");

            PlotAnim fcfsAnim = new PlotAnim(ThreFCFS, "FCFS");
            fcfsAnim.StartAnim();

            PlotAnim rrAnim = new PlotAnim(ThreRR, "RR");
            rrAnim.StartAnim();
        }
    }

    protected void PlayEvent(object sender, EventArgs e)
    {
    }
}
