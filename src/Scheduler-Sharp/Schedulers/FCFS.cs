using System;
using System.Collections.Generic;
using SchedulerSharp.Models;

using Gtk;

namespace SchedulerSharp.Schedulers
{
    /// <summary>
    /// Escalonador FCFS
    /// </summary>
    public static class FCFS
    {
        public static List<PlotableProcess> Scheduling(List<Process> list)
        /// <summary>
        /// Escalonar uma lista especifica
        /// </summary>
        /// <returns>A lista escalonada.</returns>
        /// <param name="list">Lista a ser escalonada.</param>
        public static List<PlotableProcess> Schedulering(List<Process> list, ProgressBar bar = null)
        {
            int execTime = list[0].ArrivalTime;
            Application.Invoke((sender, e) => bar.Fraction = 0);
            int execTime = 0;
            List<PlotableProcess> listPlotable = new List<PlotableProcess>();
            Queue<EscalonableProcess> escalonableProcesses = new Queue<EscalonableProcess>();

            double iteracoes = 0;
            list.ForEach((obj) => { iteracoes = iteracoes + obj.Runtime; });
            double cont = 0;

            for (int i = 0; i < list.Count; i++)
            {
                escalonableProcesses.Enqueue(new EscalonableProcess(list[i]));
            }
            while (escalonableProcesses.Count > 0)
            {
                EscalonableProcess escalonable = escalonableProcesses.Dequeue();
                while (escalonable.Run())
                {
                    listPlotable.Add(new PlotableProcess(escalonable, execTime));
                    Application.Invoke((sender, e) => bar.Fraction = cont / iteracoes);
                    cont += 1;

                    listPlotable.Add( new PlotableProcess(escalonador, execTime));
                    execTime++;
                }
            }
            Application.Invoke((sender, e) => bar.Fraction = 0);

            return listPlotable;
        }
    }
}
