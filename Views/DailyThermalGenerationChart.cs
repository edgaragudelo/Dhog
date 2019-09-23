using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;

namespace DHOG_WPF.Views
{
    public class DailyThermalGenerationChart : BaseChart
    {
        List<Color> seriesColors;
        int tipografico;

        public DailyThermalGenerationChart(int tipo)
        {
            tipografico = tipo;
            seriesColors = new List<Color>
            {
                Colors.Black,
                Colors.LimeGreen,
                Colors.Orange,
            };
            VerticalAxis.Title = "GWh-día";
        }

        public override void Update(int scenario)
        {
            if (tipografico != 0)
            {
                Series.Clear();

            List<DataSeriesViewModel> dataSeriesList;
            try
            {
                dataSeriesList = ResultsDataProvider.GetDailyThermalGenerationDataSeries(scenario);
            }
            catch
            {
                throw;
            }

            Series.Add(ChartSeriesCreator.CreateAreaSeries(dataSeriesList[0], seriesColors[0], true));

            for (int position = 1; position < dataSeriesList.Count; position++)
                Series.Add(ChartSeriesCreator.CreateAreaSeries(dataSeriesList[position], seriesColors[position], false));
        }
        }

    }
}
