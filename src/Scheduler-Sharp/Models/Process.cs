using System;
namespace SchedulerSharp.Models
{
    public class Process
    {
        public string Name;
        public int ArrivalTime;
        public int Runtime;

        public Process(string name, int arrivalTime, int runtime)
        {
            Name = name;
            ArrivalTime = arrivalTime;
            Runtime = runtime;
        }

        public Process (){ }

        public override string ToString()
        {
            string arrival = ArrivalTime.ToString();
            string runtime = Runtime.ToString();
            return "Nome: " + Name + ", ArrivalTime: " + arrival + ", Runtime: " + runtime;
        }
    }

    public class PlotableProcess : Process
    {
        public PlotableProcess(string name, int arrivalTime, int runtime)
        :base(name, arrivalTime, runtime)
        {

        }
    }
}
