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
    public class RecursoFuelContractsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecursoFuelContractsDataProvider));
        private RecursoFuelContractsCollectionViewModel UIObjects;

        public RecursoFuelContractsCollectionViewModel GetObjects()
        {
            UIObjects = new RecursoFuelContractsCollectionViewModel();

            List<RecursoFuelContract> dataObjects = RecursoFuelContractsDataAccess.GetObjects();
            foreach (RecursoFuelContract dataObject in dataObjects)
                UIObjects.Add(new RecursoFuelContractViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new RecursoFuelContractViewModel());

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
                        RecursoFuelContractViewModel UIObject = item as RecursoFuelContractViewModel;
                        RecursoFuelContractsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            RecursoFuelContractViewModel UIObject = sender as RecursoFuelContractViewModel;

            try
            {
                if (UIObject.Name1 != null)
                { 
                    int id = RecursoFuelContractsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id; 
                }
            }
            catch
            {
                UIObjects.Remove(UIObject);
                RadWindow.Alert(new DialogParameters
                {
                    Content = MessageUtil.FormatMessage("ERROR.DuplicatedEntryName", UIObject.Name1)
                });
            }
        }
    }
}
