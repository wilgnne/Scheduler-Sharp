using System;
namespace SchedulerSharp.Models
{
public static class DataConverter
        {
            public static string TxttoJSON(string txt)
            {
                string json;
                string[] aux;
                json = "[\n";
                string[] dataprocess = txt.Split('\n');
                for (int i = 0; i < dataprocess.Length - 2; i++)
                {
                    aux = dataprocess[i].Split(' ');
                    json = json + "\t{\n";
                    json = json + "\t\t" + '"' + "Name" + '"' + ": " + '"' + aux[0] + '"' + ",\n";
                    json = json + "\t\t" + '"' + "ArrivalTime" + '"' + ": " + aux[1] + ",\n";
                    json = json + "\t\t" + '"' + "Runtime" + '"' + ": " + aux[2] + "\n";
                    json = json + "\t},\n";
                }
                aux = dataprocess[dataprocess.Length - 2].Split(' ');
                json = json + "\t{\n";
                json = json + "\t\t" + '"' + "Name" + '"' + ": " + '"' + aux[0] + '"' + ",\n";
                json = json + "\t\t" + '"' + "ArrivalTime" + '"' + ": " + aux[1] + ",\n";
                json = json + "\t\t" + '"' + "Runtime" + '"' + ": " + aux[2] + "\n";
                json = json + "\t}\n]";
                return json;
            }
        }
        }

