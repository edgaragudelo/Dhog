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
    public class HydroTopologyDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HydroTopologyDataProvider));
        private HydroTopologyCollectionViewModel UIObjects;

        public HydroTopologyCollectionViewModel GetSystemTopology(string system)
        {
            UIObjects = new HydroTopologyCollectionViewModel();

            List<HydroTopology> dataObjects = HydroTopologyDataAccess.GetSystemTopology(system);
            foreach (HydroTopology dataObject in dataObjects)
                UIObjects.Add(new HydroTopologyViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new HydroTopologyViewModel());

            UIObjects.CollectionChanged += new NotifyCollectionChangedEventHandler(ObjectsCollectionChanged);
            UIObjects.ItemEndEdit += new ItemEndEditEventHandler(ObjectsItemEndEdit);
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
                        HydroTopologyViewModel UIObject = item as HydroTopologyViewModel;
                        HydroTopologyDataAccess.DeleteElement(UIObject.GetDataObject());
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
            HydroTopologyViewModel UIObject = sender as HydroTopologyViewModel;

            try
            {
                if (UIObject.System != null && UIObject.Element != null && UIObject.Type != null && UIObject.ElementType != null)
                {
                    int id = HydroTopologyDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntryName", UIObject.System)
                });
            }
        }
    }
}
