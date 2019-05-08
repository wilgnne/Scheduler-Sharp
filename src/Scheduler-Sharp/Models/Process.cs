using System;
using OxyPlot;

namespace SchedulerSharp.Models
{
    public class Process
    {
        // Nome do processo
        public string Name;
        // Tempo de chegada do processo
        public int ArrivalTime;
        // Clocks de execução
        public int Runtime;

        public Process(string name, int arrivalTime, int runtime)
        {
            Name = name;
            ArrivalTime = arrivalTime;
            Runtime = runtime;
        }

        public override string ToString()
        {
            string arrival = ArrivalTime.ToString();
            string runtime = Runtime.ToString();
            return "Nome: " + Name + ", ArrivalTime: " + arrival + ", Runtime: " + runtime;
        }
    }

    public class PlotableProcess : Process
    {
        // Tempo em que o processo foi executado
        public int ExecTime { get; private set; }
        // Cor no momento de execução
        public OxyColor Color { get; private set; }

        public PlotableProcess(EscalonableProcess process, int execTime)
        :base(process.Name, process.ArrivalTime, process.Runtime)
        {
            Color = process.RunColor;
            ExecTime = execTime;
        }
    }

    public class EscalonableProcess : Process
    {
        // Cor no momento de execução
        public OxyColor RunColor { get; private set; }
        // Cor em espera
        public OxyColor WaitingColor { get; private set; }

        public EscalonableProcess(string name, int arrivalTime, int runtime)
        : base(name, arrivalTime, runtime)
        {
            Colorize();
        }
        public EscalonableProcess (Process process)
        : base (process.Name, process.ArrivalTime, process.Runtime)
        {
            Colorize();
        }

        // Executar um ciclo de clock
        public void Run()
        {
            if (Runtime > 0)
                Runtime -= 1;
        }

        // Gerar cores
        public void Colorize()
        {
            Random random = new Random();
            byte r = (byte)random.Next(0, 255);
            byte g = (byte)random.Next(0, 255);
            byte b = (byte)random.Next(0, 255);

            WaitingColor = OxyColor.FromArgb(0x7f, r, g, b);
            RunColor = OxyColor.FromArgb(0xff, r, g, b);
        }
    }
}
