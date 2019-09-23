using DHOG_WPF.Models;
using System;


namespace DHOG_WPF.ViewModels
{
    public class ScenariosActivosCollectionViewModel : CollectionBaseViewModel
    {
        public int GetActiveScenariosQuantity1()
        {
            int quantity = 1;
            foreach (ScenariosActivosViewModel scenario in Items)
                //if (scenario.IsActive)
                //    quantity *= scenario.CasesQuantity;
                quantity=scenario.CasesQuantity;

            return quantity;
        }
    }

    public class ScenariosActivosViewModel: BaseViewModel
    {
        ScenariosActivos scenario;

        public ScenariosActivosViewModel(ScenariosActivos scenario)
        {
            this.scenario = scenario;
        }

        public ScenariosActivos GetDataObject()
        {
            return scenario;
        }

        public string Variable
        {
            get
            {
                return scenario.Variable;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    scenario.Variable = value;
                    RaisePropertyChanged("Variable");
                }
            }
        }

        public int CasesQuantity
        {
            get
            {
                return scenario.CasesQuantity;
            }
            set
            {
                scenario.CasesQuantity = value;
                RaisePropertyChanged("CasesQuantity");
            }
        }

        public bool IsActive
        {
            get
            {
                return Convert.ToBoolean(scenario.IsActive);
            }
            set
            {
                scenario.IsActive = Convert.ToInt32(value);
                RaisePropertyChanged("IsActive");
            }
        }

        public int TreePeriod
        {
            get
            {
                return scenario.TreePeriod;
            }
            set
            {
                scenario.TreePeriod = value;
                RaisePropertyChanged("TreePeriod");
            }
        }
    }
}
