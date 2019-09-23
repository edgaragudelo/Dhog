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
    public class ThermalPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThermalPlantsDataProvider));
        private ThermalPlantsCollectionViewModel UIObjects;

        public ThermalPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new ThermalPlantsCollectionViewModel();

            List<ThermalPlant> dataObjects = ThermalPlantsDataAccess.GetObjects();
            foreach (ThermalPlant dataObject in dataObjects)
                UIObjects.Add(new ThermalPlantViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new ThermalPlantViewModel());

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
                        ThermalPlantViewModel UIObject = item as ThermalPlantViewModel;
                        ThermalPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            ThermalPlantViewModel UIObject = sender as ThermalPlantViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = ThermalPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
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
