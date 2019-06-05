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
    /// Scheduler utils.
    /// </summary>
    public static class SchedulerUtils
    {
        /// <summary>
        /// Gerar uma lista de 0 ate o elemento.
        /// </summary>
        /// <returns>A lista contendo os n-elementos.</returns>
        /// <param name="elements">Numero de elementos.</param>
        public static List<string> Range(int elements)
        {
            List<string> range = new List<string>();
            for (int i = 0; i < elements; i++)
                range.Add(i.ToString());
            return range;
        }
    }
}
