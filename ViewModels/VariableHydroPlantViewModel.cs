using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class VariableHydroPlantsCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class VariableHydroPlantViewModel: BaseViewModel
    {
        VariableHydroPlant variableHydroPlant;

        public VariableHydroPlantViewModel()
        {
            variableHydroPlant = new VariableHydroPlant();
            Case = 1;
            Segment = 1;
        }

        public VariableHydroPlantViewModel(VariableHydroPlant variableHydroPlant)
        {
            this.variableHydroPlant = variableHydroPlant;
        }

        public VariableHydroPlant GetDataObject()
        {
            return variableHydroPlant;
        }

        public int Id
        {
            get
            {
                return variableHydroPlant.Id;
            }
            set
            {
                variableHydroPlant.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return variableHydroPlant.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    variableHydroPlant.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public int Segment
        {
            get
            {
                return variableHydroPlant.Segment;
            }
            set
            {
                variableHydroPlant.Segment = value;
                RaisePropertyChanged("Segment");
            }
        }

        public double Max
        {
            get
            {
                return variableHydroPlant.Max;
            }
            set
            {
                variableHydroPlant.Max = value;
                RaisePropertyChanged("Max");
            }
        }

        public double ProductionFactor
        {
            get
            {
                return variableHydroPlant.ProductionFactor;
            }
            set
            {
                variableHydroPlant.ProductionFactor = value;
                RaisePropertyChanged("ProductionFactor");
            }
        }

        public string Reservoir
        {
            get
            {
                return variableHydroPlant.Reservoir;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    variableHydroPlant.Reservoir = value;
                    RaisePropertyChanged("Reservoir");
                }
            }
        }

        public int Case
        {
            get
            {
                return variableHydroPlant.Case;
            }
            set
            {
                variableHydroPlant.Case = value;
                RaisePropertyChanged("Case");
            }
        }

        public double Level
        {
            get
            {
                return variableHydroPlant.Level;
            }
            set
            {
                variableHydroPlant.Level = value;
                RaisePropertyChanged("Level");
            }
        }
    }
}
