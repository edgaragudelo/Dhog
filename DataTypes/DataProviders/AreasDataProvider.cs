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
    public class AreasDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AreasDataProvider));
        private AreasCollectionViewModel UIObjects;

        public AreasCollectionViewModel GetObjects()
        {
            UIObjects = new AreasCollectionViewModel();

            List<Area> dataObjects = AreasDataAccess.GetObjects();
            foreach (Area dataObject in dataObjects)
                UIObjects.Add(new AreaViewModel(dataObject));
            
            if(UIObjects.Count == 0)
                UIObjects.Add(new AreaViewModel());
                
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
                        AreaViewModel UIObject = item as AreaViewModel;
                        AreasDataAccess.DeleteObject(UIObject.GetDataObject());
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
            AreaViewModel UIObject = sender as AreaViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = AreasDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntryName", UIObject.Name)
                });
            }
        }
    }
}
