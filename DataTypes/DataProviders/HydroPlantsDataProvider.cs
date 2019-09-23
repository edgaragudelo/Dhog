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
    public class HydroPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HydroPlantsDataProvider));
        private HydroPlantsCollectionViewModel UIObjects;

        public HydroPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new HydroPlantsCollectionViewModel();

            List<HydroPlant> dataObjects = HydroPlantsDataAccess.GetObjects();
            foreach (HydroPlant dataObject in dataObjects)
                UIObjects.Add(new HydroPlantViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new HydroPlantViewModel());

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
                        HydroPlantViewModel UIObject = item as HydroPlantViewModel;
                        HydroPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            HydroPlantViewModel UIObject = sender as HydroPlantViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = HydroPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntry", UIObject.Name, UIObject.Case)
                });
            }
        }
    }
}
