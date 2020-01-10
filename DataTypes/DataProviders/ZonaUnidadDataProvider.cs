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
    public class ZonaUnidadDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ZonaUnidadDataProvider));

        public ZonaUnidadCollectionViewModel GetObjects()
        {
            ZonaUnidadCollectionViewModel UIObjects = new ZonaUnidadCollectionViewModel();

            //   List<ZonaUnidad> dataObjects = ZonaUnidadDataAccess.GetZonaUnidad();
            List<ZonaUnidad> dataObjects = ZonaUnidadDataAccess.GetZonaUnidad();
            foreach (ZonaUnidad dataObject in dataObjects)
                UIObjects.Add(new ZonaUnidadViewModel(dataObject));
                 

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
                        ZonaUnidadViewModel UIObject = item as ZonaUnidadViewModel;
                        ZonaUnidadDataAccess.DeleteZonaUnidad(UIObject.GetDataObject());
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
            ZonaUnidadViewModel UIObject = sender as ZonaUnidadViewModel;

            try
            {
                ZonaUnidadDataAccess.UpdateZonaUnidad(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
