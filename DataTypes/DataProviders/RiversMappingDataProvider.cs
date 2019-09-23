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
    public class RiversMappingDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RiversMappingDataProvider));
        private NamesMappingCollectionViewModel UIObjects;

        public NamesMappingCollectionViewModel GetObjects()
        {
            UIObjects = new NamesMappingCollectionViewModel();

            List<NameMapping> dataObjects = RiversMappingDataAccess.GetObjects();
            foreach (NameMapping dataObject in dataObjects)
                UIObjects.Add(new NameMappingViewModel(dataObject));

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
            //            RiversMappingDataAccess.DeleteObject(UIObject.GetDataObject());
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
            //    if (UIObject.DHOGName != null)
            //        RiversMappingDataAccess.UpdateObject(UIObject.GetDataObject());
            //}
            //catch
            //{
            //    RadWindow.Alert(new DialogParameters
            //    {
            //        Content = MessageUtil.FormatMessage("ERROR.DuplicatedRiverNumber", UIObject.SDDPNumber)
            //    });
            //}
        }
    }
}
