
namespace DHOG_WPF.Models
{
    public class PeriodicLoadBlock : PeriodicEntity
    {
        public PeriodicLoadBlock(int block, int period, double load, int scenario)
        {
            Block = block;
            Period = period;
            Load = load;
            Case = scenario;
        }

        public int Block { get; set; }
        public double Load { get; set; }
    }
}
