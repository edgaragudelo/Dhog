using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Telerik.Windows.Controls;

namespace DHOG_WPF.DataProviders
{
    public class NonConventionalPlantBlocksDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NonConventionalPlantBlocksDataProvider));
        private NonConventionalPlantBlocksCollectionViewModel UIObjects;

        public NonConventionalPlantBlocksCollectionViewModel GetObjects()
        {
            UIObjects = new NonConventionalPlantBlocksCollectionViewModel();

            List<NonConventionalPlantBlock> dataObjects = NonConventionalPlantBlocksDataAccess.GetObjects();
            foreach (NonConventionalPlantBlock dataObject in dataObjects)
                UIObjects.Add(new NonConventionalPlantBlockViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new NonConventionalPlantBlockViewModel());

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
                        NonConventionalPlantBlockViewModel UIObject = item as NonConventionalPlantBlockViewModel;
                        NonConventionalPlantBlocksDataAccess.DeleteObject(UIObject.GetDataObject());
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
            NonConventionalPlantBlockViewModel UIObject = sender as NonConventionalPlantBlockViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = NonConventionalPlantBlocksDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch (Exception e)
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntryNameBlock", UIObject.Name, UIObject.Block)
                });
            }
        }
    }
}
