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
    public class RecursoRampaDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoRampaDataProvider));

        public RecursoRampaCollectionViewModel GetObjects()
        {
            RecursoRampaCollectionViewModel UIObjects = new RecursoRampaCollectionViewModel();

            //   List<RecursoRampa> dataObjects = RecursoRampaDataAccess.GetRecursoRampa();
            List<RecursoRampa> dataObjects = RecursoRampaDataAccess.GetRecursoRampa();
            foreach (RecursoRampa dataObject in dataObjects)
                UIObjects.Add(new RecursoRampaViewModel(dataObject));
                 

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
                        RecursoRampaViewModel UIObject = item as RecursoRampaViewModel;
                        RecursoRampaDataAccess.DeleteRecursoRampa(UIObject.GetDataObject());
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
            RecursoRampaViewModel UIObject = sender as RecursoRampaViewModel;

            try
            {
                RecursoRampaDataAccess.UpdateRecursoRampa(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
