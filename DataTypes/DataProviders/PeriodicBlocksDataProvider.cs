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
    public class PeriodicBlocksDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicBlocksDataProvider));

        public PeriodicBlocksCollectionViewModel GetObjects()
        {
            PeriodicBlocksCollectionViewModel UIObjects = new PeriodicBlocksCollectionViewModel();

            List<PeriodicBlock> dataObjects = PeriodicBlocksDataAccess.GetPeriodicBlocks();
            foreach (PeriodicBlock dataObject in dataObjects)
                UIObjects.Add(new PeriodicBlockViewModel(dataObject));
            
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
                        PeriodicBlockViewModel UIObject = item as PeriodicBlockViewModel;
                        PeriodicBlocksDataAccess.DeletePeriodicBlock(UIObject.GetDataObject());
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
            PeriodicBlockViewModel UIObject = sender as PeriodicBlockViewModel;

            try
            {
                PeriodicBlocksDataAccess.UpdatePeriodicBlock(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
