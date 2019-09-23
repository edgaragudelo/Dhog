using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicFuelsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicFuelsViewModel: PeriodicEntityViewModel
    {
        PeriodicFuel periodicFuel;

        public PeriodicFuelsViewModel(PeriodicFuel periodicFuel): base(periodicFuel)
        {
            this.periodicFuel = periodicFuel;
        }

        public PeriodicFuel GetDataObject()
        {
            return periodicFuel;
        }

        public double Capacity
        {
            get
            {
                return periodicFuel.Capacity;
            }
            set
            {
                periodicFuel.Capacity = value;
                RaisePropertyChanged("Capacity");
            }
        }

        public double Min
        {
            get
            {
                return periodicFuel.Min;
            }
            set
            {
                periodicFuel.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Cost
        {
            get
            {
                return periodicFuel.Cost;
            }
            set
            {
                periodicFuel.Cost = value;
                RaisePropertyChanged("Cost");
            }
        }

        public double TransportCost
        {
            get
            {
                return periodicFuel.TransportCost;
            }
            set
            {
                periodicFuel.TransportCost = value;
                RaisePropertyChanged("TransportCost");
            }
        }
    }
}
