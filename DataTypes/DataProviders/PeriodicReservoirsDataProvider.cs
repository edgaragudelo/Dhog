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
    public class PeriodicReservoirsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicReservoirsDataProvider));

        public PeriodicReservoirsCollectionViewModel GetObjects()
        {
            PeriodicReservoirsCollectionViewModel UIObjects = new PeriodicReservoirsCollectionViewModel();

            List<PeriodicReservoir> dataObjects = PeriodicReservoirsDataAccess.GetPeriodicReservoirs();
            foreach (PeriodicReservoir dataObject in dataObjects)
                UIObjects.Add(new PeriodicReservoirsViewModel(dataObject));
            
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
                        PeriodicReservoirsViewModel UIObject = item as PeriodicReservoirsViewModel;
                        PeriodicReservoirsDataAccess.DeletePeriodicReservoir(UIObject.GetDataObject());
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
            PeriodicReservoirsViewModel UIObject = sender as PeriodicReservoirsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicReservoirsDataAccess.UpdatePeriodicReservoir(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
