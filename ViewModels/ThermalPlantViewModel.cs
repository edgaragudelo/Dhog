using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class ThermalPlantsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class ThermalPlantViewModel : BaseViewModel
    {
        ThermalPlant plant;

        /* Constructor needed to add rows in the RadGridView control */
        public ThermalPlantViewModel()
        {
            plant = new ThermalPlant();
            Case = 1;
            Company = "";
            Fuel = "";
            Subarea = "";
            StartPeriod = 1;
        }

        public ThermalPlantViewModel(ThermalPlant plant)
        {
            this.plant = plant;
        }

        public ThermalPlant GetDataObject()
        {
            return plant;
        }

        public int Id
        {
            get
            {
                return plant.Id;
            }
            set
            {
                plant.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return plant.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    plant.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Company
        {
            get
            {
                return plant.Company;
            }
            set
            {
                if (value == null)
                    plant.Company = "";
                else
                    plant.Company = value;
                
            }
        }

        public double Min
        {
            get
            {
                return plant.Min;
            }
            set
            {
                plant.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Max
        {
            get
            {
                return plant.Max;
            }
            set
            {
                plant.Max = value;
                RaisePropertyChanged("Max");
            }
        }


        public double VariableCost
        {
            get
            {
                return plant.VariableCost;
            }
            set
            {
                plant.VariableCost = value;
                RaisePropertyChanged("VariableCost");
            }
        }

        public double ProductionFactor
        {
            get
            {
                return plant.ProductionFactor;
            }
            set
            {
                plant.ProductionFactor = value;
                RaisePropertyChanged("ProductionFactor");
            }
        }

        public double AvailabilityFactor
        {
            get
            {
                return plant.AvailabilityFactor;
            }
            set
            {
                plant.AvailabilityFactor = value;
                RaisePropertyChanged("AvailabilityFactor");
            }
        }

        public string Fuel
        {
            get
            {
                return plant.Fuel;
            }
            set
            {
                if (value == null)
                    plant.Fuel = "";
                else
                    plant.Fuel = value;

                RaisePropertyChanged("Fuel");
            }
        }

        public bool HasVariableProductionFactor
        {
            get
            {
                return Convert.ToBoolean(plant.HasVariableProductionFactor);
            }
            set
            {
                plant.HasVariableProductionFactor = Convert.ToInt32(value);
                RaisePropertyChanged("HasVariableProductionFactor ");
            }
        }

        public bool IsMandatory
        {
            get
            {
                return Convert.ToBoolean(plant.IsMandatory);
            }
            set
            {
                plant.IsMandatory = Convert.ToInt32(value);
                RaisePropertyChanged("IsMandatory");
            }
        }

        public int StartPeriod
        {
            get
            {
                return plant.StartPeriod;
            }
            set
            {
                plant.StartPeriod = value;
                RaisePropertyChanged("StartPeriod");
            }
        }

        public int Case
        {
            get
            {
                return plant.Case;
            }
            set
            {
                plant.Case = value;
                RaisePropertyChanged("Case");
            }
        }

        public string Subarea
        {
            get
            {
                return plant.Subarea;
            }
            set
            {
                if (value == null)
                    plant.Subarea = "";
                else
                    plant.Subarea = value;

            }
        }
    }
}
