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
    public class CortePeriodoDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CortePeriodoDataProvider));

        public CortePeriodoCollectionViewModel GetObjects()
        {
            CortePeriodoCollectionViewModel UIObjects = new CortePeriodoCollectionViewModel();

            //   List<CortePeriodo> dataObjects = CortePeriodoDataAccess.GetCortePeriodo();
            List<CortePeriodo> dataObjects = CortePeriodoDataAccess.GetCortePeriodo();
            foreach (CortePeriodo dataObject in dataObjects)
                UIObjects.Add(new CortePeriodoViewModel(dataObject));
                 

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
                        CortePeriodoViewModel UIObject = item as CortePeriodoViewModel;
                        CortePeriodoDataAccess.DeleteCortePeriodo(UIObject.GetDataObject());
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
            CortePeriodoViewModel UIObject = sender as CortePeriodoViewModel;

            try
            {
                CortePeriodoDataAccess.UpdateCortePeriodo(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
