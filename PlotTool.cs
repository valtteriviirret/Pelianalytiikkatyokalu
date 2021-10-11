using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.ImageSharp;
using OxyPlot.Series;
using OxyPlot.Axes;

/* Dotnet CLI commands for OxyPlot libraries. 
dotnet add package OxyPlot.Core --version 2.1.0
dotnet add package OxyPlot.ImageSharp --version 2.1.0
*/


public class PlotTool
{
    // Linear bar series constructor.
    public PlotTool(string title, List<float> lineData, List<DateTime> lineDates)
    {
        this.Title = title;
        this.LineData = lineData;
        this.LineDates = lineDates;
    }

    // Pie chart series constructor.
    /*     public PlotTool(string title, List<float> lineData, List<DateTime> lineDates)
        {
            this.Title = title;
            this.LineData = lineData;
            this.LineDates = lineDates;
        } */

    // Insert instance variables to a new function series.
    private PlotModel LineBarSeries()
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

        for (int i = 0; i < this.Dates.Count; i++)
            lineBarSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.Dates[i]), this.Data[i]));

        plotModel.Series.Add(lineBarSeries);
        plotModel.Axes.Add(xAxis);
        plotModel.Axes.Add(new LinearAxis());

        return plotModel;
    }

    private PlotModel PieChartSeries()
    {
        var plotModel = new PlotModel { Title = this.Title, Background = OxyColors.White };

        var ps = new PieSeries
        {
            StrokeThickness = 2.0,
            InsideLabelPosition = 0.8,
            AngleSpan = 360,
            StartAngle = 0
        };

        ps.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = true });
        ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
        ps.Slices.Add(new PieSlice("Asia", 4157));
        ps.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
        ps.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });

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

        if (!fileName.Contains(".png"))
            fileName += ".png";

        if (pm != null)
            PngExporter.Export(pm, fileName, width, height);
    }


    public string Title { get; set; }

    public List<float> LineData { get; set; }

    public List<DateTime> LineDates { get; set; }


}
