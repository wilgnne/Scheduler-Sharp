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

        public PlotAnim (delegateFuc fuc)
        {
            thr = new Thread(new ThreadStart(fuc));
        }

        public void StartAnim ()
        {
            thr.Start();
        }

        public void StopAnim ()
        {
            thr.Abort();
        }
    }
}
