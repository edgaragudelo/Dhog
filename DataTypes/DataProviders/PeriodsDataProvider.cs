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
    public class PeriodsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodsDataProvider));
        private PeriodsCollectionViewModel UIObjects;

        public PeriodsCollectionViewModel GetObjects()
        {
            UIObjects = new PeriodsCollectionViewModel();

            List<Period> dataObjects = PeriodsDataAccess.GetObjects();
            foreach (Period dataObject in dataObjects)
                UIObjects.Add(new PeriodViewModel(dataObject));

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
                        PeriodViewModel UIObject = item as PeriodViewModel;
                        PeriodsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            PeriodViewModel UIObject = sender as PeriodViewModel;

            try
            {
                int id = PeriodsDataAccess.UpdateObject(UIObject.GetDataObject());
                if (id != -1)
                    UIObject.Id = id;
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
