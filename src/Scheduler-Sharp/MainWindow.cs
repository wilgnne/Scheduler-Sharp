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
        escalonadPlot = new PlotInterface(plotBox);

        SchedulerCombobox.Sensitive = false;
        quantumScale.Sensitive = false;
        MediaButtonSensitibe(false);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void SaveAsEvent(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".json", "Salvar Como...", "Salvar"))
            creationController.SaveAsEvent(path, ChangeDirCallback);

    }

    protected void EditButtonEvent(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".json", "Abrir...", "Selecionar"))
            creationController.EditButtonEvent(path, ChangeDirCallback);

    }

    protected void AddCreationEvent(object sender, EventArgs e) => creationController.AddCreationEvent();

    protected void RemoveCreationEvent(object sender, EventArgs e) => creationController.RemoveCreationEvent();

    protected void SaveEvent(object sender, EventArgs e) => creationController.SaveEvent(ChangeDirCallback);

    protected void NewEvent(object sender, EventArgs e) => creationController.NewEvent();

    protected void CloseEvent(object sender, EventArgs e) => Application.Quit();

    protected void PauseEvent(object sender, EventArgs e) => plot.Pause();

    protected void OnChangeDirEntry(object sender, EventArgs e) =>
        creationController.OnChangeDirEntry((ComboBox)sender, ChangeDirCallback);

    protected override bool OnConfigureEvent(Gdk.EventConfigure evnt)
    {
        base.OnConfigureEvent(evnt);
        GetSize(out int w, out int h);
        plotBox.Position = w / 2;
        return true;
    }

    protected void OnSelectScheduler(object sender, EventArgs e)
    {
        switch (((ComboBox)sender).ActiveText)
        {
            case "Compare":
                MediaButtonSensitibe(false);

                List<double> fcfsTime = new List<double>
                {
                    responseTimeFCFS,  turnarondTimeFCFS, waitTimeFCFS,
                };
                List<double> sjfTime = new List<double>
                {
                    responseTimeSJF, turnarondTimeSJF, waitTimeSJF
                };
                List<double> rrTime = new List<double>
                {
                    responseTimeRR, turnarondTimeRR, waitTimeRR
                };
                List<string> text = new List<string>
                {
                    "Tempo de Resposta", "Tempo de Vida", "Tempo de Espera"
                };
                escalonadPlot.AnimateData(fcfsTime, sjfTime, rrTime, text, "Comparação");
                break;
            case "SJF":
                MediaButtonSensitibe(true);
                if (sjf != null)
                    escalonadPlot.AnimateData(sjf, true, "SJF");
                break;
            case "RR":
                MediaButtonSensitibe(true);
                if (rr != null)
                    escalonadPlot.AnimateData(rr, true, "Round Robin");
                break;
            case "FCFS":
                MediaButtonSensitibe(true);
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

    protected void PlayEvent(object sender, EventArgs e)
    {
    }

    protected void LogJsonEvent(object sender, EventArgs e)
    {
        if (fcfs != null && sjf != null && rr != null)
        {
            SchedulersResult result = new SchedulersResult
            {
                FCFS = fcfs.ConvertAll((input) => { return new LogProcess(input); }),
                SJF = sjf.ConvertAll((input) => { return new LogProcess(input); }),
                Quantum = quantumScale.Value,
                RR = rr.ConvertAll((input) => { return new LogProcess(input); }),
            };
            if (GTKUtils.ShowFileChooser(out string path, ".json", "Salvar Log .json", "Salvar"))
            {
                string json = JsonController.ListToJson(new List<SchedulersResult> { result });
                JsonController.SaveJson(json, path);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void LogTxtEvent(object sender, EventArgs e)
    {
        if (fcfs != null && sjf != null && rr != null)
        {
            string txt = "FCFS\n";
            foreach (PlotableProcess process in fcfs)
            {
                txt += process.Name + " " + process.ExecTime + "\n";
            }
            txt += "\nSJF\n";
            foreach (PlotableProcess process in sjf)
            {
                txt += process.Name + " " + process.ExecTime + "\n";
            }
            txt += "\nRR: Quantum = +" + quantumScale.Value + "\n";
            foreach (PlotableProcess process in rr)
            {
                txt += process.Name + " " + process.ExecTime + "\n";
            }
            if (GTKUtils.ShowFileChooser(out string path, ".txt", "Salvar Log .txt", "Salvar"))
            {
                JsonController.SaveJson(txt, path);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void OnOpenLogJson(object sender, EventArgs e)
    {
        if (GTKUtils.ShowFileChooser(out string path, ".json", "Abrir Log Json", "Abrir"))
        {
            string json = "";
            if (JsonController.OpenJson(path, ref json))
            {
                try
                {
                    SchedulersResult result = JsonController.JsonToList<SchedulersResult>(json)[0];
                    SchedulerSharp.ImportView view = new SchedulerSharp.ImportView(result);
                    view.Show();
                }
                catch (Exception exception)
                {
                    GTKUtils.ShowDilog("Erro ao decodificar arquivo!", MessageType.Error,
                    exception.Message);
                }

            }
            else
            {
                GTKUtils.ShowDilog("Erro ao abrir o arquivo!", MessageType.Warning,
                    "Arquivo inexistente ou corrompido!");
            }
        }
    }

    protected void ExportSVG(object sender, EventArgs e)
    {
        if (fcfs != null && sjf != null && rr != null)
        {
            escalonadPlot.ExportSVG();
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportPNG(object sender, EventArgs e)
    {
        if (fcfs != null && sjf != null && rr != null)
            escalonadPlot.ExportPNG();
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }
}
