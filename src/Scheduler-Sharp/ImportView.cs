using Gtk;
using System;
using System.Collections.Generic;

using SchedulerSharp.GUI.PlotInterface;
using SchedulerSharp.Schedulers;
using SchedulerSharp.Models;

namespace SchedulerSharp
{
    public partial class ImportView : Window
    {
        public PlotInterface escalonadPlot;
        public List<PlotableProcess> rr, sjf, fcfs;
        public double quantum;

        public ImportView(SchedulersResult result) :
                base(WindowType.Toplevel)
        {
            Build();
            fcfs = result.FCFS.ConvertAll((input) => { return new PlotableProcess(input.Name, input.execTime); });
            sjf = result.SJF.ConvertAll((input) => { return new PlotableProcess(input.Name, input.execTime); });
            rr = result.RR.ConvertAll((input) => { return new PlotableProcess(input.Name, input.execTime); });
            quantum = result.Quantum;

            escalonadPlot = new PlotInterface(vbox1);
            escalonadPlot.AnimateData(fcfs, true, "FCFS");
        }

        protected void OnSelectScheduler(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).ActiveText)
            {
                case "SJF":
                    if (sjf != null)
                        escalonadPlot.AnimateData(sjf, true, "SJF");
                    break;
                case "RR":
                    if (rr != null)
                        escalonadPlot.AnimateData(rr, true, "Round Robin / Quantum: " + ((int)quantum).ToString());
                    break;
                case "FCFS":
                    if (fcfs != null)
                        escalonadPlot.AnimateData(fcfs, true, "FCFS");
                    break;
            }
        }
    }
}
