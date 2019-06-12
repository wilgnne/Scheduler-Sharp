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

    public List<PlotableProcess> rr, sjf, fcfs;
    public List<PlotableProcess> toPlot;

    public double waitTimeRR, waitTimeSJF, waitTimeFCFS;
    public double turnarondTimeRR, turnarondTimeSJF, turnarondTimeFCFS;
    public double responseTimeRR, responseTimeSJF, responseTimeFCFS;

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();
        creationController = new CreationController(new List<ComboBox>
            {   dirCreatorEntry,
                directoryEntry
            },
                scrolledWindow);

        plot = new PlotInterface(plotBox);
        escalonadPlot = new PlotInterface(plotBox, AtualizeAnimCallBack);
        DisableAll();
    }

    protected void DisableAll ()
    {
        SchedulerCombobox.Sensitive = false;
        quantumScale.Sensitive = false;
        MediaButtonSensitibe(false);
    }

    #region "Eventos de Saida"     
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void CloseEvent(object sender, EventArgs e) => Application.Quit();
    #endregion

    #region "Eventos de manipulação de dados"
    //Evento de manipulação de dados
    protected void EditButtonEvent(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".prb", "Abrir...", "Selecionar"))
            creationController.EditButtonEvent(path, ChangeDirCallback);

    }

    protected void SaveAsEvent(object sender, EventArgs e) => creationController.SaveAsEvent(ChangeDirCallback);

    protected void SaveEvent(object sender, EventArgs e) => creationController.SaveEvent(ChangeDirCallback);

    protected void NewEvent(object sender, EventArgs e)
    {
        creationController.NewEvent();
        DisableAll();
    }
    #endregion

    #region "Eventos de Criação"
    protected void AddCreationEvent(object sender, EventArgs e) => creationController.AddCreationEvent();

    protected void RemoveCreationEvent(object sender, EventArgs e) => creationController.RemoveCreationEvent();
    #endregion

    #region "Eventos de Midia"
    protected void PlayEvent(object sender, EventArgs e) => escalonadPlot.Play();

    protected void PauseEvent(object sender, EventArgs e) => escalonadPlot.Pause();

    protected void NextEvent(object sender, EventArgs e) => escalonadPlot.Next();

    protected void PreviewEvent(object sender, EventArgs e) => escalonadPlot.Preview();

    #endregion

    #region "Eventos de Mudanças em GUI"
    protected void OnChangeDirEntry(object sender, EventArgs e) =>
        creationController.OnChangeDirEntry((ComboBox)sender, ChangeDirCallback);

    protected override bool OnConfigureEvent(Gdk.EventConfigure evnt)
    {
        base.OnConfigureEvent(evnt);
        GetSize(out int w, out int h);
        plotBox.Position = w / 2;
        return true;
    }

    protected void GenereteList<T>(T responseTime, T turnarondTime, T waitTime, out List<T> output) => output = new List<T>
        {
            responseTime,  turnarondTime, waitTime,
        };

    protected void OnSelectScheduler(object sender, EventArgs e)
    {
        escalonadPlot.Pause();
        switch (((ComboBox)sender).ActiveText)
        {
            case "Benchmark":
                MediaButtonSensitibe(false);

                GenereteList(responseTimeFCFS, turnarondTimeFCFS, waitTimeFCFS, out List<double> fcfsTime);

                GenereteList(responseTimeSJF, turnarondTimeSJF, waitTimeSJF, out List<double> sjfTime);

                GenereteList(responseTimeRR, turnarondTimeRR, waitTimeRR, out List<double> rrTime);

                GenereteList("Tempo de Resposta", "Tempo de Vida", "Tempo de Espera", out List<string> text);

                escalonadPlot.AnimateData(fcfsTime, sjfTime, rrTime, text, "Benchmark");
                break;
            case "SJF":
                MediaButtonSensitibe(true);
                if (sjf != null)
                    escalonadPlot.AnimateData(sjf, true, "SJF", plot.YLabel);
                break;
            case "RR":
                MediaButtonSensitibe(true);
                if (rr != null)
                    escalonadPlot.AnimateData(rr, true, "Round Robin / Quantum: " + ((int)quantumScale.Value).ToString(), plot.YLabel);
                break;
            case "FCFS":
                MediaButtonSensitibe(true);
                if (fcfs != null)
                    escalonadPlot.AnimateData(fcfs, true, "FCFS", plot.YLabel);
                break;
        }
    }

    protected void OnQuantumChange(object sender, EventArgs e)
    {

        PlotAnim rrAnim = new PlotAnim(ThreRR, "RR");
        rrAnim.StartAnim();
    }

    #endregion

    #region "Eventos de Exportagem"
    protected void LogJsonEvent(object sender, EventArgs e) => ExportController.LogExport(fcfs, sjf, rr, quantumScale.Value);

    protected void LogTxtEvent(object sender, EventArgs e) => ExportController.LogTXTExport(fcfs, sjf, rr, quantumScale.Value);

    protected void OnOpenLogJson(object sender, EventArgs e) => ExportController.LogJsonImport();

    protected void ExportALLSVG(object sender, EventArgs e)
    {
        string path = ExportController.ExportALL(fcfs, sjf, rr, ".svg", SchedulerCombobox, escalonadPlot);
        plot.ExportSVG(path + "Entrada.svg");
    }

    protected void ExportALLPNG(object sender, EventArgs e)
    {
        string path = ExportController.ExportALL(fcfs, sjf, rr, ".png", SchedulerCombobox, escalonadPlot);
        plot.ExportPNG(path + "Entrada.png");
    }

    protected void ExportFCFSPNG(object sender, EventArgs e) => ExportController.Export(fcfs, 0, ".png", SchedulerCombobox, escalonadPlot);

    protected void ExportFCFSSVG(object sender, EventArgs e) => ExportController.Export(fcfs, 0, ".svg", SchedulerCombobox, escalonadPlot);

    protected void ExportSJFPNG(object sender, EventArgs e) => ExportController.Export(sjf, 1, ".png", SchedulerCombobox, escalonadPlot);

    protected void ExportSJFSVG(object sender, EventArgs e) => ExportController.Export(sjf, 1, ".svg", SchedulerCombobox, escalonadPlot);

    protected void ExportRRPNG(object sender, EventArgs e) => ExportController.Export(rr, 2, ".png", SchedulerCombobox, escalonadPlot);

    protected void ExportRRSVG(object sender, EventArgs e) => ExportController.Export(rr, 2, ".svg", SchedulerCombobox, escalonadPlot);

    protected void ExportComparePNG(object sender, EventArgs e) => ExportController.Export(fcfs, 3, ".png", SchedulerCombobox, escalonadPlot);

    protected void ExportCompareSVG(object sender, EventArgs e) => ExportController.Export(fcfs, 3, ".svg", SchedulerCombobox, escalonadPlot);
    #endregion

    protected void AboutWindowOpen(object sender, EventArgs e)
    {
        SchedulerSharp.Windows.AboutDilog about = new SchedulerSharp.Windows.AboutDilog(this);
        about.ShowAll();
    }

    protected void TxtToPRBEvent(object sender, EventArgs e)
    {
        ExportController.TxtToPRBConverter();
    }

    public void HelpEvent(object sender, EventArgs e)
    {
        System.Threading.Thread thr = new System.Threading.Thread
        (new System.Threading.ThreadStart(delegate {
            System.Diagnostics.Process.Start("http://www.github.com/Wilgnne/Scheduler-Sharp");
            }));
        thr.Start();
    }
}
