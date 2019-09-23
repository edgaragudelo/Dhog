using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicReservoirsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicReservoirsViewModel: PeriodicEntityViewModel
    {
        PeriodicReservoir periodicReservoir;

        public PeriodicReservoirsViewModel(PeriodicReservoir periodicReservoir): base(periodicReservoir)
        {
            this.periodicReservoir = periodicReservoir;
        }

        public PeriodicReservoir GetDataObject()
        {
            return periodicReservoir;
        }

        public double MinLevel
        {
            get
            {
                return periodicReservoir.MinLevel;
            }
            set
            {
                periodicReservoir.MinLevel = value;
                RaisePropertyChanged("MinLevel");
            }
        }

        public double MaxLevel
        {
            get
            {
                return periodicReservoir.MaxLevel;
            }
            set
            {
                periodicReservoir.MaxLevel = value;
                RaisePropertyChanged("MaxLevel");
            }
        }
    }
}
