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
    public class EspecialZonesDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EspecialZonesDataProvider));
        private EspecialZonesCollectionViewModel UIObjects;

        public EspecialZonesCollectionViewModel GetObjects()
        {
            UIObjects = new EspecialZonesCollectionViewModel();

            List<EspecialZone> dataObjects = EspecialZonesDataAccess.GetZones();
            foreach (EspecialZone dataObject in dataObjects)
               UIObjects.Add(new EspecialZoneViewModel(dataObject));
            
            if (UIObjects.Count == 0)
                UIObjects.Add(new EspecialZoneViewModel());

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
                        EspecialZoneViewModel UIObject = item as EspecialZoneViewModel;
                        EspecialZonesDataAccess.DeleteZone(UIObject.GetDataObject());
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
            EspecialZoneViewModel UIObject = sender as EspecialZoneViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = EspecialZonesDataAccess.UpdateZone(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch (Exception ex)
            {


                log.Error(ex.StackTrace);
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedZone", UIObject.Name, UIObject.Name)
                });
            }
        }
    }
}
