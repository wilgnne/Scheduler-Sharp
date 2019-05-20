using Gtk;
using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.GtkSharp;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI
{
    public class PlotInterface
    {
        PlotView view;
        PlotModel model;
        IntervalBarSeries barSeries;
        Container container;
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

        public void UpdateData(List<PlotableProcess> processes)
        {
            view.Model = null;
            InitializeAModel(processes.Count);
            SetUtilizationData (processes);
            model.Series.Add(barSeries);
            view.Model = model;
            view.ShowAll();
        }


        private void SetUtilizationData(List<PlotableProcess> processes)
        {
            InitializeABarSeries();
            for (int index = 0; index < processes.Count; index++)
            {
                PlotableProcess process = processes[index];
                IntervalBarItem item = new IntervalBarItem
                {
                    Start = process.ExecTime - 0.5f,
                    End = process.ExecTime + 0.5f,
                    CategoryIndex = index,
                    Title = process.Name,
                    Color = process.RunColor,
                };
                barSeries.Items.Add(item);
            }
        }

        public void InitializeAModel(int LenX)
        {
            model = new PlotModel
            {
                LegendOrientation = LegendOrientation.Vertical,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop
            };


            // Define o eixo X
            CategoryAxis xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Clock's"
            };
            for (int i = 0; i < LenX; i++)
            {
                xAxis.Labels.Add(i.ToString());
            }

            // Adiciona ao eixo do plotModel
            model.Axes.Add(xAxis);


            // Define o eixo Y
            CategoryAxis yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Processos"
            };

            // Adiciona ao eixo do plotModel
            model.Axes.Add(yAxis);
            
        }

        public void InitializeABarSeries()
        {
            barSeries = new IntervalBarSeries
            {
                LabelMargin = 0
            };
        }
    }
}
