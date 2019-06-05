using System;
using Gtk;
using SchedulerSharp.Models;
using System.Collections.Generic;
namespace SchedulerSharp.Schedulers
{
    public static class SJF
    {

        public static List<PlotableProcess> Schedulering(List<Process> processes, ProgressBar bar = null)
        {
            //ProgressBar em 0%
            Application.Invoke((sender, e) => bar.Fraction = 0);

            double iteracoes = 0;
            processes.ForEach((obj) => { iteracoes = iteracoes + obj.Runtime; });
            double cont = 0;

            List<PlotableProcess> plotableProcesses = new List<PlotableProcess>();
            List<EscalonableProcess> escalonableProcesses = new List<EscalonableProcess>();
            int execTime;
            for (int i = 0; i < processes.Count; i++)
            {
                escalonableProcesses.Add(new EscalonableProcess(processes[i]));
            }
            execTime = escalonableProcesses[0].ArrivalTime;
            EscalonableProcess aux = escalonableProcesses[0];
            while (escalonableProcesses.Count > 0)
            {
                List<EscalonableProcess> tempo = escalonableProcesses.FindAll((obj) => obj.ArrivalTime <= execTime);

                if (tempo.Count == 0)
                {
                    execTime += 1;
                    continue;
                }

                int indice = escalonableProcesses.FindIndex((obj) => obj.Equals(ShortestJob(tempo, aux)));
                aux = escalonableProcesses[indice];
                escalonableProcesses[indice].Run();
                plotableProcesses.Add(new PlotableProcess(escalonableProcesses[indice], execTime));
                execTime++;

                Application.Invoke((sender, e) => bar.Fraction = cont / iteracoes);
                cont += 1;

                if (escalonableProcesses[indice].Runtime == 0)
                {
                    tempo.RemoveAt(tempo.FindIndex((obj) => obj.Name == escalonableProcesses[indice].Name));
                    escalonableProcesses.RemoveAt(indice);
                    if (escalonableProcesses.Count > 0)
                    {
                        aux = escalonableProcesses[0];
                    }
                }
            }

            //ProgressBar em 0%
            Application.Invoke((sender, e) => bar.Fraction = 0);

            return plotableProcesses;
        }
        public static EscalonableProcess ShortestJob(List<EscalonableProcess> tempo, EscalonableProcess aux)
        {
            for (int i = 0; i < tempo.Count; i++)
            {
                if (aux.Runtime > tempo[i].Runtime)
                {
                    aux = tempo[i];
                }
            }
            return aux;
        }
    }
}
