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
    public class CompaniesDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CompaniesDataProvider));
        private CompaniesCollectionViewModel UIObjects;

        public CompaniesCollectionViewModel GetObjects()
        {
            UIObjects = new CompaniesCollectionViewModel();

            List<Company> dataObjects = CompaniesDataAccess.GetObjects();
            foreach (Company dataObject in dataObjects)
                UIObjects.Add(new CompanyViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new CompanyViewModel());

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
                        CompanyViewModel UIObject = item as CompanyViewModel;
                        CompaniesDataAccess.DeleteObject(UIObject.GetDataObject());
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex.StackTrace);
                }
            }
        }

        void ObjectsItemEndEdit(IEditableObject sender)
        {
            CompanyViewModel UIObject = sender as CompanyViewModel;

            try
            {
                if (UIObject.Name != null)
                {
                    int id = CompaniesDataAccess.UpdateObject(UIObject.GetDataObject());
                    if(id != -1)
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
