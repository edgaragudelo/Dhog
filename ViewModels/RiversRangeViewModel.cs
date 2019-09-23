
using DHOG_WPF.DataTypes;


namespace DHOG_WPF.ViewModels
{
    public class RiversRangeViewModel : BaseViewModel
    {
        OpenRange openRange;

        public RiversRangeViewModel()
        {
            openRange = new OpenRange();
        }

        public RiversRangeViewModel(int min, int max)
        {
            openRange = new OpenRange(min, max);
        }

        public int Min
        {
            get
            {
                return openRange.Min;
            }
            set
            {
                openRange.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public int Max
        {
            get
            {
                return openRange.Max;
            }
            set
            {
                openRange.Max = value;
                RaisePropertyChanged("Max");
            }
        }
    }
}
