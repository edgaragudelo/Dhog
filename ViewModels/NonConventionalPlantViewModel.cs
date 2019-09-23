using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class NonConventionalPlantsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class NonConventionalPlantViewModel : BaseViewModel
    {
        NonConventionalPlant plant;

        /* Constructor needed to add rows in the RadGridView control */
        public NonConventionalPlantViewModel()
        {
            plant = new NonConventionalPlant();
            Case = 1;
            Type = "";
            Company = "";
            Subarea = "";
            StartPeriod = 1;
        }

        public NonConventionalPlantViewModel(NonConventionalPlant plant)
        {
            this.plant = plant;
        }

        public NonConventionalPlant GetDataObject()
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

        public string Type
        {
            get
            {
                return plant.Type;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    plant.Type = value;
                    RaisePropertyChanged("Type");
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
