using System;
using System.Collections.Generic;
using SchedulerSharp.Models;

namespace SchedulerSharp.Schedulers
{
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
            PlotableProcess anterior = plotableProcesses[0];
            for (int i = 0; i < plotableProcesses.Count; i++)
            {
                PlotableProcess atual = plotableProcesses[i];
                if (anterior.Name != atual.Name)
                {
                    List<PlotableProcess> aux = plotableProcesses.GetRange(0, i);
                    PlotableProcess ultimaExec =  aux.FindLast((obj) => obj.Name == atual.Name);
                    double wait = 0;
                    if (ultimaExec != null)
                    {
                        wait = (atual.ExecTime - ultimaExec.ExecTime) - 1;
                    }
                    else
                    {
                        wait = (atual.ExecTime - atual.ArrivalTime);
                    }

                    waitTime += wait;
                }
                anterior = atual;
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
                PlotableProcess aux = plotableProcesses.FindLast((obj) => obj.Name.Equals(processes[i].Name));
                soma = soma + (aux.ExecTime - aux.ArrivalTime);
                Console.WriteLine("Exectime {0} --- ArrivalTime {1}", aux.ExecTime, aux.ArrivalTime);
            }
            return Convert.ToDouble(soma) / processes.Count;
        }

        public static double ResponseTime(List<PlotableProcess> plotableProcesses, List<Process> processes)
        {
            int soma = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                PlotableProcess aux = plotableProcesses.Find((obj) => obj.Name.Equals(processes[i].Name));
                soma = soma + (aux.ExecTime - aux.ArrivalTime);
                Console.WriteLine("Exectime {0} --- ArrivalTime {1} ---- Name {2}", aux.ExecTime, aux.ArrivalTime, aux.Name);
            }
            return Convert.ToDouble(soma) / processes.Count;
        }
    }
}
