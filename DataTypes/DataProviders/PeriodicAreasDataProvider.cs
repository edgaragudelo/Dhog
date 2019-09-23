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
    public class PeriodicAreasDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicAreasDataProvider));

        public PeriodicAreasCollectionViewModel GetObjects()
        {
            PeriodicAreasCollectionViewModel UIObjects = new PeriodicAreasCollectionViewModel();

            List<PeriodicArea> dataObjects = PeriodicAreasDataAccess.GetPeriodicAreas();
            foreach (PeriodicArea dataObject in dataObjects)
                UIObjects.Add(new PeriodicAreaViewModel(dataObject));
            
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
                        PeriodicAreaViewModel UIObject = item as PeriodicAreaViewModel;
                        PeriodicAreasDataAccess.DeletePeriodicArea(UIObject.GetDataObject());
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
            PeriodicAreaViewModel UIObject = sender as PeriodicAreaViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicAreasDataAccess.UpdatePeriodicArea(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
