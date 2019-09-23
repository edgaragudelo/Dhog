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
    public class VariableThermalPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VariableThermalPlantsDataProvider));
        private VariableConventionalPlantsCollectionViewModel UIObjects;

        public VariableConventionalPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new VariableConventionalPlantsCollectionViewModel();

            List<VariableConventionalPlant> dataObjects = VariableThermalPlantsDataAccess.GetObjects();
            foreach (VariableConventionalPlant dataObject in dataObjects)
                UIObjects.Add(new VariableConventionalPlantViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new VariableConventionalPlantViewModel());

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
                        VariableConventionalPlantViewModel UIObject = item as VariableConventionalPlantViewModel;
                        VariableThermalPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            VariableConventionalPlantViewModel UIObject = sender as VariableConventionalPlantViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = VariableThermalPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                MessageBox.Show(MessageUtil.FormatMessage("ERROR.DuplicatedVariableThermalPlant", UIObject.Name, UIObject.Segment),
                                "DHOG", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
