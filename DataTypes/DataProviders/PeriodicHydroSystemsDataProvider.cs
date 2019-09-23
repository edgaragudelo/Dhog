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
    public class PeriodicHydroSystemsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicHydroSystemsDataProvider));

        public PeriodicHydroSystemsCollectionViewModel GetObjects()
        {
            PeriodicHydroSystemsCollectionViewModel UIObjects = new PeriodicHydroSystemsCollectionViewModel();

            List<PeriodicHydroSystem> dataObjects = PeriodicHydroSystemsDataAccess.GetPeriodicHydroSystems();
            foreach (PeriodicHydroSystem dataObject in dataObjects)
                UIObjects.Add(new PeriodicHydroSystemsViewModel(dataObject));
            
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
                        PeriodicHydroSystemsViewModel UIObject = item as PeriodicHydroSystemsViewModel;
                        PeriodicHydroSystemsDataAccess.DeletePeriodicHydroSystem(UIObject.GetDataObject());
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
            PeriodicHydroSystemsViewModel UIObject = sender as PeriodicHydroSystemsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicHydroSystemsDataAccess.UpdatePeriodicHydroSystem(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
