using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class FuelContractsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class FuelContractViewModel : BaseViewModel
    {
        FuelContract fuelContract;

        /* Constructor needed to add rows in the RadGridView control */
        public FuelContractViewModel()
        {
            fuelContract = new FuelContract();
            Type = "";
        }

        public FuelContractViewModel(FuelContract fuelContract)
        {
            this.fuelContract = fuelContract;
        }

        public FuelContract GetDataObject()
        {
            return fuelContract;
        }

        public int Id
        {
            get
            {
                return fuelContract.Id;
            }
            set
            {
                fuelContract.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return fuelContract.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    fuelContract.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Type
        {
            get
            {
                return fuelContract.Type;
            }
            set
            {
                if (value == null)
                    fuelContract.Type = "";
                else
                    fuelContract.Type = value;
                
            }
        }

        public double Min
        {
            get
            {
                return fuelContract.Min;
            }
            set
            {
                fuelContract.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Capacity
        {
            get
            {
                return fuelContract.Capacity;
            }
            set
            {
                fuelContract.Capacity = value;
                RaisePropertyChanged("Capacity");
            }
        }


        public double Cost
        {
            get
            {
                return fuelContract.Cost;
            }
            set
            {
                fuelContract.Cost = value;
                RaisePropertyChanged("Cost");
            }
        }

        public int InitialPeriod
        {
            get
            {
                return fuelContract.InitialPeriod;
            }
            set
            {
                fuelContract.InitialPeriod = value;
                RaisePropertyChanged("InitialPeriod");
            }
        }

        public int FinalPeriod
        {
            get
            {
                return fuelContract.FinalPeriod;
            }
            set
            {
                fuelContract.FinalPeriod= value;
                RaisePropertyChanged("FinalPeriod");
            }
        }
    }
}
