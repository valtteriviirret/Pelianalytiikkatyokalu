using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;


/* Dotnet CLI commands for OxyPlot libraries. 

dotnet add package OxyPlot.Core --version 2.1.0

*/


public class PlotTool
{
    private PlotModel plotModel;

    // Initializes a new instance of the PlotTool class.
    public PlotTool(string title, List<double> data, List<DateTime> dates)
    {
        this.Title = title;
        this.Data = data;
        this.Dates = dates;
    }

    // Insert instance variables to a new function series.
    public void DrawPlot()
    {
        plotModel = new PlotModel { Title = this.Title, Background = OxyColors.White };

        var datesDouble = new List<double>();

        for (int i = 1; i <= 10; i++)
        {
            datesDouble.Add(DateTimeAxis.ToDouble(DateTime.Now.AddDays(i)));
        }


        var xAxis = new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "dd/MM/yyyy",
            Title = "Year",
            MinorIntervalType = DateTimeIntervalType.Days,
            IntervalType = DateTimeIntervalType.Days,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.None,
        };

        FunctionSeries fs = new FunctionSeries();


        for (int i = 0; i < datesDouble.Count; i++)
        {
            fs.Points.Add(new DataPoint(datesDouble[i], this.Data[i]));
        }


        plotModel.Series.Add(fs);
        plotModel.Axes.Add(xAxis);
        plotModel.Axes.Add(new LinearAxis());
    }

    // If the plot is created, export as an PNG file.
/*     public void ExportPlot(string fileName, int width = 600, int height = 400)
    {
        if (plotModel != null)
        {
            if (!fileName.Contains(".png"))
                fileName += ".png";

            var pngExport = new PngExporter { Width = width, Height = height };
            pngExport.ExportToFile(plotModel, fileName);
        }
    } */


    public string Title { get; set; }

    public List<double> Data { get; set; }

    public List<DateTime> Dates { get; set; }


}
