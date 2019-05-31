using System;
using System.Collections.Generic;
using SchedulerSharp.Models;
namespace SchedulerSharp.Schedulers
{
    public static class AverageWaitTime
    {
        public static double WaitTime(List<PlotableProcess> plotableProcesses)
        {
            int waitTime = 0;
            PlotableProcess anterior = plotableProcesses[0];
            for (int i = 0; i < plotableProcesses.Count; i++)
            {
                PlotableProcess atual = plotableProcesses[i];
                if (anterior.Name != atual.Name)
                {
                    waitTime = waitTime + (atual.ExecTime - atual.ArrivalTime);
                    Console.WriteLine("waitTime = {0}, {1} - {2}", waitTime, atual.ExecTime, atual.ArrivalTime);
                }
                anterior = atual;
            }
            return Convert.ToDouble(waitTime) / plotableProcesses.Count;
        }
    }
}
