using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicEntityCollectionViewModel : CollectionBaseViewModel
    {
        public void SetPeriodsDate()
        {
            foreach (PeriodicEntityViewModel periodicEntity in Items)
                periodicEntity.SetPeriodDate();
        }
    }

    public class PeriodicEntityViewModel: BaseViewModel
    {
        PeriodicEntity periodicEntity;
        string periodDate;

        public PeriodicEntityViewModel(PeriodicEntity periodicEntity)
        {
            this.periodicEntity = periodicEntity;
        }

        public string Name
        {
            get
            {
                return periodicEntity.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    periodicEntity.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public int Period
        {
            get
            {
                return periodicEntity.Period;
            }
            set
            {
                periodicEntity.Period = value;
                RaisePropertyChanged("Period");
            }
        }

        public string PeriodDate
        {
            get
            {
                return periodDate;
            }
        }

        public void SetPeriodDate()
        {
            if (DHOGDataBaseViewModel.PeriodsDate != null)
            {
                if (periodicEntity.Period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                {
                    periodDate = DHOGDataBaseViewModel.PeriodsDate[periodicEntity.Period - 1];
                    RaisePropertyChanged("PeriodDate");
                }
                else
                {
                    periodDate = "";
                }
            }
        }

        public int Case
        {
            get
            {
                return periodicEntity.Case;
            }
            set
            {
                periodicEntity.Case = value;
                RaisePropertyChanged("Case");
            }
        }
    }
}
