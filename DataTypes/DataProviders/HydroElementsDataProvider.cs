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
    public class HydroElementsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HydroElementsDataProvider));
        private HydroElementsCollectionViewModel UIObjects;

        public HydroElementsCollectionViewModel GetObjects()
        {
            UIObjects = new HydroElementsCollectionViewModel();

            List<HydroElement> dataObjects = HydroElementsDataAccess.GetObjects();
            foreach (HydroElement dataObject in dataObjects)
                UIObjects.Add(new HydroElementViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new HydroElementViewModel());

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
                        HydroElementViewModel UIObject = item as HydroElementViewModel;
                        HydroElementsDataAccess.DeleteObject(UIObject.GetDataObject());
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex.StackTrace);
                }
            }
        }

        void ObjectsItemEndEdit(IEditableObject sender)
        {
            HydroElementViewModel UIObject = sender as HydroElementViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = HydroElementsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if(id != -1)
                        UIObject.Id = id;
                }
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
