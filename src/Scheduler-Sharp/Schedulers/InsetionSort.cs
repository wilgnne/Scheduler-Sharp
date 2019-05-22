using System;
using System.Collections.Generic;
using SchedulerSharp.Models;
namespace SchedulerSharp.Schedulers
{
    public static class InsertionSort
    {
        public static List<Process> InsertionSort_Processes(List<Process> list)
        {
            int i, n, flag, j;
            n = list.Count;
            for (i = 1; i < n; i++)
            {
                Process value = list[i];
                flag = 0;
                for (j = i - 1; j >= 0 && flag != 1;)
                {
                    if (value.ArrivalTime < list[j].ArrivalTime)
                    {
                        list[j + 1] = list[j];
                        j--;
                        list[j + 1] = value;
                    }
                    else flag = 1;
                }
            }
            return list;
        }
    }
}
