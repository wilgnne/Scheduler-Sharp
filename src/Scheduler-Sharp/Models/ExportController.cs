using System;
using Gtk;
using System.Collections.Generic;
using SchedulerSharp.GUI;
using SchedulerSharp.GUI.PlotInterface;
using SchedulerSharp.Schedulers;

namespace SchedulerSharp.Models
{

    public static class ExportController
    {
        public static void NotProcess() => 
            GTKUtils.ShowDilog("Não ha processos escalonados!", 
                MessageType.Info, "Não existem dados a serem exportados!");

        public static void ErrorDilog (string title, string messege) => 
            GTKUtils.ShowDilog(title, MessageType.Error, messege);

        public static void WarningDilog(string title, string messege) =>
            GTKUtils.ShowDilog(title, MessageType.Warning, messege);

        public static void InfoDilog(string title, string messege) =>
            GTKUtils.ShowDilog(title, MessageType.Info, messege);

        public static void LogJsonExport(List<PlotableProcess> fcfs, List<PlotableProcess> sjf, List<PlotableProcess> rr, double quantum)
        {
            if (fcfs != null && sjf != null && rr != null)
            {
                SchedulersResult result = new SchedulersResult
                {
                    FCFS = fcfs.ConvertAll((input) => { return new LogProcess(input); }),
                    SJF = sjf.ConvertAll((input) => { return new LogProcess(input); }),
                    Quantum = quantum,
                    RR = rr.ConvertAll((input) => { return new LogProcess(input); }),
                };
                if (GTKUtils.ShowFileChooser(out string path, ".log", "Exportar Log: .log", "Salvar"))
                {
                    string json = JsonController.ObjectToJson(result);
                    JsonController.SaveJson(json, path);
                }
            }
            else
            {
                NotProcess();
            }
        }

        public static void LogTXTExport (List<PlotableProcess> fcfs, List<PlotableProcess> sjf, List<PlotableProcess> rr, double quantum)
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
                txt += "\nRR: Quantum = +" + quantum.ToString() + "\n";
                foreach (PlotableProcess process in rr)
                {
                    txt += process.Name + " " + process.ExecTime + "\n";
                }
                if (GTKUtils.ShowFileChooser(out string path, ".txt", "Exportar Log: .txt", "Salvar"))
                {
                    JsonController.SaveJson(txt, path);
                }
            }
            else
            {
                NotProcess();
            }
        }
    
        public static void LogJsonImport ()
        {
            if (GTKUtils.ShowFileChooser(out string path, ".log", "Abrir Log", "Abrir"))
            {
                string json = "";
                if (JsonController.OpenJson(path, ref json))
                {
                    SchedulersResult result = new SchedulersResult();
                    if (JsonController.JsonToObject(json, ref result))
                    {
                        Console.WriteLine(result);
                        ImportView view = new ImportView(result);
                        view.Show();
                    }
                    else
                    {
                        WarningDilog("Erro ao deserializar arquivo!",
                        "Arquivo não contem as informações nessesarias!");
                    }

                }
                else
                {
                    WarningDilog("Erro ao abrir o arquivo!", "Arquivo inexistente ou corrompido!");
                }
            }
        }


        public static void Export<T>(List<T> toExport, int index, string extension, ComboBox box, PlotInterface @interface)
        {
            if (toExport != null)
            {
                if (GTKUtils.ShowFileChooser(out string path, extension, "Salvar Como...", "Salvar"))
                {
                    ExportTo(index, path, "", "", box, @interface);
                }
            }
            else
            {
                InfoDilog("Não ha processos escalonados!", "Não existem dados a serem exportados!");
            }
        }

        private static void ExportTo(int index, string path, string name, string extension, ComboBox box, PlotInterface @interface)
        {
            box.Active = index;
            @interface.ExportPNG(path + name + extension);
        }

        public static string ExportALL<T>(List<T> fcfs, List<T> sjf, List<T> rr, string extension, ComboBox box, PlotInterface @interface)
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
                        ExportTo(i, path, names[i], extension, box, @interface);
                    }

                    return path;
                }
            }
            else
            {
                InfoDilog("Não ha processos escalonados!", "Não existem dados a serem exportados!");
            }

            return null;
        }

    }
}
