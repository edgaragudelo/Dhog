using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.Generic;

namespace DHOG_WPF.ViewModels
{
    public class ExecutionParametersViewModel : BaseViewModel
    {
        ExecutionParameters executionParameters;
        string executionStatus;
        bool isCaseEnabled;
        //public static bool BooleanTrue = true;
        //public static bool BooleanFalse = false;

        public ExecutionParametersViewModel()
        {
            executionParameters = ExecutionParameterssDataAccess.GetExecutionParameters();
            IsCaseEnabled = !Convert.ToBoolean(executionParameters.IsIterative);
        }

        public ExecutionParameters GetDataObject()
        {
            return executionParameters;
        }

        public int InitialPeriod
        {
            get
            {
                return executionParameters.InitialPeriod;
            }
            set
            {
                executionParameters.InitialPeriod = value;
                ExecutionParameterssDataAccess.UpdateInitialPeriod(value);
                RaisePropertyChanged("InitialPeriod");
            }
        }

        public int FinalPeriod
        {
            get
            {
                return executionParameters.FinalPeriod;
            }
            set
            {
                executionParameters.FinalPeriod = value;
                ExecutionParameterssDataAccess.UpdateFinalPeriod(value);
                RaisePropertyChanged("FinalPeriod");
            }
        }

        public ObjectiveFunctionType ObjectiveFunction
        {
            get
            {
                switch (executionParameters.ObjectiveFunction)
                {
                    case 1:
                        return ObjectiveFunctionType.MinCosts;
                    case 2:
                        return ObjectiveFunctionType.MaxProfit;
                    case 3:
                        return ObjectiveFunctionType.MaxEnergy;
                    default:
                        return ObjectiveFunctionType.MinCosts;
                }
            }
            set
            {
                int intValue = 1;
                switch (value)
                {
                    case ObjectiveFunctionType.MinCosts:
                        executionParameters.ObjectiveFunction = 1;
                        intValue = 1;
                        break;
                    case ObjectiveFunctionType.MaxProfit:
                        executionParameters.ObjectiveFunction = 2;
                        intValue = 2;
                        break;
                    case ObjectiveFunctionType.MaxEnergy:
                        executionParameters.ObjectiveFunction = 3;
                        intValue = 3;
                        break;
                }
                
                ExecutionParameterssDataAccess.UpdateObjectiveFuncion(intValue);
                RaisePropertyChanged("ObjectiveFunction");
            }
        }

        public int Case
        {
            get
            {
                return executionParameters.Case;
            }
            set
            {
                executionParameters.Case = value;
                ExecutionParameterssDataAccess.UpdateCase(value);
                RaisePropertyChanged("Case");
            }
        }

        public bool IsIterative
        {
            get
            {
                return Convert.ToBoolean(executionParameters.IsIterative);
            }
            set
            {
                executionParameters.IsIterative = Convert.ToInt32(value);
                IsCaseEnabled = !value;
                ExecutionParameterssDataAccess.UpdateIsIterative(executionParameters.IsIterative);
                RaisePropertyChanged("IsIterative");
            }
        }

        public String ExecutionStatus
        {
            get
            {
                return executionStatus;
            }
            set
            {
                executionStatus = value;
                RaisePropertyChanged("ExecutionStatus");
            }
        }

        public bool IsCaseEnabled
        {
            get
            {
                return isCaseEnabled;
            }
            set
            {
                isCaseEnabled = value;
                RaisePropertyChanged("IsCaseEnabled");
            }
        }
    }

    public enum ObjectiveFunctionType
    {
        MinCosts, 
        MaxProfit, 
        MaxEnergy
    }
}
