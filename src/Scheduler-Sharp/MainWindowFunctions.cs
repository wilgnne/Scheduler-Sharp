using Gtk;
using System;
using System.Collections.Generic;

using SchedulerSharp.Models;
using SchedulerSharp.Schedulers;
using SchedulerSharp.GUI.PlotInterface;

public partial class MainWindow : Gtk.Window
{
    protected List<PlotableProcess> GetProcessFromCreation()
    {
        List<PlotableProcess> plotables = new List<PlotableProcess>();
        List<EscalonableProcess> escalonables = new List<EscalonableProcess>();

        ProcessCompare compare = new ProcessCompare();

        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);

        foreach (Process process in li)
        {
            EscalonableProcess eProcess = new EscalonableProcess(process);
            plotables.Add(new PlotableProcess(eProcess, 0));
        }

        return plotables;
    }

    void ThreRR()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        rr = RR.Schedulering(li, (int)quantumScale.Value, progressRR);

        waitTimeRR = Benchmark.WaitTime(rr, li.Count);
        turnarondTimeRR = Benchmark.TurnarondTime(rr, li);
        responseTimeRR = Benchmark.ResponseTime(rr, li);

        Application.Invoke((sender, e) => OnSelectScheduler(SchedulerCombobox, e));
    }

    void ThreSJF()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        sjf = SJF.Schedulering(li, progressRR);

        waitTimeSJF = Benchmark.WaitTime(sjf, li.Count);
        turnarondTimeSJF = Benchmark.TurnarondTime(sjf, li);
        responseTimeSJF = Benchmark.ResponseTime(sjf, li);

        Application.Invoke((sender, e) => OnSelectScheduler(SchedulerCombobox, e));
    }

    void ThreFCFS()
    {
        ProcessCompare compare = new ProcessCompare();
        List<Process> li = new List<Process>(creationController.GetItens().ToArray());
        li.Sort(compare);
        fcfs = FCFS.Schedulering(li, progressFCFS);

        waitTimeFCFS = Benchmark.WaitTime(fcfs, li.Count);
        turnarondTimeFCFS = Benchmark.TurnarondTime(fcfs, li);
        responseTimeFCFS = Benchmark.ResponseTime(fcfs, li);

        Application.Invoke((sender, e) => OnSelectScheduler(SchedulerCombobox, e));
    }

    protected void MediaButtonSensitibe(bool sensitive)
    {
        nextButton.Sensitive = sensitive;
        previewButton.Sensitive = sensitive;
        playButton.Sensitive = sensitive;
        pauseButton.Sensitive = sensitive;
    }

    protected void ChangeDirCallback()
    {
        SchedulerCombobox.Sensitive = true;
        quantumScale.Sensitive = true;
        Console.WriteLine("Atualizado com susses");
        toPlot = GetProcessFromCreation();
        plot.AnimateData(toPlot, false, "Entrada");

        PlotAnim fcfsAnim = new PlotAnim(ThreFCFS, "FCFS");
        fcfsAnim.StartAnim();

        PlotAnim rrAnim = new PlotAnim(ThreRR, "RR");
        rrAnim.StartAnim();

        PlotAnim sjfAnim = new PlotAnim(ThreSJF, "SJF");
        sjfAnim.StartAnim();
    }

    protected void AtualizeAnimCallBack(PlotableProcess process)
    {
        List<PlotableProcess> list = GetProcessFromCreation();
        list.Find((obj) => obj.Name == process.Name).attColor = list.Find((obj) => obj.Name == process.Name).RunColor;
        plot.AnimateData(list, false, "Entrada", isAnim:true);
        list.Find((obj) => obj.Name == process.Name).attColor = list.Find((obj) => obj.Name == process.Name).WaitingColor;
    }

}