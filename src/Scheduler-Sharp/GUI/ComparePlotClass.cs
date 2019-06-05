using System;
using Gtk;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
namespace SchedulerSharp.GUI
{
    public class ComparePlotClass
    {
        public PlotView plotView;
        public PlotModel model;
        public BarSeries barSeries;
        public ComparePlotClass(Container container)
        {
            plotView = new PlotView();
            container.Add(plotView);
            plotView.ShowAll();

            InitializeModel();
        }

        private void InitializeModel (string Title = "")
        {
            model = new PlotModel
            {
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPosition = LegendPosition.BottomCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendBorderThickness = 0,
                Title = Title
            };
        }

        public void InitializeABarSeries()
        {
            barSeries = new BarSeries
            {
                LabelMargin = 0,
            };
            /*
            if (isPlotable)
                barSeries.Title = "Tempo medio de espera: 30clk's";
                */
        }
    }
}
