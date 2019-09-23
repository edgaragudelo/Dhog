
namespace DHOG_WPF.DataTypes
{
    public class ChartDataPoint
    {
        public ChartDataPoint(string xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }

        public string XValue { get; }
        public double YValue { get; }
    }
}
