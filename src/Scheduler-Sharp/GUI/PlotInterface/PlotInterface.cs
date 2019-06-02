using Gtk;
using System;
using System.Linq;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.GtkSharp;
using System.Threading;
using System.Collections.Generic;

using SchedulerSharp.Models;
using SchedulerSharp.Schedulers;

namespace SchedulerSharp.GUI.PlotInterface
{
    public partial class PlotInterface
    {
        PlotView view;
        PlotModel model;
        Container container;

        //PlotAnim Global
        List<PlotableProcess> toPlot;
        List<String> xLabel;
        List<String> yLabel;
        bool isPlotable;
        bool paused = false;


        public PlotInterface(Container container)
        {
            this.container = container;
            InitializeABarSeries();
            //InitializeAModel();

            view = new PlotView();
            container.Add(view);
            view.ShowAll();
            view.Model = model;

            //model.Series.Add(barSeries);
        }

        /// <summary>
        /// Exportar o modelo atual para SVG
        /// </summary>
        public void ExportSVG()
        {
            if (GTKUtils.ShowFileChooser(out string path, ".svg", "Salvar Como...", "Salvar"))
            {
                using (var stream = File.Create(path))
                {
                    var exporter = new SvgExporter { Width = 800, Height = 600 };
                    exporter.Export(model, stream);
                }
            }
        }

        /// <summary>
        /// Exportar o modelo atual para PNG
        /// </summary>
        public void ExportPNG()
        {
            if (GTKUtils.ShowFileChooser(out string path, ".png", "Salvar Como...", "Salvar"))
            {
                using (var stream = File.Create(path))
                {
                    var exporter = new PngExporter { Width = 800, Height = 600, Background = OxyColors.White, Resolution = 25 };
                    exporter.Export(model, stream);
                }
            }
        }

        /// <summary>
        /// Pausar animação
        /// </summary>
        public void Pause ()
        {
            paused = !paused;
        }

        /// <summary>
        /// Animar a entrada
        /// </summary>
        /// <param name="processes">Processos a ser plotados.</param>
        /// <param name="plotable">Se for <c>true</c> sera considerado um objeto plotavel.</param>
        public void AnimateData (List<PlotableProcess> processes, bool plotable, string Title = "Title")
        {
            paused = false;
            isPlotable = plotable;
            toPlot = processes;
            if (plotable)
            {
                yLabel = toPlot.ConvertAll(x => x.Name);
                yLabel = yLabel.Distinct().ToList();
                xLabel = SchedulerUtils.Range(processes.Count);
            }
            else
            {
                List<int> ranges = toPlot.ConvertAll(x => (x.Runtime + x.ArrivalTime));
                ranges.Sort();
                xLabel = SchedulerUtils.Range(ranges[ranges.Count - 1] + 1);
                yLabel = toPlot.ConvertAll(x => x.Name);
            }

            UpdateData(toPlot, xLabel, yLabel, Title);
        }

        /// <summary>
        /// Atualizar Modelo de plotagem
        /// </summary>
        /// <param name="processes">Processos a serem plotados.</param>
        public void UpdateData(List<PlotableProcess> processes, List<string> xLabel, List<string> yLabel, string Title)
        {
            view.Model = null;
            InitializeAModel(xLabel, yLabel, Title);

            if (isPlotable)
            {
                SetUtilizationData(processes);
            }
            else
            {
                SetUtilizationData(processes.ConvertAll(x => (Process)x));
            }

            model.Series.Add(barSeries);
            view.Model = model;
            view.ShowAll();
        }
    }
}
