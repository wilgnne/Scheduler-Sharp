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
            //ProgressBar em 0%
            Application.Invoke((sender, e) => bar.Fraction = 0);
            int execTime = 0;
            List<PlotableProcess> listPlotable = new List<PlotableProcess>();
            Queue<EscalonableProcess> escalonableProcesses;

            //Calculo de procentagem para a progessBar
            double iteracoes = 0;
            list.ForEach((obj) => { iteracoes = iteracoes + obj.Runtime; });
            double cont = 0;

            //Empilihando Processos
            escalonableProcesses = new Queue<EscalonableProcess>(list.ConvertAll(
                (input) => { return new EscalonableProcess(input); }));

            //Enquanto houver processos
            while (escalonableProcesses.Count > 0)
            {
                //Desenpilhamos o processo
                EscalonableProcess escalonable = escalonableProcesses.Dequeue();
                //Viajamos para o futuro caso o processo esteja em um execTime a frente
                if (escalonable.ArrivalTime > execTime)
                    execTime = escalonable.ArrivalTime;
                //E o executamos
                while (escalonable.Run())
                {
                    Application.Invoke((sender, e) => bar.Fraction = cont / iteracoes);
                    cont += 1;

                    listPlotable.Add( new PlotableProcess(escalonable, execTime));
                    execTime++;
                }
            }
            Application.Invoke((sender, e) => bar.Fraction = 0);

            return listPlotable;
        }
    }
}
