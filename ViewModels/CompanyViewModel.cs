using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class CompaniesCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class CompanyViewModel : BaseViewModel
    {
        Company company;

        /* Constructor needed to add rows in the RadGridView control */
        public CompanyViewModel() {
            company = new Company();
            Case = 1;
        } 

        public CompanyViewModel(Company company)
        {
            this.company = company;
        }

        public Company GetDataObject()
        {
            return company;
        }

        public int Id
        {
            get
            {
                return company.Id;
            }
            set
            {
                company.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return company.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    company.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double StockPrice
        {
            get
            {
                return company.StockPrice;
            }
            set
            {
                company.StockPrice = value;
                RaisePropertyChanged("StockPrice");
            }
        }

        public double Contract
        {
            get
            {
                return company.Contract;
            }
            set
            {
                company.Contract = value;
                RaisePropertyChanged("Contract");
            }
        }

        public bool IsContractModeled
        {
            get
            {
                return Convert.ToBoolean(company.IsContractModeled);
            }
            set
            {
                company.IsContractModeled = Convert.ToInt32(value);
                RaisePropertyChanged("IsContractModeled");
            }
        }

        public double ContractFactor
        {
            get
            {
                return company.ContractFactor;
            }
            set
            {
                company.ContractFactor = value;
                RaisePropertyChanged("ContractFactor");
            }
        }

        public double ContractPenalizationFactor
        {
            get
            {
                return company.ContractPenalizationFactor;
            }
            set
            {
                company.ContractPenalizationFactor = value;
                RaisePropertyChanged("ContractPenalizationFactor");
            }
        }

        public int Case
        {
            get
            {
                return company.Case;
            }
            set
            {
                company.Case = value;
                RaisePropertyChanged("Case");
            }
        }
    }
}
