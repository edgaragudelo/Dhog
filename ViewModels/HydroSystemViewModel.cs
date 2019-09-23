using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class HydroSystemsCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class HydroSystemViewModel : BaseViewModel
    {
        HydroSystem hydroSystem;

        /* Constructor needed to add rows in the RadGridView control */
        public HydroSystemViewModel() {
            hydroSystem = new HydroSystem();
        } 

        public HydroSystemViewModel(HydroSystem hydroSystem)
        {
            this.hydroSystem = hydroSystem;
        }

        public HydroSystem GetDataObject()
        {
            return hydroSystem;
        }

        public int Id
        {
            get
            {
                return hydroSystem.Id;
            }
            set
            {
                hydroSystem.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return hydroSystem.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    hydroSystem.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double MinTurbinedOutflow
        {
            get
            {
                return hydroSystem.MinTurbinedOutflow;
            }
            set
            {
                hydroSystem.MinTurbinedOutflow = value;
                RaisePropertyChanged("MinTurbinedOutflow");
            }
        }

        public double MaxTurbinedOutflow
        {
            get
            {
                return hydroSystem.MaxTurbinedOutflow;
            }
            set
            {
                hydroSystem.MaxTurbinedOutflow = value;
                RaisePropertyChanged("MaxTurbinedOutflow");
            }
        }


        public int StartPeriod
        {
            get
            {
                return hydroSystem.StartPeriod;
            }
            set
            {
                hydroSystem.StartPeriod = value;
                RaisePropertyChanged("StartPeriod");
            }
        }

        public double EnergyFactor
        {
            get
            {
                return hydroSystem.EnergyFactor;
            }
            set
            {
                hydroSystem.EnergyFactor = value;
                RaisePropertyChanged("EnergyFactor");
            }
        }
    }
}
