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
    public class AreaBarraDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AreaBarraDataProvider));

        public AreaBarraCollectionViewModel GetObjects()
        {
            AreaBarraCollectionViewModel UIObjects = new AreaBarraCollectionViewModel();

            //   List<AreaBarra> dataObjects = AreaBarraDataAccess.GetAreaBarra();
            List<AreaBarra> dataObjects = AreaBarraDataAccess.GetAreaBarra();
            foreach (AreaBarra dataObject in dataObjects)
                UIObjects.Add(new AreaBarraViewModel(dataObject));
                 

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
                        AreaBarraViewModel UIObject = item as AreaBarraViewModel;
                        AreaBarraDataAccess.DeleteAreaBarra(UIObject.GetDataObject());
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
            AreaBarraViewModel UIObject = sender as AreaBarraViewModel;

            try
            {
                AreaBarraDataAccess.UpdateAreaBarra(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
