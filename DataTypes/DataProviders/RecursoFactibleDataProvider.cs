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
    public class RecursoFactibleDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoFactibleDataProvider));

        public RecursoFactibleCollectionViewModel GetObjects()
        {
            RecursoFactibleCollectionViewModel UIObjects = new RecursoFactibleCollectionViewModel();

            //   List<RecursoFactible> dataObjects = RecursoFactibleDataAccess.GetRecursoFactible();
            List<RecursoFactible> dataObjects = RecursoFactibleDataAccess.GetRecursoFactible();
            foreach (RecursoFactible dataObject in dataObjects)
                UIObjects.Add(new RecursoFactibleViewModel(dataObject));
                 

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
                        RecursoFactibleViewModel UIObject = item as RecursoFactibleViewModel;
                        RecursoFactibleDataAccess.DeleteRecursoFactible(UIObject.GetDataObject());
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
            RecursoFactibleViewModel UIObject = sender as RecursoFactibleViewModel;

            try
            {
                RecursoFactibleDataAccess.UpdateRecursoFactible(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
