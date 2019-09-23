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
    public class ReservoirsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReservoirsDataProvider));
        private ReservoirsCollectionViewModel UIObjects;

        public ReservoirsCollectionViewModel GetObjects()
        {
            UIObjects = new ReservoirsCollectionViewModel();

            List<Reservoir> dataObjects = ReservoirsDataAccess.GetObjects();
            foreach (Reservoir dataObject in dataObjects)
                UIObjects.Add(new ReservoirViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new ReservoirViewModel());

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
                        ReservoirViewModel UIObject = item as ReservoirViewModel;
                        ReservoirsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            ReservoirViewModel UIObject = sender as ReservoirViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = ReservoirsDataAccess.UpdateObject(UIObject.GetDataObject());
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
