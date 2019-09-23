using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class HydroElementsCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class HydroElementViewModel : BaseViewModel
    {
        HydroElement hydroElement;

        /* Constructor needed to add rows in the RadGridView control */
        public HydroElementViewModel() {
            hydroElement = new HydroElement();
        } 

        public HydroElementViewModel(HydroElement hydroElement)
        {
            this.hydroElement = hydroElement;
        }

        public HydroElement GetDataObject()
        {
            return hydroElement;
        }

        public int Id
        {
            get
            {
                return hydroElement.Id;
            }
            set
            {
                hydroElement.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return hydroElement.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    hydroElement.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double MinTurbinedOutflow
        {
            get
            {
                return hydroElement.MinTurbinedOutflow;
            }
            set
            {
                hydroElement.MinTurbinedOutflow = value;
                RaisePropertyChanged("MinTurbinedOutflow");
            }
        }

        public double MaxTurbinedOutflow
        {
            get
            {
                return hydroElement.MaxTurbinedOutflow;
            }
            set
            {
                hydroElement.MaxTurbinedOutflow = value;
                RaisePropertyChanged("MaxTurbinedOutflow");
            }
        }


        public int StartPeriod
        {
            get
            {
                return hydroElement.StartPeriod;
            }
            set
            {
                hydroElement.StartPeriod = value;
                RaisePropertyChanged("StartPeriod");
            }
        }

        public double Filtration
        {
            get
            {
                return hydroElement.Filtration;
            }
            set
            {
                hydroElement.Filtration = value;
                RaisePropertyChanged("Filtration");
            }
        }

        public double RecoveryFactor
        {
            get
            {
                return hydroElement.RecoveryFactor;
            }
            set
            {
                hydroElement.RecoveryFactor = value;
                RaisePropertyChanged("RecoveryFactor");
            }
        }
    }
}
