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
        /// <summary>
        /// Escalonar uma lista especifica
        /// </summary>
        /// <returns>A lista escalonada.</returns>
        /// <param name="list">Lista a ser escalonada.</param>
        public static List<PlotableProcess> Schedulering(List<Process> list, ProgressBar bar = null)
        {
            Application.Invoke((sender, e) => bar.Fraction = 0);
            int execTime = 0;
            List<PlotableProcess> listPlotable = new List<PlotableProcess>();

            double iteracoes = 0;
            list.ForEach((obj) => { iteracoes = iteracoes + obj.Runtime; });
            double cont = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ArrivalTime > execTime)
                    execTime = list[i].ArrivalTime;

                EscalonableProcess escalonador = new EscalonableProcess(list[i]);
                while (escalonador.Run())
                {
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
