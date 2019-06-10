using System;
using System.Collections.Generic;
using System.Threading;
using Gtk;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.PlotInterface
{
    public delegate void delegateFuc();
    public class PlotAnim
    {
        Thread thr;

        public PlotAnim (delegateFuc fuc, string name)
        {
            thr = new Thread(new ThreadStart(fuc))
            {
                Name = name
            };
        }

        public void StartAnim ()
        {
            Console.WriteLine("entry {0}", thr.Name);
            if (thr.ThreadState == ThreadState.Running)
            {
                thr.Abort();
                Console.WriteLine("Aborted: {0}", thr.Name);
            }
            thr.Start();
        }

        public void StopAnim ()
        {
            thr.Abort();
        }
    }
}
