using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.Generic;

namespace DHOG_WPF.ViewModels
{
    public class ProblemConfigurationParametersCollectionViewModel: CollectionBaseViewModel
    {
        public ProblemConfigurationParametersCollectionViewModel()
        {
            LoadTypes = new List<int>() { 0, 1, 2 };
            ConversionFactorTypes = new List<int>() { 0, 1, 2, 3 };
            ConsumptionFactorTypes = new List<int>() { 0, 1};
        }
        
        public List<int> LoadTypes { get; }
        public List<int> ConversionFactorTypes { get; }
        public List<int> ConsumptionFactorTypes { get; }

    }

    public class ProblemConfigurationParameterViewModel : BaseViewModel
    {
        ProblemConfigurationParameter problemConfigurationParameter;

        public ProblemConfigurationParameterViewModel(ProblemConfigurationParameter problemConfigurationParameter)
        {
            this.problemConfigurationParameter = problemConfigurationParameter;
        }

        public ProblemConfigurationParameter GetDataObject()
        {
            return problemConfigurationParameter;
        }

        public string Name
        {
            get
            {
                return problemConfigurationParameter.UIName;
            }
        }

        public string DBName
        {
            get
            {
                return problemConfigurationParameter.Name;
            }
        }

        public int Value
        {
            get
            {
                return problemConfigurationParameter.Value;
            }
            set
            {
                problemConfigurationParameter.Value = value;
                ProblemConfigurationParametersDataAccess.UpdateObject(problemConfigurationParameter);
                RaisePropertyChanged("Value");
            }
        }

        public bool BoolValue
        {
            get
            {
                return Convert.ToBoolean(problemConfigurationParameter.Value);
            }
            set
            {
                problemConfigurationParameter.Value = Convert.ToInt32(value);
                ProblemConfigurationParametersDataAccess.UpdateObject(problemConfigurationParameter);
                RaisePropertyChanged("BoolValue ");
            }
        }
    }
}
