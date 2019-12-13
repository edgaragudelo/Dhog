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
    public class RecursoPrecioDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoPrecioDataProvider));

        public RecursoPrecioCollectionViewModel GetObjects()
        {
            RecursoPrecioCollectionViewModel UIObjects = new RecursoPrecioCollectionViewModel();

            //   List<RecursoPrecio> dataObjects = RecursoPrecioDataAccess.GetRecursoPrecio();
            List<RecursoPrecio> dataObjects = RecursoPrecioDataAccess.GetRecursoPrecio();
            foreach (RecursoPrecio dataObject in dataObjects)
                UIObjects.Add(new RecursoPrecioViewModel(dataObject));
                 

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
                        RecursoPrecioViewModel UIObject = item as RecursoPrecioViewModel;
                        RecursoPrecioDataAccess.DeleteRecursoPrecio(UIObject.GetDataObject());
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
            RecursoPrecioViewModel UIObject = sender as RecursoPrecioViewModel;

            try
            {
                RecursoPrecioDataAccess.UpdateRecursoPrecio(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
