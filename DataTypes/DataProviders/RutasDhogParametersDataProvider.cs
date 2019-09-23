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
    public class RutasDhogParametersDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RutasDhogParametersDataProvider));

        public static RutasDhogParametersCollectionViewModel GetConstraints()
        {
            RutasDhogParametersCollectionViewModel UIObjects = new RutasDhogParametersCollectionViewModel();

            List<RutasDhogParameter> dataObjects = RutasDhogParametersDataAccess.GetObjects();
            foreach (RutasDhogParameter dataObject in dataObjects)
                UIObjects.Add(new RutasDhogParameterViewModel(dataObject));

            return UIObjects;
        }

        public static RutasDhogParametersCollectionViewModel GetConfigurationParameters()
        {
            RutasDhogParametersCollectionViewModel UIObjects = new RutasDhogParametersCollectionViewModel();

            List<RutasDhogParameter> dataObjects = RutasDhogParametersDataAccess.GetObjects();
            foreach (RutasDhogParameter dataObject in dataObjects)
                UIObjects.Add(new RutasDhogParameterViewModel(dataObject));

            return UIObjects;
        }
    }
}
