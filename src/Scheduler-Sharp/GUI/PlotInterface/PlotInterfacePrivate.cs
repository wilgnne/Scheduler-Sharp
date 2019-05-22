using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.GUI.PlotInterface
{
    public partial class PlotInterface
    {
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

        private void SetUtilizationData (List<Process> processes)
        {
            InitializeABarSeries();
            for (int index = 0; index < processes.Count; index++)
            {
                Process process = processes[index];
                IntervalBarItem item = new IntervalBarItem
                {
                    Start = process.ArrivalTime - 0.5f,
                    End = process.ArrivalTime + process.Runtime + 0.5f,
                    CategoryIndex = index,
                    Title = process.Name,
                    Color = ((PlotableProcess)process).RunColor,
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
