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
    public class PeriodicBarraDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicBarraDataProvider));

        public PeriodicBarraCollectionViewModel GetObjects()
        {
            PeriodicBarraCollectionViewModel UIObjects = new PeriodicBarraCollectionViewModel();

            //   List<PeriodicBarra> dataObjects = PeriodicBarraDataAccess.GetPeriodicBarra();
            List<PeriodicBarra> dataObjects = PeriodicBarraDataAccess.GetPeriodicBarra();
            foreach (PeriodicBarra dataObject in dataObjects)
                UIObjects.Add(new PeriodicBarraViewModel(dataObject));
                 

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
                        PeriodicBarraViewModel UIObject = item as PeriodicBarraViewModel;
                        PeriodicBarraDataAccess.DeletePeriodicBarra(UIObject.GetDataObject());
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
            PeriodicBarraViewModel UIObject = sender as PeriodicBarraViewModel;

            try
            {
                PeriodicBarraDataAccess.UpdatePeriodicBarra(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
