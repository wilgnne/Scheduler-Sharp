using Gtk;
using System;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.GtkSharp;
using System.Threading;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.PlotInterface
{
    public partial class PlotInterface
    {
        PlotView view;
        PlotModel model;
        IntervalBarSeries barSeries;
        Container container;

        //PlotAnim Global
        List<PlotableProcess> toPlot;
        int toPlotRealLen;
        bool isPlotable;
        bool paused = false;

        public PlotInterface(Container container)
        {
            this.container = container;
            InitializeABarSeries();
            InitializeAModel(0);

            view = new PlotView();
            container.Add(view);
            view.ShowAll();
            view.Model = model;

            model.Series.Add(barSeries);
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
                    var exporter = new PdfExporter { Width = 800, Height = 600 ,Background = OxyColors.White };
                    exporter.Export(model, stream);
                }
            }
        }

        private void ThredCall()
        {
            for (int i = 0; i < toPlot.Count; i++)
            {
                List<PlotableProcess> listToPlot = toPlot.GetRange(0, i);
                UpdateData(listToPlot);
                while (paused)
                {
                    Thread.Sleep(1);
                }
                Thread.Sleep(250);
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
        public void AnimateData (List<PlotableProcess> processes, bool plotable)
        {
            paused = false;
            isPlotable = plotable;
            toPlot = processes;
            if (plotable)
            {
                toPlotRealLen = processes.Count;
            }
            else
            {
                //Tamanho total
                int len = 0;
                for (int i = 0; i < processes.Count; i++)
                {
                    len += processes[i].Runtime;
                }
                toPlotRealLen = len;
            }
            PlotAnim anim = new PlotAnim(ThredCall);
            anim.StartAnim();
        }

        /// <summary>
        /// Atualizar Modelo de plotagem
        /// </summary>
        /// <param name="processes">Processos a serem plotados.</param>
        public void UpdateData(List<PlotableProcess> processes)
        {
            view.Model = null;
            InitializeAModel(toPlotRealLen);

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
