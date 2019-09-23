using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicInflowsCollectionViewModel : PeriodicEntityCollectionViewModel
    {

    }

    public class PeriodicInflowViewModel: PeriodicEntityViewModel
    {
        PeriodicInflow periodicInflow;

        public PeriodicInflowViewModel(PeriodicInflow periodicInflow):base(periodicInflow)
        {
            this.periodicInflow = periodicInflow;
        }

        public PeriodicInflow GetDataObject()
        {
            return periodicInflow;
        }

        public double Value
        {
            get
            {
                return periodicInflow.Value;
            }
            set
            {
                periodicInflow.Value = value;
                RaisePropertyChanged("Value");
            }
        }
    }
}
