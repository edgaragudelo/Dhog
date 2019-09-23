using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;

namespace DHOG_WPF.Views
{
    public class ReservoirEvolutionChart : BaseChart
    {
        List<Color> seriesColors;
        int tipografico;

        public ReservoirEvolutionChart(int tipo)
        {
            tipografico = tipo;
            if (tipografico !=0)
            { 
             seriesColors = new List<Color>
             {
                Color.FromRgb(226,235,160),
                Color.FromRgb(138,186,212),
                Colors.Black
             };
             VerticalAxis.Title = "Porcentaje Embalse Útil";
            }
         }

        public override void Update(int scenario, string selectedReservoir)
        {
            if (tipografico != 0)
            {
                Series.Clear();

                List<DataSeriesViewModel> dataSeriesList;
                try
                {
                    dataSeriesList = ResultsDataProvider.GetReservoirEvolutionDataSeries(scenario, selectedReservoir);
                }
                catch
                {
                    throw;
                }

                Series.Add(ChartSeriesCreator.CreateAreaSeries(dataSeriesList[0], seriesColors[0], true));
                Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[1], seriesColors[1], true, false));

                LineSeries generationSeries = ChartSeriesCreator.CreateLineSeries(dataSeriesList[2], seriesColors[2], false, false);
                generationSeries.VerticalAxis = new LinearAxis()
                {
                HorizontalLocation = AxisHorizontalLocation.Right,
                Title = "Generación en MWh / Etapa",
                Minimum = 0
                };
                Series.Add(generationSeries);
            }
               }
    }
}
