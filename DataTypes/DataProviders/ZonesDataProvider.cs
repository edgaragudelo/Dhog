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
    public class ZonesDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ZonesDataProvider));
        private ZonesCollectionViewModel UIObjects;

        public ZonesCollectionViewModel GetObjects()
        {
            UIObjects = new ZonesCollectionViewModel();

            List<Zone> dataObjects = ZonesDataAccess.GetZones();
            foreach (Zone dataObject in dataObjects)
                UIObjects.Add(new ZoneViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new ZoneViewModel());

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
                        ZoneViewModel UIObject = item as ZoneViewModel;
                        ZonesDataAccess.DeleteZone(UIObject.GetDataObject());
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
            ZoneViewModel UIObject = sender as ZoneViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = ZonesDataAccess.UpdateZone(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedZone", UIObject.Name, UIObject.Type)
                });
            }
        }
    }
}
