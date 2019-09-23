
namespace DHOG_WPF.Models
{
    public class PeriodicCompany:PeriodicEntity
    {
        public PeriodicCompany(string name, int period, double stockPrice, double contract, int scenario)
        {
            Name = name;
            Period = period;
            StockPrice = stockPrice;
            Contract = contract;
            Case = scenario;
        }

        public double StockPrice { get; set; }
        public double Contract { get; set; }
    }
}
