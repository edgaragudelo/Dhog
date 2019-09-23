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
    public class PeriodicFuelsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicFuelsDataProvider));

        public PeriodicFuelsCollectionViewModel GetObjects()
        {
            PeriodicFuelsCollectionViewModel UIObjects = new PeriodicFuelsCollectionViewModel();

            List<PeriodicFuel> dataObjects = PeriodicFuelsDataAccess.GetPeriodicFuels();
            foreach (PeriodicFuel dataObject in dataObjects)
                UIObjects.Add(new PeriodicFuelsViewModel(dataObject));
            
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
                        PeriodicFuelsViewModel UIObject = item as PeriodicFuelsViewModel;
                        PeriodicFuelsDataAccess.DeletePeriodicFuel(UIObject.GetDataObject());
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
            PeriodicFuelsViewModel UIObject = sender as PeriodicFuelsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicFuelsDataAccess.UpdatePeriodicFuel(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
