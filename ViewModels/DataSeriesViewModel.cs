using DHOG_WPF.DataTypes;
using System.Collections.ObjectModel;

namespace DHOG_WPF.ViewModels
{
    public class DataSeriesViewModel
    {
        public DataSeriesViewModel(string name, ObservableCollection<ChartDataPoint> dataPoints)
        {
            Name = name;
            DataPoints = dataPoints;
        }

        public string  Name{ get; }
        public ObservableCollection<ChartDataPoint> DataPoints { get; }
    }
}
