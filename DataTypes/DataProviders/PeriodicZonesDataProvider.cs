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
    public class PeriodicZonesDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicZonesDataProvider));

        public PeriodicZonesCollectionViewModel GetObjects()
        {
            PeriodicZonesCollectionViewModel UIObjects = new PeriodicZonesCollectionViewModel();

            List<PeriodicZone> dataObjects = PeriodicZonesDataAccess.GetPeriodicZones();
            foreach (PeriodicZone dataObject in dataObjects)
                UIObjects.Add(new PeriodicZonesViewModel(dataObject));
            
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
                        PeriodicZonesViewModel UIObject = item as PeriodicZonesViewModel;
                        PeriodicZonesDataAccess.DeletePeriodicZone(UIObject.GetDataObject());
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
            PeriodicZonesViewModel UIObject = sender as PeriodicZonesViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicZonesDataAccess.UpdatePeriodicZone(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
