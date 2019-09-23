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
    public class BlocksDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BlocksDataProvider));
        private BlocksCollectionViewModel UIObjects;

        public BlocksCollectionViewModel GetObjects()
        {
            UIObjects = new BlocksCollectionViewModel();

            List<Block> dataObjects = BlocksDataAccess.GetObjects();
            foreach (Block dataObject in dataObjects)
                UIObjects.Add(new BlockViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new BlockViewModel());

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
                        BlockViewModel UIObject = item as BlockViewModel;
                        BlocksDataAccess.DeleteObject(UIObject.GetDataObject());
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
            BlockViewModel UIObject = sender as BlockViewModel;

            try
            {
                int id = BlocksDataAccess.UpdateObject(UIObject.GetDataObject());
                if (id != -1)
                    UIObject.Id = id;
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntryName", UIObject.Name)
                });
            }
        }
    }
}
