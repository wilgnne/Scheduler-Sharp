using System;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.Schedulers
{
    public static class RR
    {
        public static List<PlotableProcess> Schedulering(List<Process> processes, int quantun)
        {
            List<PlotableProcess> plotables = new List<PlotableProcess>();

            List<EscalonableProcess> escalonables = processes.ConvertAll((input) => new EscalonableProcess(input));

            int execTime = 0;
            while (escalonables.Count != 0)
            {
                List<EscalonableProcess> attTime = escalonables.FindAll((obj) => obj.ArrivalTime <= execTime);
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
                        }
                        else
                        {
                            escalonables.Remove(process);
                            break;
                        }
                    }
                }
            }

            return plotables;
        }

        public static List<PlotableProcess> NewScheduling(List<Process> processes, int quantun)
        {
            List<PlotableProcess> plotables = new List<PlotableProcess>();

            List<EscalonableProcess> escalonables = processes.ConvertAll((input) => new EscalonableProcess(input));

            int execTime = 0;
            while (escalonables.Count != 0)
            {
                List<EscalonableProcess> attTime = escalonables.FindAll((obj) => obj.ArrivalTime <= execTime);
                execTime = attTime.Count == 0 ? execTime + 1 : execTime;

                for(int i = 0; i < attTime.Count; i++)
                {
                    EscalonableProcess process = attTime[i];
                    int attQuantun = 0;
                    while (attQuantun < quantun)
                    {
                        if (process.Run())
                        {
                            plotables.Add(new PlotableProcess(process, execTime));
                            attQuantun += 1;
                            execTime += 1;

                            attTime = escalonables.FindAll((obj) => obj.ArrivalTime <= execTime);
                        }
                        else
                        {
                            escalonables.Remove(process);
                            break;
                        }
                    }
                }

                foreach (EscalonableProcess process in attTime)
                {

                }
            }

            return plotables;
        }
    }
}
