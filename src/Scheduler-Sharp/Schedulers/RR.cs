using System;
using System.Collections.Generic;
using Gtk;

using SchedulerSharp.Models;

namespace SchedulerSharp.Schedulers
{
    public static class RR
    {
        public static List<PlotableProcess> Schedulering(List<Process> processes, int quantun, ProgressBar bar)
        {
            List<PlotableProcess> plotables = new List<PlotableProcess>();

            List<EscalonableProcess> escalonables = processes.ConvertAll((input) => new EscalonableProcess(input));

            Application.Invoke((sender, e) => bar.Fraction = 0);
            double iteracoes = 0;
            processes.ForEach((obj) => { iteracoes = iteracoes + obj.runtime; });
            double cont = 0;


            int execTime = 0;
            while (escalonables.Count != 0)
            {
                List<EscalonableProcess> attTime = escalonables.FindAll((obj) => obj.arrivalTime <= execTime);
                execTime = attTime.Count == 0 ? execTime + 1 : execTime;

                foreach (EscalonableProcess process in attTime)
                {
                    int attQuantun = 0;
                    while (attQuantun < quantun)
                    {
                        if (process.Run())
                        {
                            plotables.Add(new PlotableProcess(process, execTime));
                            attQuantun += 1;
                            execTime += 1;

                            Application.Invoke((sender, e) => bar.Fraction = cont / iteracoes);
                            cont += 1;
                        }
                        else
                        {
                            escalonables.Remove(process);
                            break;
                        }
                    }
                }
            }
            Application.Invoke((sender, e) => bar.Fraction = 0);
            return plotables;
        }
    }
}
