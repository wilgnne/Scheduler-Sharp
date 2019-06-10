using System;

namespace SchedulerSharp.Models
{
    public static class DataConverter
    {
        public static string TxtToPRB(string txt)
        {
            string prb;
            string[] aux;
            prb = "[\n";
            string[] dataprocess = txt.Split('\n');
            for (int i = 0; i < dataprocess.Length - 2; i++)
            {
                aux = dataprocess[i].Split(' ');
                prb = prb + "\t{\n";
                prb = prb + "\t\t" + '"' + "Name" + '"' + ": " + '"' + aux[0] + '"' + ",\n";
                prb = prb + "\t\t" + '"' + "ArrivalTime" + '"' + ": " + aux[1] + ",\n";
                prb = prb + "\t\t" + '"' + "Runtime" + '"' + ": " + aux[2] + "\n";
                prb = prb + "\t},\n";
            }
            aux = dataprocess[dataprocess.Length - 2].Split(' ');
            prb = prb + "\t{\n";
            prb = prb + "\t\t" + '"' + "Name" + '"' + ": " + '"' + aux[0] + '"' + ",\n";
            prb = prb + "\t\t" + '"' + "ArrivalTime" + '"' + ": " + aux[1] + ",\n";
            prb = prb + "\t\t" + '"' + "Runtime" + '"' + ": " + aux[2] + "\n";
            prb = prb + "\t}\n]";
            return prb;
        }
    }
}

