using System;
using OxyPlot;

namespace SchedulerSharp.Models
{
    public struct LogProcess
    {
        public string Name;
        public int execTime;

        public LogProcess (PlotableProcess process)
        {
            Name = process.name;
            execTime = process.ExecTime;
        }
    }

    /// <summary>
    /// Classe base para os processos
    /// </summary>
    public class Process
    {
        // Nome do processo
        public string name;
        // Tempo de chegada do processo
        public int arrivalTime;
        // Clocks de execução
        public int runtime;

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="T:SchedulerSharp.Models.Process"/> .
        /// </summary>
        /// <param name="name">Nome do processo.</param>
        /// <param name="arrivalTime">Tempo de chegada.</param>
        /// <param name="runtime">Tempo de execução.</param>
        public Process(string name, int arrivalTime, int runtime)
        {
            this.name = name;
            this.arrivalTime = arrivalTime;
            this.runtime = runtime;
        }

        public override string ToString()
        {
            return "Nome: " + name + 
                ", ArrivalTime: " + arrivalTime.ToString() + 
                ", Runtime: " + runtime.ToString();
        }
    }

    /// <summary>
    /// Processo para ser exibido
    /// </summary>
    public class PlotableProcess : EscalonableProcess
    {
        public OxyColor attColor;
        // Tempo em que o processo foi executado
        public int ExecTime { get; private set; }

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="T:SchedulerSharp.Models.PlotableProcess"/> .
        /// </summary>
        /// <param name="process">Processo base.</param>
        /// <param name="execTime">Tempo em que foi executado.</param>
        public PlotableProcess(EscalonableProcess process, int execTime)
        : base(process.name, process.arrivalTime, process.runtime)
        {
            ExecTime = execTime;
            attColor = WaitingColor;
        }

        public PlotableProcess(string Name, int ExecTime)
            : base(Name, 0, 0)
        {
            this.ExecTime = ExecTime;
            Colorize();
            attColor = WaitingColor;
        }
    }

    /// <summary>
    /// Processo para ser escalonado
    /// </summary>
    public class EscalonableProcess : Process
    {
        // Cor no momento de execução
        public OxyColor RunColor { get; private set; }
        // Cor em espera
        public OxyColor WaitingColor { get; private set; }

        public Process Source { get; private set; }

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="T:SchedulerSharp.Models.EscalonableProcess"/> .
        /// </summary>
        /// <param name="name">Nome do processo.</param>
        /// <param name="arrivalTime">Tempo de chegada.</param>
        /// <param name="runtime">Tempo de execução.</param>
        public EscalonableProcess(string name, int arrivalTime, int runtime)
        : base(name, arrivalTime, runtime)
        {
            Colorize();
        }

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="T:SchedulerSharp.Models.EscalonableProcess"/> .
        /// </summary>
        /// <param name="process">Processo base.</param>
        public EscalonableProcess(Process process)
        : base(process.name, process.arrivalTime, process.runtime)
        {
            Source = process;
            Colorize();
        }

        /// <summary>
        /// Executar uma instancia de clock
        /// </summary>
        public bool Run()
        {
            if (runtime > 0)
            {
                runtime -= 1;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gerar cores para o processo
        /// </summary>
        public void Colorize()
        {
            Random random = new Random(StringToInt(name));
            byte r = (byte)random.Next(50, 255);
            byte g = (byte)random.Next(50, 255);
            byte b = (byte)random.Next(50, 255);

            WaitingColor = OxyColor.FromArgb(255, r, g, b);
            RunColor = OxyColor.FromArgb(255, (byte)(r - 50), (byte)(g - 50), (byte)(b - 50));
        }

        public static int StringToInt(string str)
        {
            char[] vs = str.ToCharArray();
            int acc = 0;
            for(int i= 0; i < vs.Length; i++)
            {
                acc += Convert.ToInt32(Convert.ToByte(vs[i]));
            }
            return acc;
        }
    }
}
