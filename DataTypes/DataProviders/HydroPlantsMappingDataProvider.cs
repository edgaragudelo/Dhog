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
    public class HydroPlantsMappingDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HydroPlantsMappingDataProvider));
        private NamesMappingCollectionViewModel UIObjects;

        public NamesMappingCollectionViewModel GetObjects(List<string> HydroPlantsNames)
        {
            UIObjects = new NamesMappingCollectionViewModel();

            List<NameMapping> dataObjects = HydroPlantsMappingDataAccess.GetObjects();
            foreach (NameMapping dataObject in dataObjects)
            {
                UIObjects.Add(new NameMappingViewModel(dataObject));
                HydroPlantsNames.Remove(dataObject.DHOGName);
            }

            foreach(string hydroPlant in HydroPlantsNames)
                UIObjects.Add(new NameMappingViewModel(new NameMapping(hydroPlant, "")));

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
            //            HydroPlantsMappingDataAccess.DeleteObject(UIObject.GetDataObject());
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
            //        HydroPlantsMappingDataAccess.UpdateObject(UIObject.GetDataObject());
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}
