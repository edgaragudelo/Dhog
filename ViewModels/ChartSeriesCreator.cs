using DHOG_WPF.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;


namespace DHOG_WPF.ViewModels
{
    public class ChartSeriesCreator
    {
        public static CandlestickSeries CreateCandleStickSeries(List<CandleDataInfo> dataSeries)
        {
            CandlestickSeries candlestickSeries = new CandlestickSeries();

            foreach (var item in dataSeries)
            {
                candlestickSeries.DataPoints.Add(new OhlcDataPoint
                {
                    Open = item.Open,
                    Close = item.Close,
                    Low = item.Low,
                    High = item.High
                });
            }


            return candlestickSeries;
        }

        public static BarSeries CreateBarSeries(DataSeriesViewModel dataSeries, bool firstSeries)
        {
            BarSeries barSeries = new BarSeries();
            SeriesLegendSettings legendSettings = new SeriesLegendSettings() { Title = dataSeries.Name };
            barSeries.LegendSettings = legendSettings;
            barSeries.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XValue" };
            barSeries.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YValue" };
            barSeries.ItemsSource = dataSeries.DataPoints;
            barSeries.TrackBallInfoTemplate = CreateTooltipDataTemplate(dataSeries.Name, firstSeries);
            barSeries.CombineMode = ChartSeriesCombineMode.Cluster;
            barSeries.PaletteMode = SeriesPaletteMode.Series;
            return barSeries;
        }

        public static StepLineSeries CreateStepLineSeries(DataSeriesViewModel dataSeries, Color color, bool firstSeries)
        {
            StepLineSeries stepLineSeries = new StepLineSeries();
            SeriesLegendSettings legendSettings = new SeriesLegendSettings() { Title = dataSeries.Name };
            stepLineSeries.LegendSettings = legendSettings;
            stepLineSeries.CombineMode = ChartSeriesCombineMode.None;
            stepLineSeries.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XValue" };
            stepLineSeries.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YValue" };
            stepLineSeries.ItemsSource = dataSeries.DataPoints;
            stepLineSeries.TrackBallInfoTemplate = CreateTooltipDataTemplate(dataSeries.Name, firstSeries);
            stepLineSeries.Stroke = new SolidColorBrush(color);
            return stepLineSeries;
        }

        public static AreaSeries CreateAreaSeries(DataSeriesViewModel dataSeries, Color color, bool firstSeries)
        {
            AreaSeries areaSeries = new AreaSeries();
            SeriesLegendSettings legendSettings = new SeriesLegendSettings() { Title = dataSeries.Name };
            areaSeries.LegendSettings = legendSettings;
            areaSeries.Fill = new SolidColorBrush(color);
            areaSeries.CombineMode = ChartSeriesCombineMode.Stack;
            areaSeries.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XValue" };
            areaSeries.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YValue" };
            areaSeries.ItemsSource = dataSeries.DataPoints;
            areaSeries.TrackBallInfoTemplate = CreateTooltipDataTemplate(dataSeries.Name, firstSeries);
            return areaSeries;
        }

        public static LineSeries CreateLineSeries(DataSeriesViewModel dataSeries, Color color, bool dashed, bool firstSeries)
        {
            LineSeries lineSeries = new LineSeries();
            lineSeries.LegendSettings = new SeriesLegendSettings() { Title = dataSeries.Name };
            lineSeries.Stroke = new SolidColorBrush(color);
            lineSeries.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XValue" };
            lineSeries.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YValue" };
            lineSeries.ItemsSource = dataSeries.DataPoints;
            lineSeries.TrackBallInfoTemplate = CreateTooltipDataTemplate(dataSeries.Name, firstSeries);

            if (dashed)
            {
                List<double> dashArray = new List<double>() { 1, 1 };
                lineSeries.DashArray = new DoubleCollection(dashArray);
                lineSeries.StrokeThickness = 5;
            }

            return lineSeries;
        }

        public static DataTemplate CreateTooltipDataTemplate(String seriesName, bool firstSeries)
        {
            DataTemplate dataTemplate = new DataTemplate(typeof(CartesianSeries));

            FrameworkElementFactory valueText = new FrameworkElementFactory(typeof(TextBlock));

            Binding binding = new Binding()
            {
                Path = new PropertyPath("DataPoint.Value"),
                StringFormat = seriesName + " = {0:F3}"
            };
            valueText.SetBinding(TextBlock.TextProperty, binding);

            if (firstSeries)
            {
                FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));

                FrameworkElementFactory dateText = new FrameworkElementFactory(typeof(TextBlock));
                binding = new Binding()
                {
                    Path = new PropertyPath("DataPoint.Category"),
                    StringFormat = "Fecha = {0}"
                };
                dateText.SetBinding(TextBlock.TextProperty, binding);
                stackPanel.AppendChild(dateText);

                stackPanel.AppendChild(valueText);

                dataTemplate.VisualTree = stackPanel;
            }
            else
                dataTemplate.VisualTree = valueText;

            return dataTemplate;
        }

        public static DataTemplate CreateMainSeriesTooltipDataTemplate(String seriesName)
        {
            DataTemplate dataTemplate = new DataTemplate(typeof(CartesianSeries));

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));

            FrameworkElementFactory txtElement = new FrameworkElementFactory(typeof(TextBlock));
            Binding binding = new Binding()
            {
                Path = new PropertyPath("DataPoint.Value"),
                StringFormat = seriesName + " = {0:F3} "
            };
            txtElement.SetBinding(TextBlock.TextProperty, binding);
            stackPanel.AppendChild(txtElement);

            FrameworkElementFactory txtElement2 = new FrameworkElementFactory(typeof(TextBlock));
            binding = new Binding()
            {
                Path = new PropertyPath("DataPoint.Category"),
                StringFormat = "Fecha = {0}"
            };
            txtElement2.SetBinding(TextBlock.TextProperty, binding);
            stackPanel.AppendChild(txtElement2);

            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
        }
    }
}
