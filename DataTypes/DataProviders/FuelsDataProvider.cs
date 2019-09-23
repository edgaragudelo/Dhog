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
    public class FuelsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FuelsDataProvider));
        private FuelsCollectionViewModel UIObjects;

        public FuelsCollectionViewModel GetObjects()
        {
            UIObjects = new FuelsCollectionViewModel();

            List<Fuel> dataObjects = FuelsDataAccess.GetObjects();
            foreach (Fuel dataObject in dataObjects)
                UIObjects.Add(new FuelViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new FuelViewModel());

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
                        FuelViewModel UIObject = item as FuelViewModel;
                        FuelsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            FuelViewModel UIObject = sender as FuelViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = FuelsDataAccess.UpdateObject(UIObject.GetDataObject());
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
