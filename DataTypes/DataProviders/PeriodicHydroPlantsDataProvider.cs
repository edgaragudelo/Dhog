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
    public class PeriodicHydroPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicHydroPlantsDataProvider));

        public PeriodicConventionalPlantsCollectionViewModel GetObjects()
        {
            PeriodicConventionalPlantsCollectionViewModel UIObjects = new PeriodicConventionalPlantsCollectionViewModel();

            List<PeriodicConventionalPlant> dataObjects = PeriodicHydroPlantsDataAccess.GetPeriodicHydroPlants();
            foreach (PeriodicConventionalPlant dataObject in dataObjects)
                UIObjects.Add(new PeriodicConventionalPlantsViewModel(dataObject));
            
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
                        PeriodicConventionalPlantsViewModel UIObject = item as PeriodicConventionalPlantsViewModel;
                        PeriodicHydroPlantsDataAccess.DeletePeriodicHydroPlant(UIObject.GetDataObject());
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
            PeriodicConventionalPlantsViewModel UIObject = sender as PeriodicConventionalPlantsViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicHydroPlantsDataAccess.UpdatePeriodicHydroPlant(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
