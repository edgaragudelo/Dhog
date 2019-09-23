using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicConventionalPlantsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicConventionalPlantsViewModel: PeriodicEntityViewModel
    {
        PeriodicConventionalPlant periodicHydroPlant;

        public PeriodicConventionalPlantsViewModel(PeriodicConventionalPlant periodicHydroPlant): base(periodicHydroPlant)
        {
            this.periodicHydroPlant = periodicHydroPlant;
        }

        public PeriodicConventionalPlant GetDataObject()
        {
            return periodicHydroPlant;
        }

        public double VariableCost
        {
            get
            {
                return periodicHydroPlant.VariableCost;
            }
            set
            {
                periodicHydroPlant.VariableCost = value;
                RaisePropertyChanged("VariableCost");
            }
        }

        public double Min
        {
            get
            {
                return periodicHydroPlant.Min;
            }
            set
            {
                periodicHydroPlant.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Max
        {
            get
            {
                return periodicHydroPlant.Max;
            }
            set
            {
                periodicHydroPlant.Max = value;
                RaisePropertyChanged("Max");
            }
        }

        public bool IsMandatory
        {
            get
            {
                return Convert.ToBoolean(periodicHydroPlant.IsMandatory);
            }
            set
            {
                periodicHydroPlant.IsMandatory = Convert.ToInt32(value);
                RaisePropertyChanged("IsMandatory");
            }
        }
    }
}
