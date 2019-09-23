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
    public class ExcludingPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExcludingPlantsDataProvider));
        private ExcludingPlantsCollectionViewModel UIObjects;

        public ExcludingPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new ExcludingPlantsCollectionViewModel();

            List<ExcludingPlants> dataObjects = ExcludingPlantsDataAccess.GetObjects();
            foreach (ExcludingPlants dataObject in dataObjects)
                UIObjects.Add(new ExcludingPlantsViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new ExcludingPlantsViewModel());

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
                        ExcludingPlantsViewModel UIObject = item as ExcludingPlantsViewModel;
                        if (UIObject.Plant1 != null && UIObject.Plant2 != null)
                            ExcludingPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            ExcludingPlantsViewModel UIObject = sender as ExcludingPlantsViewModel;

            try
            {
                if (UIObject.Plant1 != null && UIObject.Plant2 != null)
                    ExcludingPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
            }
            catch
            {
                throw;
            }
        }
    }
}
