using System;
using System.Collections.Generic;
using SchedulerSharp.Models;
namespace SchedulerSharp.Schedulers
{
    public static class FCFS
    {
        public static List<PlotableProcess> Schedulering(List<Process> list)
        {
            int execTime = 0;
            List<PlotableProcess> listPlotable = new List<PlotableProcess>();
            for (int i = 0; i < list.Count; i++)
            {
                EscalonableProcess escalonador = new EscalonableProcess(list[i]);
                while (escalonador.Run())
                {
                    listPlotable.Add(new PlotableProcess(escalonador, execTime));
                    execTime++;
                }
            }

            return listPlotable;
        }
    }
}
