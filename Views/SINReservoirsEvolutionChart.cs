using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;

namespace DHOG_WPF.Views
{
    public class SINReservoirsEvolutionChart : BaseChart
    {
        List<Color> seriesColors;
        int tipografico;

        public SINReservoirsEvolutionChart(int tipo)
        {
            tipografico=tipo;
            seriesColors = new List<Color>
            {
                //Colors.LightGray,
                Colors.LimeGreen,
                Color.FromRgb(138,186,212)
            };
            VerticalAxis.Title = "%";
        }

        public override void Update(int scenario)
        {
            if (tipografico !=0)
            { 
            Series.Clear();

            List<DataSeriesViewModel> dataSeriesList;
            try
            {
                dataSeriesList = ResultsDataProvider.GetSINReservoirsEvolutionDataSeries(scenario);
            }
            catch
            {
                throw;
            }

            Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[0], seriesColors[0], false, true));

            for (int position = 1; position < dataSeriesList.Count; position++)
                Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[position], seriesColors[position], false, false));
            }
        }
    }
}
