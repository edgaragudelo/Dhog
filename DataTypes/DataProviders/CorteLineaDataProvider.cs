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
    public class CorteLineaDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CorteLineaDataProvider));

        public CorteLineaCollectionViewModel GetObjects()
        {
            CorteLineaCollectionViewModel UIObjects = new CorteLineaCollectionViewModel();

            //   List<CorteLinea> dataObjects = CorteLineaDataAccess.GetCorteLinea();
            List<CorteLinea> dataObjects = CorteLineaDataAccess.GetCorteLinea();
            foreach (CorteLinea dataObject in dataObjects)
                UIObjects.Add(new CorteLineaViewModel(dataObject));
                 

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
                        CorteLineaViewModel UIObject = item as CorteLineaViewModel;
                        CorteLineaDataAccess.DeleteCorteLinea(UIObject.GetDataObject());
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
            CorteLineaViewModel UIObject = sender as CorteLineaViewModel;

            try
            {
                CorteLineaDataAccess.UpdateCorteLinea(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
