using System;
using System.Collections.Generic;
using SchedulerSharp.Models;

namespace SchedulerSharp.Schedulers
{
    public struct SchedulersResult
    {
        public List<LogProcess> FCFS, SJF;
        public double Quantum;
        public List<LogProcess> RR;
    }

    /// <summary>
    /// Classe de Benchmark de Escalonadores de Processos
    /// </summary>
    public static class Benchmark
    {
        /// <summary>
        /// Tempo de Espera.
        /// </summary>
        /// <returns>Tempo de espera medio dos processos.</returns>
        /// <param name="plotableProcesses">Lista escalonada.</param>
        /// <param name="totalProcess">Total de processos.</param>
        public static double WaitTime(List<PlotableProcess> plotableProcesses, int totalProcess)
        {
            double waitTime = 0;
            if (totalProcess > 0)
            {
                PlotableProcess anterior = plotableProcesses[0];
                for (int i = 0; i < plotableProcesses.Count; i++)
                {
                    PlotableProcess atual = plotableProcesses[i];
                    if (anterior.name != atual.name)
                    {
                        List<PlotableProcess> aux = plotableProcesses.GetRange(0, i);
                        PlotableProcess ultimaExec = aux.FindLast((obj) => obj.name == atual.name);
                        double wait = 0;
                        if (ultimaExec != null)
                        {
                            wait = (atual.ExecTime - ultimaExec.ExecTime) - 1;
                        }
                        else
                        {
                            wait = (atual.ExecTime - atual.arrivalTime);
                        }

                        waitTime += wait;
                    }
                    anterior = atual;
                }
            }

            return waitTime / Convert.ToDouble(totalProcess);
        }

        /// <summary>
        /// Tempo de Vida.
        /// </summary>
        /// <returns>Tempo medio de vida dos processos.</returns>
        /// <param name="plotableProcesses">Processos escalonados.</param>
        /// <param name="processes">Processos ha escalonar.</param>
        public static double TurnarondTime(List<PlotableProcess> plotableProcesses, List<Process> processes)
        {
            int soma = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                PlotableProcess aux = plotableProcesses.FindLast((obj) => obj.name.Equals(processes[i].name));
                soma = soma + (aux.ExecTime - aux.arrivalTime);
            }
            return Convert.ToDouble(soma) / processes.Count;
        }

        public static double ResponseTime(List<PlotableProcess> plotableProcesses, List<Process> processes)
        {
            int soma = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                PlotableProcess aux = plotableProcesses.Find((obj) => obj.name.Equals(processes[i].name));
                soma = soma + (aux.ExecTime - aux.arrivalTime);
            }
            return Convert.ToDouble(soma) / processes.Count;
        }
    }
}
