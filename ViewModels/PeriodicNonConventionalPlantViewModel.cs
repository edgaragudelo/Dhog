using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicNonConventionalPlantsCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicNonConventionalPlantsViewModel: PeriodicEntityViewModel
    {
        PeriodicNonConventionalPlant periodicNonConventionalPlant;

        public PeriodicNonConventionalPlantsViewModel(PeriodicNonConventionalPlant periodicNonConventionalPlant): base(periodicNonConventionalPlant)
        {
            this.periodicNonConventionalPlant = periodicNonConventionalPlant;
        }

        public PeriodicNonConventionalPlant GetDataObject()
        {
            return periodicNonConventionalPlant;
        }

        public double Max
        {
            get
            {
                return periodicNonConventionalPlant.Max;
            }
            set
            {
                periodicNonConventionalPlant.Max = value;
                RaisePropertyChanged("Max");
            }
        }

        public double PlantFactor
        {
            get
            {
                return periodicNonConventionalPlant.PlantFactor;
            }
            set
            {
                periodicNonConventionalPlant.PlantFactor = value;
                RaisePropertyChanged("PlantFactor");
            }
        }
    }
}
