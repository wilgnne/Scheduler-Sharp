using System;
using OxyPlot;

namespace SchedulerSharp.Schedulers
{
    public class Process
    {
        //Nome do processo
        public string Name { get; private set; }
        //Tempo de chegada do processo
        public int ArrivalTime { get; private set; }
        //Clocks de execução
        public int Runtime { get; private set; }
        //Cor no momento de execução
        public OxyColor RunColor { get; private set; }
        //Cor em espera
        public OxyColor WaitingColor { get; private set; }

        /*Executar um ciclo de clock*/
        public void Run()
        {
            if (Runtime > 0)
                Runtime -= 1;
        }

        /*Gerar cores*/
        public void Colorize()
        {
            Random random = new Random();
            byte r = (byte)random.Next(0, 255);
            byte g = (byte)random.Next(0, 255);
            byte b = (byte)random.Next(0, 255);

            this.WaitingColor = OxyColor.FromArgb(0x7f, r, g, b);
            this.RunColor = OxyColor.FromArgb(0xff, r, g, b);
        }

        public override string ToString()
        {
            string arrivalTime = ArrivalTime.ToString();
            string runTime = Runtime.ToString();
            return "Name:" + Name + ", AttivalTime: " + arrivalTime + ", RunTime: " + runTime;
        }
    }

    public class LogProcess
    {
        //Nome do processo
        public string Name { get; private set; }
        //Tempo em que o processo foi executado
        public int ExecTime { get; private set; }
        //Cor no momento de execução
        public OxyColor Color { get; private set; }

        public LogProcess(Process process, int execTime)
        {
            Name = process.Name;
            ExecTime = execTime;
            Color = process.RunColor;
        }
    }
}
