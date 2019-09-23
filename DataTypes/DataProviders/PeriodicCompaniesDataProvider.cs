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
    public class PeriodicCompaniesDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicCompaniesDataProvider));

        public PeriodicCompaniesCollectionViewModel GetObjects()
        {
            PeriodicCompaniesCollectionViewModel UIObjects = new PeriodicCompaniesCollectionViewModel();

            List<PeriodicCompany> dataObjects = PeriodicCompaniesDataAccess.GetPeriodicCompanies();
            foreach (PeriodicCompany dataObject in dataObjects)
                UIObjects.Add(new PeriodicCompanyViewModel(dataObject));
            
            UIObjects.ItemEndEdit += new ItemEndEditEventHandler(ObjectsItemEndEdit);
            UIObjects.CollectionChanged += new NotifyCollectionChangedEventHandler(ObjectsCollectionChanged);

            return UIObjects;
        }

        void ObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    foreach (object item in e.OldItems)
                    {
                        PeriodicCompanyViewModel UIObject = item as PeriodicCompanyViewModel;
                        PeriodicCompaniesDataAccess.DeletePeriodicCompany(UIObject.GetDataObject());
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.StackTrace);
                }
            }
        }

        void ObjectsItemEndEdit(IEditableObject sender)
        {
            PeriodicCompanyViewModel UIObject = sender as PeriodicCompanyViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicCompaniesDataAccess.UpdatePeriodicCompany(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
