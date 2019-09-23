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
    public class NonConventionalPlantsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NonConventionalPlantsDataProvider));
        private NonConventionalPlantsCollectionViewModel UIObjects;

        public NonConventionalPlantsCollectionViewModel GetObjects()
        {
            UIObjects = new NonConventionalPlantsCollectionViewModel();

            List<NonConventionalPlant> dataObjects = NonConventionalPlantsDataAccess.GetObjects();
            foreach (NonConventionalPlant dataObject in dataObjects)
                UIObjects.Add(new NonConventionalPlantViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new NonConventionalPlantViewModel());

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
                        NonConventionalPlantViewModel UIObject = item as NonConventionalPlantViewModel;
                        NonConventionalPlantsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            NonConventionalPlantViewModel UIObject = sender as NonConventionalPlantViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = NonConventionalPlantsDataAccess.UpdateObject(UIObject.GetDataObject());
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
