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
    public class UnidadPeriodoDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnidadPeriodoDataProvider));

        public UnidadPeriodoCollectionViewModel GetObjects()
        {
            UnidadPeriodoCollectionViewModel UIObjects = new UnidadPeriodoCollectionViewModel();

            //   List<UnidadPeriodo> dataObjects = UnidadPeriodoDataAccess.GetUnidadPeriodo();
            List<UnidadPeriodo> dataObjects = UnidadPeriodoDataAccess.GetUnidadPeriodo();
            foreach (UnidadPeriodo dataObject in dataObjects)
                UIObjects.Add(new UnidadPeriodoViewModel(dataObject));
                 

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
                        UnidadPeriodoViewModel UIObject = item as UnidadPeriodoViewModel;
                        UnidadPeriodoDataAccess.DeleteUnidadPeriodo(UIObject.GetDataObject());
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
            UnidadPeriodoViewModel UIObject = sender as UnidadPeriodoViewModel;

            try
            {
                UnidadPeriodoDataAccess.UpdateUnidadPeriodo(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
