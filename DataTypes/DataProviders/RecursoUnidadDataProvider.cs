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
    public class RecursoUnidadDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoUnidadDataProvider));

        public RecursoUnidadCollectionViewModel GetObjects()
        {
            RecursoUnidadCollectionViewModel UIObjects = new RecursoUnidadCollectionViewModel();

            //   List<RecursoUnidad> dataObjects = RecursoUnidadDataAccess.GetRecursoUnidad();
            List<RecursoUnidad> dataObjects = RecursoUnidadDataAccess.GetRecursoUnidad();
            foreach (RecursoUnidad dataObject in dataObjects)
                UIObjects.Add(new RecursoUnidadViewModel(dataObject));
                 

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
                        RecursoUnidadViewModel UIObject = item as RecursoUnidadViewModel;
                        RecursoUnidadDataAccess.DeleteRecursoUnidad(UIObject.GetDataObject());
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
            RecursoUnidadViewModel UIObject = sender as RecursoUnidadViewModel;

            try
            {
                RecursoUnidadDataAccess.UpdateRecursoUnidad(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
