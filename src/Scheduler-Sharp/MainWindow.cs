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

    protected void SaveEvent(object sender, EventArgs e) => creationController.SaveEvent(ChangeDirCallback);

    protected void NewEvent(object sender, EventArgs e) => creationController.NewEvent();
    #endregion

    #region "Eventos de Criação"
    protected void AddCreationEvent(object sender, EventArgs e) => creationController.AddCreationEvent();

    protected void RemoveCreationEvent(object sender, EventArgs e) => creationController.RemoveCreationEvent();
    #endregion

    #region "Eventos de Midia"
    protected void PlayEvent(object sender, EventArgs e) => escalonadPlot.Play();

    protected void PauseEvent(object sender, EventArgs e) => escalonadPlot.Pause();

    protected void NextEvent(object sender, EventArgs e)
    {
        escalonadPlot.Next();
    }

    protected void PreviewEvent(object sender, EventArgs e)
    {
        escalonadPlot.Preview();
    }

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

    protected void OnSelectScheduler(object sender, EventArgs e)
    {
        escalonadPlot.Pause();
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

    protected void ExportALLSVG(object sender, EventArgs e)
    {
        if (fcfs != null && sjf != null && rr != null)
        {
            if (GTKUtils.ShowFolderChooser(out string path))
            {
                string[] names =
                {
                    "FCFS","SJF", "RR", "Compare",
                };

                for (int i = 0; i < names.Length; i++)
                {
                    ExportTo(i, path, names[i], false);
                }
                plot.ExportSVG(path + "Entrada.svg");
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportTo(int index, string path, string name, bool png)
    {
        if (png)
        {
            SchedulerCombobox.Active = index;
            OnSelectScheduler(SchedulerCombobox, EventArgs.Empty);
            escalonadPlot.ExportPNG(path + name + ".png");
        }
        else
        {
            SchedulerCombobox.Active = index;
            OnSelectScheduler(SchedulerCombobox, EventArgs.Empty);
            escalonadPlot.ExportPNG(path + name + ".svg");
        }
    }

    protected void ExportALLPNG(object sender, EventArgs e)
    {
        /*
        if (GTKUtils.ShowFileChooser(out string qw, ".png", "Salvar Como...", "Salvar"))
        { }
        */

        if (fcfs != null && sjf != null && rr != null)
        {
            if (GTKUtils.ShowFolderChooser(out string path))
            {
                string[] names =
                {
                "FCFS","SJF", "RR", "Compare",
                };

                for (int i = 0; i < names.Length; i++)
                {
                    ExportTo(i, path, names[i], true);
                }
                plot.ExportPNG(path + "Entrada.png");
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportFCFSPNG(object sender, EventArgs e)
    {
        if (fcfs != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".png", "Salvar Como...", "Salvar"))
            {
                ExportTo(0, path, "", true);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportFCFSSVG(object sender, EventArgs e)
    {
        if (fcfs != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".svg", "Salvar Como...", "Salvar"))
            {
                ExportTo(0, path, "", false);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportSJFPNG(object sender, EventArgs e)
    {
        if (sjf != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".png", "Salvar Como...", "Salvar"))
            {
                ExportTo(1, path, "", true);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportSJFSVG(object sender, EventArgs e)
    {
        if (sjf != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".svg", "Salvar Como...", "Salvar"))
            {
                ExportTo(1, path, "", false);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportRRPNG(object sender, EventArgs e)
    {
        if (rr != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".png", "Salvar Como...", "Salvar"))
            {
                ExportTo(2, path, "", true);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportRRSVG(object sender, EventArgs e)
    {
        if (rr != null)
        {
            if (GTKUtils.ShowFileChooser(out string path, ".svg", "Salvar Como...", "Salvar"))
            {
                ExportTo(2, path, "", false);
            }
        }
        else
        {
            GTKUtils.ShowDilog("Não ha processos escalonados!", MessageType.Info,
                "Não existem dados a serem exportados!");
        }
    }

    protected void ExportComparePNG(object sender, EventArgs e)
    {
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

        if (GTKUtils.ShowFileChooser(out string path, ".png", "Salvar Como...", "Salvar"))
        {
            escalonadPlot.ExportPNG(path  + "Comparação.png");
        }

    }

    protected void ExportCompareSVG(object sender, EventArgs e)
    {
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

        if (GTKUtils.ShowFileChooser(out string path, ".svg", "Salvar Como...", "Salvar"))
        {
            escalonadPlot.ExportSVG(path + "Comparação.svg");
        }

    }
    #endregion
}
