
namespace DHOG_WPF.DataTypes
{
    public class OpenRange
    {
        public OpenRange() { }

        public OpenRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; set; }
        public int Max { get; set; }

        public bool Contains(int value)
        {
            if (value >= Min && value < Max)
                return true;
            else
                return false;
        }

        public bool isValid()
        {
            if (Min < Max)
                return true;
            else
                return false;
        }

    }
}
