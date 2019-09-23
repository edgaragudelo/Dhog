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
    public class ThermalPlantsMappingDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThermalPlantsMappingDataProvider));
        private NamesMappingCollectionViewModel UIObjects;

        public NamesMappingCollectionViewModel GetObjects(List<string> thermalPlantsNames)
        {
            UIObjects = new NamesMappingCollectionViewModel();

            List<NameMapping> dataObjects = ThermalPlantsMappingDataAccess.GetObjects();
            foreach (NameMapping dataObject in dataObjects)
            {
                UIObjects.Add(new NameMappingViewModel(dataObject));
                thermalPlantsNames.Remove(dataObject.DHOGName);
            }

            foreach(string thermalPlant in thermalPlantsNames)
                UIObjects.Add(new NameMappingViewModel(new NameMapping(thermalPlant, "")));

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
            //            ThermalPlantsMappingDataAccess.DeleteObject(UIObject.GetDataObject());
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
            //        ThermalPlantsMappingDataAccess.UpdateObject(UIObject.GetDataObject());
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}
