using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicFuelContractsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicFuelContractsViewModel: PeriodicEntityViewModel
    {
        PeriodicFuelContract periodicFuelContract;

        public PeriodicFuelContractsViewModel(PeriodicFuelContract periodicFuelContract): base(periodicFuelContract)
        {
            this.periodicFuelContract = periodicFuelContract;
        }

        public PeriodicFuelContract GetDataObject()
        {
            return periodicFuelContract;
        }

        public double Capacity
        {
            get
            {
                return periodicFuelContract.Capacity;
            }
            set
            {
                periodicFuelContract.Capacity = value;
                RaisePropertyChanged("Capacity");
            }
        }

        public double Min
        {
            get
            {
                return periodicFuelContract.Min;
            }
            set
            {
                periodicFuelContract.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Cost
        {
            get
            {
                return periodicFuelContract.Cost;
            }
            set
            {
                periodicFuelContract.Cost = value;
                RaisePropertyChanged("Cost");
            }
        }
    }
}
