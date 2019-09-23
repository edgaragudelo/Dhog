using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Views
{
    public class GenerationByCompanyChart : BaseChart
    {
        int tipografico;
        public GenerationByCompanyChart(int tipo)
        {
            tipografico = tipo;
            VerticalAxis.Title = "MWh / Empresa";
        }

        public override void Update(int scenario, string selctedCompany)
        {
            if (tipografico != 0)
            {
                Series.Clear();

            List<DataSeriesViewModel> dataSeriesList;
            try
            {
                dataSeriesList = ResultsDataProvider.GetGenerationByCompanyDataSeries(scenario, selctedCompany);
            }
            catch
            {
                throw;
            }

            this.Palette = new ChartPalette();
            this.Palette.GlobalEntries.Clear();
            this.Palette.GlobalEntries.Add(new PaletteEntry(new SolidColorBrush(Colors.DodgerBlue)));
            this.Palette.GlobalEntries.Add(new PaletteEntry(new SolidColorBrush(Colors.Goldenrod)));

            Series.Add(ChartSeriesCreator.CreateBarSeries(dataSeriesList[0], false));

            Series.Add(ChartSeriesCreator.CreateBarSeries(dataSeriesList[1], false));
        }
        }
    }
}
