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
    public class HydroSystemsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HydroSystemsDataProvider));
        private HydroSystemsCollectionViewModel UIObjects;

        public HydroSystemsCollectionViewModel GetObjects()
        {
            UIObjects = new HydroSystemsCollectionViewModel();

            List<HydroSystem> dataObjects = HydroSystemsDataAccess.GetObjects();
            foreach (HydroSystem dataObject in dataObjects)
                UIObjects.Add(new HydroSystemViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new HydroSystemViewModel());

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
                        HydroSystemViewModel UIObject = item as HydroSystemViewModel;
                        HydroSystemsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            HydroSystemViewModel UIObject = sender as HydroSystemViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = HydroSystemsDataAccess.UpdateObject(UIObject.GetDataObject());
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
