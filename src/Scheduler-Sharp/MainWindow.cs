using System;
using Gtk;
using OxyPlot.GtkSharp;
using OxyPlot;
using OxyPlot.Series;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        for (int i = 0; i < 3; i++)
        {
            CreatePlot(plotBox);
        }
    }

    protected void CreatePlot (Container container)
    {
        var plotView = new PlotView();
        container.Add(plotView);
        plotView.ShowAll();

        var myModel = new PlotModel { Title = "Example 1" };
        myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        plotView.Model = myModel;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
