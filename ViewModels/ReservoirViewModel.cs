using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class ReservoirsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class ReservoirViewModel : BaseViewModel
    {
        Reservoir reservoir;

        /* Constructor needed to add rows in the RadGridView control */
        public ReservoirViewModel()
        {
            reservoir = new Reservoir();
            Case = 1;
            Company = "";
            StartPeriod = 1;
        }

        public ReservoirViewModel(Reservoir reservoir)
        {
            this.reservoir = reservoir;
        }

        public Reservoir GetDataObject()
        {
            return reservoir;
        }

        public int Id
        {
            get
            {
                return reservoir.Id;
            }
            set
            {
                reservoir.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return reservoir.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    reservoir.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Company
        {
            get
            {
                return reservoir.Company;
            }
            set
            {
                if (value == null)
                    reservoir.Company = "";
                else
                    reservoir.Company = value;
                
            }
        }

        public double MinLevel
        {
            get
            {
                return reservoir.MinLevel;
            }
            set
            {
                reservoir.MinLevel = value;
                RaisePropertyChanged("MinLevel");
            }
        }

        public double MaxLevel
        {
            get
            {
                return reservoir.MaxLevel;
            }
            set
            {
                reservoir.MaxLevel = value;
                RaisePropertyChanged("MaxLevel");
            }
        }


        public double InitialLevel
        {
            get
            {
                return reservoir.InitialLevel;
            }
            set
            {
                reservoir.InitialLevel = value;
                RaisePropertyChanged("InitialLevel");
            }
        }

        public double FinalLevel
        {
            get
            {
                return reservoir.FinalLevel;
            }
            set
            {
                reservoir.FinalLevel = value;
                RaisePropertyChanged("FinalLevel");
            }
        }

        public double Filtration
        {
            get
            {
                return reservoir.Filtration;
            }
            set
            {
                reservoir.Filtration = value;
                RaisePropertyChanged("Filtration");
            }
        }

        public double RecoveryFactor
        {
            get
            {
                return reservoir.RecoveryFactor;
            }
            set
            {
                reservoir.RecoveryFactor = value;
                RaisePropertyChanged("RecoveryFactor ");
            }
        }

        public double SpillagePenalizationFactor
        {
            get
            {
                return reservoir.SpillagePenalizationFactor;
            }
            set
            {
                reservoir.SpillagePenalizationFactor = value;
                RaisePropertyChanged("SpillagePenalizationFactor");
            }
        }

        public int StartPeriod
        {
            get
            {
                return reservoir.StartPeriod;
            }
            set
            {
                reservoir.StartPeriod = value;
                RaisePropertyChanged("StartPeriod");
            }
        }

        public int Case
        {
            get
            {
                return reservoir.Case;
            }
            set
            {
                reservoir.Case = value;
                RaisePropertyChanged("Case");
            }
        }
    }
}
