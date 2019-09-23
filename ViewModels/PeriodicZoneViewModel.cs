using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicZonesCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicZonesViewModel: PeriodicEntityViewModel
    {
        PeriodicZone periodicZone;

        public PeriodicZonesViewModel(PeriodicZone periodicZone): base(periodicZone)
        {
            this.periodicZone = periodicZone;
        }

        public PeriodicZone GetDataObject()
        {
            return periodicZone;
        }

        public string Type
        {
            get
            {
                return periodicZone.Type;
            }
            set
            {
                periodicZone.Type = value;
                RaisePropertyChanged("Type");
            }
        }

        public double Value
        {
            get
            {
                return periodicZone.Value;
            }
            set
            {
                periodicZone.Value = value;
                RaisePropertyChanged("Value");
            }
        }
    }
}
