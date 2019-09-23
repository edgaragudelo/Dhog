using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicAreasCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicAreaViewModel: PeriodicEntityViewModel
    {
        PeriodicArea periodicArea;

        public PeriodicAreaViewModel(PeriodicArea periodicArea): base(periodicArea)
        {
            this.periodicArea = periodicArea;
        }

        public PeriodicArea GetDataObject()
        {
            return periodicArea;
        }

        public double Load
        {
            get
            {
                return periodicArea.Load;
            }
            set
            {
                periodicArea.Load = value;
                RaisePropertyChanged("Load");
            }
        }

        public double ImportationLimit
        {
            get
            {
                return periodicArea.ImportationLimit;
            }
            set
            {
                periodicArea.ImportationLimit = value;
                RaisePropertyChanged("ImportationLimit");
            }
        }

        public double ExportationLimit
        {
            get
            {
                return periodicArea.ExportationLimit;
            }
            set
            {
                periodicArea.ExportationLimit = value;
                RaisePropertyChanged("ExportationLimit");
            }
        }
    }
}
