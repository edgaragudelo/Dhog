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
    public class RecursoBasicaDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoBasicaDataProvider));

        public RecursoBasicaCollectionViewModel GetObjects()
        {
            RecursoBasicaCollectionViewModel UIObjects = new RecursoBasicaCollectionViewModel();

            //   List<RecursoBasica> dataObjects = RecursoBasicaDataAccess.GetRecursoBasica();
            List<RecursoBasica> dataObjects = RecursoBasicaDataAccess.GetRecursoBasica();
            foreach (RecursoBasica dataObject in dataObjects)
                UIObjects.Add(new RecursoBasicaViewModel(dataObject));
                 

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
                        RecursoBasicaViewModel UIObject = item as RecursoBasicaViewModel;
                        RecursoBasicaDataAccess.DeleteRecursoBasica(UIObject.GetDataObject());
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
            RecursoBasicaViewModel UIObject = sender as RecursoBasicaViewModel;

            try
            {
                RecursoBasicaDataAccess.UpdateRecursoBasica(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
