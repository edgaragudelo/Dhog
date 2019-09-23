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
    public class PeriodicNonConventionalPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicNonConventionalPlantsDataProvider));

        public PeriodicNonConventionalPlantsCollectionViewModel GetObjects()
        {
            PeriodicNonConventionalPlantsCollectionViewModel UIObjects = new PeriodicNonConventionalPlantsCollectionViewModel();

            List<PeriodicNonConventionalPlant> dataObjects = PeriodicNonConventionalPlantsDataAccess.GetPeriodicNonConventionalPlants();
            foreach (PeriodicNonConventionalPlant dataObject in dataObjects)
                UIObjects.Add(new PeriodicNonConventionalPlantsViewModel(dataObject));
            
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
                        PeriodicNonConventionalPlantsViewModel UIObject = item as PeriodicNonConventionalPlantsViewModel;
                        PeriodicNonConventionalPlantsDataAccess.DeletePeriodicNonConventionalPlant(UIObject.GetDataObject());
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
            PeriodicNonConventionalPlantsViewModel UIObject = sender as PeriodicNonConventionalPlantsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicNonConventionalPlantsDataAccess.UpdatePeriodicNonConventionalPlant(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
