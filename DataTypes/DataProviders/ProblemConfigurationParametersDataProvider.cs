using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DHOG_WPF.DataProviders
{
    public class ProblemConfigurationParametersDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProblemConfigurationParametersDataProvider));

        public static ProblemConfigurationParametersCollectionViewModel GetConstraints()
        {
            ProblemConfigurationParametersCollectionViewModel UIObjects = new ProblemConfigurationParametersCollectionViewModel();

            List<ProblemConfigurationParameter> dataObjects = ProblemConfigurationParametersDataAccess.GetObjects(1);
            foreach (ProblemConfigurationParameter dataObject in dataObjects)
                UIObjects.Add(new ProblemConfigurationParameterViewModel(dataObject));

            return UIObjects;
        }

        public static ProblemConfigurationParametersCollectionViewModel GetConfigurationParameters()
        {
            ProblemConfigurationParametersCollectionViewModel UIObjects = new ProblemConfigurationParametersCollectionViewModel();

            List<ProblemConfigurationParameter> dataObjects = ProblemConfigurationParametersDataAccess.GetObjects(2);
            foreach (ProblemConfigurationParameter dataObject in dataObjects)
                UIObjects.Add(new ProblemConfigurationParameterViewModel(dataObject));

            return UIObjects;
        }
    }
}
