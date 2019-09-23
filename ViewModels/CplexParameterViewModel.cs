using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.Generic;

namespace DHOG_WPF.ViewModels
{
    public class CplexParametersCollectionViewModel: CollectionBaseViewModel
    {
        public CplexParametersCollectionViewModel()
        {
            List<CplexParameter> dataObjects = CplexParametersDataAccess.GetObjects();
            foreach (CplexParameter dataObject in dataObjects)
                Add(new CplexParameterViewModel(dataObject));
        }
    }

    public class CplexParameterViewModel : BaseViewModel
    {
        CplexParameter cplexParameter;

        public CplexParameterViewModel(CplexParameter cplexParameter)
        {
            this.cplexParameter = cplexParameter;
        }

        public CplexParameter GetDataObject()
        {
            return cplexParameter;
        }

        public string Name
        {
            get
            {
                return cplexParameter.Name;
            }
        }

        public double Value
        {
            get
            {
                return cplexParameter.Value;
            }
            set
            {
                cplexParameter.Value = value;
                CplexParametersDataAccess.UpdateObject(cplexParameter);
                RaisePropertyChanged("Value");
            }
        }

        public string Description
        {
            get
            {
                return cplexParameter.Description;
            }
        }
    }
}
