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
    public class UnidadBarraDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnidadBarraDataProvider));

        public UnidadBarraCollectionViewModel GetObjects()
        {
            UnidadBarraCollectionViewModel UIObjects = new UnidadBarraCollectionViewModel();

            //   List<UnidadBarra> dataObjects = UnidadBarraDataAccess.GetUnidadBarra();
            List<UnidadBarra> dataObjects = UnidadBarraDataAccess.GetUnidadBarra();
            foreach (UnidadBarra dataObject in dataObjects)
                UIObjects.Add(new UnidadBarraViewModel(dataObject));
                 

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
                        UnidadBarraViewModel UIObject = item as UnidadBarraViewModel;
                        UnidadBarraDataAccess.DeleteUnidadBarra(UIObject.GetDataObject());
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
            UnidadBarraViewModel UIObject = sender as UnidadBarraViewModel;

            try
            {
                UnidadBarraDataAccess.UpdateUnidadBarra(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
