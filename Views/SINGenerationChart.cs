using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Views
{
    public class SINGenerationChart: BaseChart
    {
        List<Color> seriesColors;
        SINGenerationType generationType;
        int tipografico;

        public SINGenerationChart(SINGenerationType generationType,int tipo)
        {
            this.generationType = generationType;
            tipografico=tipo;

            seriesColors = new List<Color>
            {
                Colors.Red,
                Colors.Gray,
                Colors.PapayaWhip,
                Colors.Purple,
                Colors.Yellow,
                Colors.HotPink,
                Colors.Black,
                Colors.LimeGreen,
                Colors.Orange,
                Color.FromRgb(138,186,212),
                Colors.Black,
                Colors.Red
            };

            switch (generationType)
            {
                case SINGenerationType.Daily:
                    VerticalAxis.Title = "GWh-día";
                    break;
                case SINGenerationType.Hourly:
                    VerticalAxis.Title = "MWh-hora";
                    break;
                case SINGenerationType.Period:
                    VerticalAxis.Title = "GWh / Etapa";
                    break;
            }
        }

        public override void Update(int scenario)
        {
            if (tipografico != 0)
            {
                Series.Clear();

            List<DataSeriesViewModel> dataSeriesList;
            try
            {
                dataSeriesList = ResultsDataProvider.GetSINGenerationDataSeries(scenario, generationType);
            }
            catch
            {
                throw;
            }

            Series.Add(ChartSeriesCreator.CreateAreaSeries(dataSeriesList[0], seriesColors[0], true));

            int position;
            for (position = 1; position < dataSeriesList.Count - 2; position++)
                Series.Add(ChartSeriesCreator.CreateAreaSeries(dataSeriesList[position], seriesColors[position], false));

            Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[position], seriesColors[position], false, false));
            position++;

            LineSeries marginalCostSeries = ChartSeriesCreator.CreateLineSeries(dataSeriesList[position], seriesColors[position], true, false);
            marginalCostSeries.VerticalAxis = new LinearAxis()
            {
                HorizontalLocation = AxisHorizontalLocation.Right,
                Title = "$/MWh",
                Minimum = 0
            };
            Series.Add(marginalCostSeries);
            }
        }
        }
}
