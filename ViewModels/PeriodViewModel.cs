using DHOG_WPF.Models;
using System;
using System.Collections.ObjectModel;

namespace DHOG_WPF.ViewModels
{
    public class PeriodsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class PeriodViewModel : BaseViewModel
    {
        Period basicPeriod;

        /* Constructor needed to add rows in the RadGridView control */
        public PeriodViewModel()
        {
           // public ObservableCollection<Period> Periods { get; set; }
            basicPeriod = new Period();
        }

        public PeriodViewModel(Period basicPeriod)
        {
            this.basicPeriod = basicPeriod;
        }

        public Period GetDataObject()
        {
            return basicPeriod;
        }

        public int Id
        {
            get
            {
                return basicPeriod.Id;
            }
            set
            {
                basicPeriod.Id = value;
            }
        }

        public string Date
        {
            get
            {
                return basicPeriod.Date;
            }
            set
            {
                basicPeriod.Date = value;
                RaisePropertyChanged("Date");
            }
        }

        public int Name
        {
            get
            {
                return basicPeriod.Name;
            }
            set
            {
                basicPeriod.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public double Load
        {
            get
            {
                return basicPeriod.Load;
            }
            set
            {
                basicPeriod.Load = value;
                RaisePropertyChanged("Load");
            }
        }

        public double HourlyDuration
        {
            get
            {
                return basicPeriod.HourlyDuration;
            }
            set
            {
                basicPeriod.HourlyDuration = value;
                RaisePropertyChanged("HourlyDuration");
            }
        }


        public double AGCReservoir
        {
            get
            {
                return basicPeriod.AGCReservoir;
            }
            set
            {
                basicPeriod.AGCReservoir = value;
                RaisePropertyChanged("AGCReservoir");
            }
        }

        public double RationingCost
        {
            get
            {
                return basicPeriod.RationingCost;
            }
            set
            {
                basicPeriod.RationingCost = value;
                RaisePropertyChanged("RationingCost");
            }
        }

        public double CAR
        {
            get
            {
                return basicPeriod.CAR;
            }
            set
            {
                basicPeriod.CAR = value;
                RaisePropertyChanged("CAR");
            }
        }

        public double InternationalLoad
        {
            get
            {
                return basicPeriod.InternationalLoad;
            }
            set
            {
                basicPeriod.InternationalLoad = value;
                RaisePropertyChanged("InternationalLoad");
            }
        }

        public double DiscountRate
        {
            get
            {
                return basicPeriod.DiscountRate;
            }
            set
            {
                basicPeriod.DiscountRate = value;
                RaisePropertyChanged("DiscountRate");
            }
        }

        public int Case
        {
            get
            {
                return basicPeriod.Case;
            }
            set
            {
                basicPeriod.Case = value;
                RaisePropertyChanged("Case");
            }
        }
    }
}
