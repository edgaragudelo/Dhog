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
    public class PeriodicHydroElementsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicHydroElementsDataProvider));

        public PeriodicHydroElementsCollectionViewModel GetObjects()
        {
            PeriodicHydroElementsCollectionViewModel UIObjects = new PeriodicHydroElementsCollectionViewModel();

            List<PeriodicHydroElement> dataObjects = PeriodicHydroElementsDataAccess.GetPeriodicHydroElements();
            foreach (PeriodicHydroElement dataObject in dataObjects)
                UIObjects.Add(new PeriodicHydroElementsViewModel(dataObject));
            
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
                        PeriodicHydroElementsViewModel UIObject = item as PeriodicHydroElementsViewModel;
                        PeriodicHydroElementsDataAccess.DeletePeriodicHydroElement(UIObject.GetDataObject());
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
            PeriodicHydroElementsViewModel UIObject = sender as PeriodicHydroElementsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicHydroElementsDataAccess.UpdatePeriodicHydroElement(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
