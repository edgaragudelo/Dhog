using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace DHOG_WPF.DataProviders
{
    public class VariableHydroPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VariableHydroPlantsDataProvider));
        private VariableHydroPlantsCollectionViewModel UIObjects;

        public VariableHydroPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new VariableHydroPlantsCollectionViewModel();

            List<VariableHydroPlant> dataObjects = VariableHydroPlantsDataAccess.GetObjects();
            foreach (VariableHydroPlant dataObject in dataObjects)
                UIObjects.Add(new VariableHydroPlantViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new VariableHydroPlantViewModel());

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
                        VariableHydroPlantViewModel UIObject = item as VariableHydroPlantViewModel;
                        VariableHydroPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            VariableHydroPlantViewModel UIObject = sender as VariableHydroPlantViewModel;

            try
            {
                if (UIObject.Name != null && UIObject.Reservoir != null)
                {
                    int id = VariableHydroPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                MessageBox.Show(MessageUtil.FormatMessage("ERROR.DuplicatedVariableHydroPlant", UIObject.Name, UIObject.Reservoir, UIObject.Segment, UIObject.Case), 
                                "DHOG", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                
            }
        }
    }
}
