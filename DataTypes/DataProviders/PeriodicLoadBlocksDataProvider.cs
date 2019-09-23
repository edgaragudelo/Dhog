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
    public class PeriodicLoadBlocksDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodicLoadBlocksDataProvider));

        public PeriodicLoadBlocksCollectionViewModel GetObjects()
        {
            PeriodicLoadBlocksCollectionViewModel UIObjects = new PeriodicLoadBlocksCollectionViewModel();

            List<PeriodicLoadBlock> dataObjects = PeriodicLoadBlocksDataAccess.GetPeriodicLoadBlocks();
            foreach (PeriodicLoadBlock dataObject in dataObjects)
                UIObjects.Add(new PeriodicLoadBlockViewModel(dataObject));
            
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
                        PeriodicLoadBlockViewModel UIObject = item as PeriodicLoadBlockViewModel;
                        PeriodicLoadBlocksDataAccess.DeletePeriodicLoadBlock(UIObject.GetDataObject());
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
            PeriodicLoadBlockViewModel UIObject = sender as PeriodicLoadBlockViewModel;

            try
            {
                PeriodicLoadBlocksDataAccess.UpdatePeriodicLoadBlock(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
