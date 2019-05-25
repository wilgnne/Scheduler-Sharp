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
        IntervalBarSeries barSeries;

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
                    CategoryIndex = yLabel.IndexOf(process.Name),
                    //Title = process.Name,
                    Color = process.RunColor,
                };
                barSeries.Items.Add(item);
            }
        }

        private void SetUtilizationData(List<Process> processes)
        {
            InitializeABarSeries();
            for (int index = 0; index < processes.Count; index++)
            {
                Process process = processes[index];
                IntervalBarItem item = new IntervalBarItem
                {
                    Start = process.ArrivalTime,
                    End = process.ArrivalTime + process.Runtime,
                    CategoryIndex = index,
                    //Title = process.Name,
                    Color = ((PlotableProcess)process).RunColor,
                };
                barSeries.Items.Add(item);
            }
        }

            public void InitializeAModel(List<string> xLabels, List<string> yLabels)
        {
            model = new PlotModel
            {
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPosition = LegendPosition.BottomCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendBorderThickness = 0,
                Title = "Titulo"
            };

            /*
            // Define o eixo X
            CategoryAxis xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Clock's"
            };
            for (int i = 0; i < xLabels.Count; i++)
            {
                xAxis.Labels.Add(xLabels[i]);
            }
            */

            LinearAxis xAxis = new LinearAxis()
            {
                Title = "Clock's",
                Minimum = 0,
                Position = AxisPosition.Bottom,
            };

            // Adiciona ao eixo do plotModel
            model.Axes.Add(xAxis);

            /*
            LinearAxis yAxis = new LinearAxis()
            {
                Title = "Clock's",
                Minimum = 0,
                Position = AxisPosition.Left,
            };
            */
            // Define o eixo Y
            CategoryAxis yAxis = new CategoryAxis
            {
                Title = "Processos",
                Minimum = 0,
                Position = AxisPosition.Left,
            };
            yAxis.Labels.AddRange(yLabel);

            // Adiciona ao eixo do plotModel
            model.Axes.Add(yAxis);
        }

        public void InitializeABarSeries()
        {
            barSeries = new IntervalBarSeries
            {
                LabelMargin = 0,
            };
            if (isPlotable)
                barSeries.Title = "Tempo medio de espera: 30clk's";
        }
    }
}
