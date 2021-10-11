using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.ImageSharp;
using OxyPlot.Series;
using OxyPlot.Axes;

/* OxyPlot dependencies. 
dotnet add package OxyPlot.Core --version 2.1.0
dotnet add package OxyPlot.ImageSharp --version 2.1.0
*/

public class PlotTool
{
    // linear bar series constructor.
    public PlotTool(string title, List<float> lineData, List<DateTime> lineDates)
    {
        this.Title = title;
        this.LineValues = lineData;
        this.LineDates = lineDates;
    }

    // pie chart series constructor.
    public PlotTool(string title, List<string> pieStrings, List<float> pieValues)
    {
        this.Title = title;
        this.PieStrings = pieStrings;
        this.PieValues = pieValues;
    }

    // returns plot model with linear bar values and axes
    PlotModel LineBarSeries()
    {

        var plotModel = new PlotModel { Title = this.Title, Background = OxyColors.White, DefaultFont = "Roboto" };

        var xAxis = new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "dd.MM.yyyy",
            MinorIntervalType = DateTimeIntervalType.Days,
            IntervalType = DateTimeIntervalType.Days,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.None,
        };

        var lineBarSeries = new LinearBarSeries() { BarWidth = 50 };

        for (int i = 0; i < this.LineDates.Count; i++)
            lineBarSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.LineDates[i]), this.LineValues[i]));

        plotModel.Series.Add(lineBarSeries);
        plotModel.Axes.Add(xAxis);
        plotModel.Axes.Add(new LinearAxis());

        return plotModel;
    }

    // returns plot model with pie chart strings and values.
    PlotModel PieChartSeries()
    {
        var plotModel = new PlotModel { Title = this.Title, Background = OxyColors.White, DefaultFont = "Roboto" };

        var ps = new PieSeries
        {
            StrokeThickness = 2.0,
            InsideLabelPosition = 0.8,
            AngleSpan = 360,
            StartAngle = 0,
        };

        for (int i = 0; i < this.PieStrings.Count; i++)
            ps.Slices.Add(new PieSlice(this.PieStrings[i], this.PieValues[i]) { IsExploded = true });

        plotModel.Series.Add(ps);

        return plotModel;
    }

    public void ExportPng(string fileName, int plotStyle, int width = 600, int height = 400)
    {
        PlotModel pm;
        switch (plotStyle)
        {
            case 1: pm = LineBarSeries(); break;
            case 2: pm = PieChartSeries(); break;
            default: throw new NullReferenceException("Plot style null.");
        }

        //check if parameters contains file extension
        if (!fileName.Contains(".png"))
            fileName += ".png";

        if (pm != null)
            PngExporter.Export(pm, fileName, width, height);    //export
    }


    public string Title { get; set; }

    public List<float> LineValues { get; set; }
    public List<DateTime> LineDates { get; set; }

    public List<float> PieValues { get; set; }
    public List<string> PieStrings { get; set; }

}
