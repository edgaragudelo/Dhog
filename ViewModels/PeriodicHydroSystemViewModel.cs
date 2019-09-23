using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicHydroSystemsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicHydroSystemsViewModel: PeriodicEntityViewModel
    {
        PeriodicHydroSystem periodicHydroSystem;

        public PeriodicHydroSystemsViewModel(PeriodicHydroSystem periodicHydroSystem): base(periodicHydroSystem)
        {
            this.periodicHydroSystem = periodicHydroSystem;
        }

        public PeriodicHydroSystem GetDataObject()
        {
            return periodicHydroSystem;
        }

        public double MinTurbinedOutflow
        {
            get
            {
                return periodicHydroSystem.MinTurbinedOutflow;
            }
            set
            {
                periodicHydroSystem.MinTurbinedOutflow = value;
                RaisePropertyChanged("MinTurbinedOutflow");
            }
        }

        public double MaxTurbinedOutflow
        {
            get
            {
                return periodicHydroSystem.MaxTurbinedOutflow;
            }
            set
            {
                periodicHydroSystem.MaxTurbinedOutflow = value;
                RaisePropertyChanged("MaxTurbinedOutflow");
            }
        }
    }
}
