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
    public class PeriodicFuelContractsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicFuelContractsDataProvider));

        public PeriodicFuelContractsCollectionViewModel GetObjects()
        {
            PeriodicFuelContractsCollectionViewModel UIObjects = new PeriodicFuelContractsCollectionViewModel();

            List<PeriodicFuelContract> dataObjects = PeriodicFuelContractsDataAccess.GetPeriodicFuelContracts();
            foreach (PeriodicFuelContract dataObject in dataObjects)
                UIObjects.Add(new PeriodicFuelContractsViewModel(dataObject));
            
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
                        PeriodicFuelContractsViewModel UIObject = item as PeriodicFuelContractsViewModel;
                        PeriodicFuelContractsDataAccess.DeletePeriodicFuelContract(UIObject.GetDataObject());
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
            PeriodicFuelContractsViewModel UIObject = sender as PeriodicFuelContractsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicFuelContractsDataAccess.UpdatePeriodicFuelContract(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
