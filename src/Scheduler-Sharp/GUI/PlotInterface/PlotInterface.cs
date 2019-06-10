using Gtk;
using System;
using System.Linq;
using System.IO;
using OxyPlot;
using OxyPlot.GtkSharp;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.PlotInterface
{
    public delegate void AnimCallBack(PlotableProcess inExec);

    public partial class PlotInterface
    {
        PlotView view;
        PlotModel model;
        Container container;

        //PlotAnim Global
        List<PlotableProcess> toPlot;
        int rangePlot;

        public List<string> YLabel { get; private set; }
        bool isPlotable;
        bool paused, autoIncr = true;
        string title;

        AnimCallBack callBack;

        uint tagTrh;


        public PlotInterface(Container container, AnimCallBack callBack = null)
        {
            this.container = container;
            InitializeAIntervalBarSeries();
            //InitializeAModel();

            view = new PlotView();
            container.Add(view);
            view.ShowAll();
            view.Model = model;

            this.callBack = callBack;

            tagTrh = GLib.Timeout.Add(60, AnimationThr);
            

            //model.Series.Add(barSeries);
        }

        /// <summary>
        /// Exportar o modelo atual para SVG
        /// </summary>
        public void ExportSVG(string path)
        {
            using (var stream = File.Create(path))
            {
                var exporter = new SvgExporter { Width = 1920, Height = 1080 };
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Exportar o modelo atual para PNG
        /// </summary>
        public void ExportPNG(string path)
        {
            using (var stream = File.Create(path))
            {
                var exporter = new PngExporter { Width = 1920, Height = 1080, Background = OxyColors.White, Resolution = 125 };
                exporter.Export(model, stream);
            }
        }

        public void CutListEndPlot (bool isAnim)
        {
            List<PlotableProcess> processes = toPlot.GetRange(0, rangePlot);
            if (processes.Count > 0)
            {
                processes[processes.Count - 1].attColor = processes[processes.Count - 1].RunColor;

                callBack?.Invoke(processes[processes.Count - 1]);

                UpdateData(processes, YLabel, title, isAnim);
                processes[processes.Count - 1].attColor = processes[processes.Count - 1].WaitingColor;
            }
        }

        public bool AnimationThr ()
        {
            if (toPlot != null)
            {
                if (rangePlot < toPlot.Count && rangePlot >= 0)
                {
                    if (paused)
                    {
                        return true;
                    }
                    if (autoIncr)
                    {
                        rangePlot += 1;
                        CutListEndPlot(true);
                    }
                }
            }
            return true;
        }

        public void Play()
        {
            rangePlot = 0;
            paused = false;
            autoIncr = true;
        }

        /// <summary>
        /// Pausar animação
        /// </summary>
        public void Pause ()
        {
            //GLib.Source.Remove(tagTrh);
            if (toPlot != null)
            {
                if (rangePlot < toPlot.Count)
                {
                    paused = false;
                    autoIncr = !autoIncr;
                }
                else
                {
                    paused = true;
                    CutListEndPlot(false);
                }
            }
        }

        public void Next()
        {
            autoIncr = false;
            paused = false;
            if (rangePlot < toPlot.Count)
                rangePlot = rangePlot + 1;
            else
                rangePlot = 0;

            CutListEndPlot(false);
        }

        public void Preview()
        {
            autoIncr = false;
            paused = false;
            if (rangePlot > 0)
                rangePlot = rangePlot - 1;
            else
                rangePlot = toPlot.Count - 1;

            CutListEndPlot(false);
        }

        public void AnimateData (List<double> waitTime, List<double> turnaroundTime, List<double> responseTime, List<string> text, string Title = "Title")
        {
            UpdateData(waitTime, turnaroundTime, responseTime, text, Title);
        }

        public void UpdateData(List<double> waitTime, List<double> turnaroundTime, List<double> responseTime, List<string> text, string Title = "Title")
        {
            view.Model = null;
            InitializeAModel(text, Title, invert:true);

            SetUtilizationData(waitTime, turnaroundTime, responseTime, text);
            view.Model = model;
            view.ShowAll();
        }


        /// <summary>
        /// Animar a entrada
        /// </summary>
        /// <param name="processes">Processos a ser plotados.</param>
        /// <param name="plotable">Se for <c>true</c> sera considerado um objeto plotavel.</param>
        public void AnimateData(List<PlotableProcess> processes, bool plotable, string Title = "Title", List<string> yLabel = null, bool isAnim = false)
        {
            paused = false;
            isPlotable = plotable;
            toPlot = processes;
            title = Title;
            rangePlot = toPlot.Count;
            if (plotable)
            {
                if (yLabel == null)
                {
                    YLabel = toPlot.ConvertAll(x => x.name);
                    YLabel = YLabel.Distinct().ToList();
                }
                else
                {
                    YLabel = yLabel;
                }
            }
            else
            {
                List<int> ranges = toPlot.ConvertAll(x => (x.runtime + x.arrivalTime));
                ranges.Sort();
                YLabel = toPlot.ConvertAll(x => x.name);
            }

            UpdateData(toPlot, YLabel, Title, isAnim);
        }

        /// <summary>
        /// Atualizar Modelo de plotagem
        /// </summary>
        /// <param name="processes">Processos a serem plotados.</param>
        private void UpdateData(List<PlotableProcess> processes, List<string> yLabels, string Title, bool isAnim = false)
        {
            view.Model = null;
            InitializeAModel(yLabels, Title, isAnim: isAnim);
            InitializeAIntervalBarSeries(isAnim);

            if (isPlotable)
            {
                SetUtilizationData(processes);
            }
            else
            {
                SetUtilizationData(processes.ConvertAll(x => (Process)x));
            }

            model.Series.Add(intervalBarSeries);
            view.Model = model;
            view.ShowAll();
        }
    }
}
