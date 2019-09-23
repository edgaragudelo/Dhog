using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class VariableConventionalPlantsCollectionViewModel : CollectionBaseViewModel
    {
        
    }

    public class VariableConventionalPlantViewModel: BaseViewModel
    {
        VariableConventionalPlant variableConventionalPlant;

        /* Constructor needed to add rows in the RadGridView control */
        public VariableConventionalPlantViewModel()
        {
            variableConventionalPlant = new VariableConventionalPlant();
        }

        public VariableConventionalPlantViewModel(VariableConventionalPlant variableConventionalPlant)
        {
            this.variableConventionalPlant = variableConventionalPlant;
        }

        public VariableConventionalPlant GetDataObject()
        {
            return variableConventionalPlant;
        }

        public int Id
        {
            get
            {
                return variableConventionalPlant.Id;
            }
            set
            {
                variableConventionalPlant.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return variableConventionalPlant.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    variableConventionalPlant.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public int Segment
        {
            get
            {
                return variableConventionalPlant.Segment;
            }
            set
            {
                variableConventionalPlant.Segment = value;
                RaisePropertyChanged("Segment");
            }
        }

        public double Max
        {
            get
            {
                return variableConventionalPlant.Max;
            }
            set
            {
                variableConventionalPlant.Max = value;
                RaisePropertyChanged("Max");
            }
        }

        public double ProductionFactor
        {
            get
            {
                return variableConventionalPlant.ProductionFactor;
            }
            set
            {
                variableConventionalPlant.ProductionFactor = value;
                RaisePropertyChanged("ProductionFactor");
            }
        }
    }
}
