using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicCompaniesCollectionViewModel : PeriodicEntityCollectionViewModel
    {
        
    }

    public class PeriodicCompanyViewModel: PeriodicEntityViewModel
    {
        PeriodicCompany periodicCompany;

        public PeriodicCompanyViewModel(PeriodicCompany periodicCompany): base(periodicCompany)
        {
            this.periodicCompany = periodicCompany;
        }

        public PeriodicCompany GetDataObject()
        {
            return periodicCompany;
        }

        public double StockPrice
        {
            get
            {
                return periodicCompany.StockPrice;
            }
            set
            {
                periodicCompany.StockPrice = value;
                RaisePropertyChanged("StockPrice");
            }
        }

        public double Contract
        {
            get
            {
                return periodicCompany.Contract;
            }
            set
            {
                periodicCompany.Contract = value;
                RaisePropertyChanged("Contract");
            }
        }
    }
}
