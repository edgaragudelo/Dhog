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
    public class PeriodicInflowsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicInflowsDataProvider));

        public PeriodicInflowsCollectionViewModel GetObjects()
        {
            PeriodicInflowsCollectionViewModel UIObjects = new PeriodicInflowsCollectionViewModel();

            List<PeriodicInflow> dataObjects = PeriodicInflowsDataAccess.GetPeriodicInflows();
            foreach (PeriodicInflow dataObject in dataObjects)
                UIObjects.Add(new PeriodicInflowViewModel(dataObject));
            
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
                        PeriodicInflowViewModel UIObject = item as PeriodicInflowViewModel;
                        PeriodicInflowsDataAccess.DeletePeriodicInflow(UIObject.GetDataObject());
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
            PeriodicInflowViewModel UIObject = sender as PeriodicInflowViewModel;

            try
            {
                if (UIObject.Name != null )
                    PeriodicInflowsDataAccess.UpdatePeriodicInflow(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
