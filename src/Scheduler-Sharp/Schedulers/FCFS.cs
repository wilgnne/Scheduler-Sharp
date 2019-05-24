using System;
using System.Collections.Generic;
using SchedulerSharp.Models;
namespace SchedulerSharp.Schedulers
{
    public static class FCFS
    {
        public static List<PlotableProcess> Scheduling(List<Process> list)
        {
            int execTime = list[0].ArrivalTime;
            List<PlotableProcess> listPlotable = new List<PlotableProcess>();
            Queue<EscalonableProcess> escalonableProcesses = new Queue<EscalonableProcess>();
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
                    execTime++;
                }
            }
            return listPlotable;
        }
    }
}
