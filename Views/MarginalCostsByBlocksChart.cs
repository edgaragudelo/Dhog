using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Globalization;

using System.Diagnostics;

using System.Windows;

namespace DHOG_WPF.Views
{
    public class MarginalCostsByBlocksChart : BaseChart
    {
       List<System.Windows.Media.Color> seriesColors;

        List<string> seriesColors1 = new List<string>();


        int tipografico;

        public MarginalCostsByBlocksChart(int tipo)
        {
          if (tipo != 2)
          { 
            //string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));
            // Prueba();
                      

             foreach (System.Reflection.PropertyInfo prop in typeof(Colors).GetProperties())
            {
                //if (prop.PropertyType.FullName == "System.Drawing.Color")
                seriesColors1.Add(prop.Name);
                //System.Drawing.Color colorconv = ColorTranslator.FromHtml(prop.Name);
                //seriesColors.Add(colorconv);
            }

            seriesColors = new List<System.Windows.Media.Color>

            {
                System.Windows.Media.Color.FromRgb(138, 186, 212),
                Colors.LimeGreen,Colors.Orange,Colors.Gray,Colors.Yellow,Colors.Red,Colors.Black,
                Colors.Blue,Colors.Violet,Colors.Silver,Colors.Purple,Colors.Beige,Colors.Chocolate,
                Colors.DodgerBlue,Colors.SeaGreen,Colors.RoyalBlue,Colors.SteelBlue,Colors.BlueViolet,
                Colors.Fuchsia,Colors.Honeydew,Colors.HotPink


            };
            VerticalAxis.Title = "$/MWh";
            tipografico = tipo;
           }
            }



       
        public void Prueba()
        {

            //string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));

            //System.Array colorsArray = Enum.GetValues(typeof(KnownColor));
           // List<Color> myList = new List<Color>();

            List<string> myList = new List<string>();

            foreach (System.Reflection.PropertyInfo prop in typeof(Colors).GetProperties())
            {
                //if (prop.PropertyType.FullName == "System.Drawing.Color")
                    myList.Add(prop.Name);
                
            }


           // seriesColors = myList;


        }





       

        public override void Update(int scenario)
        {
          if (tipografico !=2)
          { 
            Series.Clear();



            List<DataSeriesViewModel> dataSeriesList;
            try
            {
                dataSeriesList = ResultsDataProvider.GetMarginalCostsByBlocksDataSeries(scenario,tipografico);
            }
            catch
            {
                throw;
            }

            Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[0], seriesColors[0], false, true));

            for (int position = 1; position < dataSeriesList.Count; position++)
            {
                try
                {
                    Series.Add(ChartSeriesCreator.CreateLineSeries(dataSeriesList[position], seriesColors[position], false, false));

               

                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }
            }
    }
}
