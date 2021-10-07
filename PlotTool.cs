using System;
using System.IO;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.SkiaSharp;
using OxyPlot.Series;
using OxyPlot.Axes;


/* Dotnet CLI commands for OxyPlot libraries. 

dotnet add package OxyPlot.Core --version 2.1.0
dotnet add package OxyPlot.SkiaSharp --version 2.1.0
dotnet add package System.IO.FileSystem.AccessControl --version 5.0.0

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


        var functionSeries = new FunctionSeries();
        var datesDouble = new List<double>();


        for (int i = 0; i < this.Dates.Count; i++)
        {
            datesDouble.Add(DateTimeAxis.ToDouble(this.Dates[i]));
            functionSeries.Points.Add(new DataPoint(datesDouble[i], this.Data[i]));
        }


        plotModel.Series.Add(functionSeries);
        plotModel.Axes.Add(xAxis);
        plotModel.Axes.Add(new LinearAxis());
    }

    public void ExportPng(string fileName, int width = 600, int height = 400)
    {
        string path = Directory.GetCurrentDirectory();

        if (plotModel != null)
            using (FileStream fs = File.Create(path))
                PngExporter.Export(plotModel, path, width, height);
    }

    public string Title { get; set; }

    public List<double> Data { get; set; }

    public List<DateTime> Dates { get; set; }


}
