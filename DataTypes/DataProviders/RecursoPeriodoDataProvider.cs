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
    public class RecursoPeriodoDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoPeriodoDataProvider));

        public RecursoPeriodoCollectionViewModel GetObjects()
        {
            RecursoPeriodoCollectionViewModel UIObjects = new RecursoPeriodoCollectionViewModel();

            //   List<RecursoPeriodo> dataObjects = RecursoPeriodoDataAccess.GetRecursoPeriodo();
            List<RecursoPeriodo> dataObjects = RecursoPeriodoDataAccess.GetRecursoPeriodo();
            foreach (RecursoPeriodo dataObject in dataObjects)
                UIObjects.Add(new RecursoPeriodoViewModel(dataObject));
                 

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
                        RecursoPeriodoViewModel UIObject = item as RecursoPeriodoViewModel;
                        RecursoPeriodoDataAccess.DeleteRecursoPeriodo(UIObject.GetDataObject());
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
            RecursoPeriodoViewModel UIObject = sender as RecursoPeriodoViewModel;

            try
            {
                RecursoPeriodoDataAccess.UpdateRecursoPeriodo(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
