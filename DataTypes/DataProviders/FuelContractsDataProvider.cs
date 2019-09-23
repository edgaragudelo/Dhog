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
    public class FuelContractsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FuelContractsDataProvider));
        private FuelContractsCollectionViewModel UIObjects;

        public FuelContractsCollectionViewModel GetObjects()
        {
            UIObjects = new FuelContractsCollectionViewModel();

            List<FuelContract> dataObjects = FuelContractsDataAccess.GetObjects();
            foreach (FuelContract dataObject in dataObjects)
                UIObjects.Add(new FuelContractViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new FuelContractViewModel());

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
                        FuelContractViewModel UIObject = item as FuelContractViewModel;
                        FuelContractsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            FuelContractViewModel UIObject = sender as FuelContractViewModel;

            try
            {
                if (UIObject.Name != null)
                { 
                    int id = FuelContractsDataAccess.UpdateObject(UIObject.GetDataObject());
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
