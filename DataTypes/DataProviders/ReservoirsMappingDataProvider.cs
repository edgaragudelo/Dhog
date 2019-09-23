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
    public class ReservoirsMappingDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReservoirsMappingDataProvider));
        private NamesMappingCollectionViewModel UIObjects;

        public NamesMappingCollectionViewModel GetObjects(List<string> reservoirsNames)
        {
            UIObjects = new NamesMappingCollectionViewModel();

            List<NameMapping> dataObjects = ReservoirsMappingDataAccess.GetObjects();
            foreach (NameMapping dataObject in dataObjects)
            {
                UIObjects.Add(new NameMappingViewModel(dataObject));
                reservoirsNames.Remove(dataObject.DHOGName);
            }

            foreach(string reservoir in reservoirsNames)
                UIObjects.Add(new NameMappingViewModel(new NameMapping(reservoir, "")));

            UIObjects.ItemEndEdit += new ItemEndEditEventHandler(ObjectsItemEndEdit);
            UIObjects.CollectionChanged += new NotifyCollectionChangedEventHandler(ObjectsCollectionChanged);

            return UIObjects;
        }

        void ObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    try
            //    {
            //        foreach (object item in e.OldItems)
            //        {
            //            NameMappingViewModel UIObject = item as NameMappingViewModel;
            //            ReservoirsMappingDataAccess.DeleteObject(UIObject.GetDataObject());
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex.StackTrace);
            //    }
            //}
        }

        void ObjectsItemEndEdit(IEditableObject sender)
        {
            //NameMappingViewModel UIObject = sender as NameMappingViewModel;

            //try
            //{
            //    if (UIObject.DHOGName != null && UIObject.SDDPName != null)
            //        ReservoirsMappingDataAccess.UpdateObject(UIObject.GetDataObject());
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}
