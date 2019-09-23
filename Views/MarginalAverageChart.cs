using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using static DHOG_WPF.DataTypes.Types;
using Telerik.Windows.Controls;
using DHOG_WPF.DataTypes;
using System.Windows.Data;
using System;

namespace DHOG_WPF.Views
{
    public class MarginalAverageChart : BaseChart
    {
        
        public MarginalAverageChart(int tipo)
        {
            if (tipo != 0)
            {
                try
            {
                Series.Clear();

                IsEnabled = true;
                
                Series.Add(ChartSeriesCreator.CreateCandleStickSeries(ResultsDataProvider.GetMarginaAverageDataSeries()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error en la grafica de bigotes", e.Message);
                Series.Clear();
                //throw;
            }
            }

        }
    }
}
