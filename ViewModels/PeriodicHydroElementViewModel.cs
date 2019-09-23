using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicHydroElementsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicHydroElementsViewModel: PeriodicEntityViewModel
    {
        PeriodicHydroElement periodicHydroElement;

        public PeriodicHydroElementsViewModel(PeriodicHydroElement periodicHydroElement): base(periodicHydroElement)
        {
            this.periodicHydroElement = periodicHydroElement;
        }

        public PeriodicHydroElement GetDataObject()
        {
            return periodicHydroElement;
        }

        public double MinTurbinedOutflow
        {
            get
            {
                return periodicHydroElement.MinTurbinedOutflow;
            }
            set
            {
                periodicHydroElement.MinTurbinedOutflow = value;
                RaisePropertyChanged("MinTurbinedOutflow");
            }
        }

        public double MaxTurbinedOutflow
        {
            get
            {
                return periodicHydroElement.MaxTurbinedOutflow;
            }
            set
            {
                periodicHydroElement.MaxTurbinedOutflow = value;
                RaisePropertyChanged("MaxTurbinedOutflow");
            }
        }

        public double Filtration
        {
            get
            {
                return periodicHydroElement.Filtration;
            }
            set
            {
                periodicHydroElement.Filtration = value;
                RaisePropertyChanged("Filtration");
            }
        }

        public double RecoveryFactor
        {
            get
            {
                return periodicHydroElement.RecoveryFactor;
            }
            set
            {
                periodicHydroElement.RecoveryFactor = value;
                RaisePropertyChanged("RecoveryFactor");
            }
        }
    }
}
