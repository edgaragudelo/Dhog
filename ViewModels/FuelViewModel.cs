using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class FuelsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class FuelViewModel : BaseViewModel
    {
        Fuel fuel;

        /* Constructor needed to add rows in the RadGridView control */
        public FuelViewModel()
        {
            fuel = new Fuel();
            Type = "";
        }

        public FuelViewModel(Fuel fuel)
        {
            this.fuel = fuel;
        }

        public Fuel GetDataObject()
        {
            return fuel;
        }

        public int Id
        {
            get
            {
                return fuel.Id;
            }
            set
            {
                fuel.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return fuel.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    fuel.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Type
        {
            get
            {
                return fuel.Type;
            }
            set
            {
                if (value == null)
                    fuel.Type = "";
                else
                    fuel.Type = value;
                
            }
        }

        public double Min
        {
            get
            {
                return fuel.Min;
            }
            set
            {
                fuel.Min = value;
                RaisePropertyChanged("Min");
            }
        }

        public double Capacity
        {
            get
            {
                return fuel.Capacity;
            }
            set
            {
                fuel.Capacity = value;
                RaisePropertyChanged("Capacity");
            }
        }


        public double Cost
        {
            get
            {
                return fuel.Cost;
            }
            set
            {
                fuel.Cost = value;
                RaisePropertyChanged("Cost");
            }
        }

        public double TransportCost
        {
            get
            {
                return fuel.TransportCost;
            }
            set
            {
                fuel.TransportCost = value;
                RaisePropertyChanged("TransportCost");
            }
        }
    }
}
