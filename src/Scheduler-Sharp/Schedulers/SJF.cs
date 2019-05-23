using System;
using SchedulerSharp.Models;
using System.Collections.Generic;
namespace SchedulerSharp.Schedulers
{
    public static class SJF
    {

        public static List<PlotableProcess> Scheduling(List<Process> processes)
        {
            List<PlotableProcess> plotableProcesses = new List<PlotableProcess>();
            List<EscalonableProcess> interrupted = new List<EscalonableProcess>();
            List<EscalonableProcess> escalonableProcesses = new List<EscalonableProcess>();
            int execTime = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                escalonableProcesses.Add(new EscalonableProcess(processes[i]));
            }
            execTime = escalonableProcesses[0].ArrivalTime;
            EscalonableProcess aux = escalonableProcesses[0];
            while (escalonableProcesses.Count > 0)
            {
                List<EscalonableProcess> tempo = escalonableProcesses.FindAll((obj) => obj.ArrivalTime <= execTime);
                int indice = escalonableProcesses.FindIndex((obj) => obj.Equals(ShortestJob(tempo, aux)));
                aux = escalonableProcesses[indice];
                escalonableProcesses[indice].Run();
                plotableProcesses.Add(new PlotableProcess(escalonableProcesses[indice], execTime));
                execTime++;
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
