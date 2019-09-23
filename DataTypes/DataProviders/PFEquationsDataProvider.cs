using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace DHOG_WPF.DataProviders
{
    public class PFEquationsDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PFEquationsDataProvider));
        private PFEquationsCollectionViewModel UIObjects;

        public PFEquationsCollectionViewModel GetObjects()
        {
            UIObjects = new PFEquationsCollectionViewModel();

            List<PFEquation> dataObjects = PFEquationsDataAccess.GetObjects();
            foreach (PFEquation dataObject in dataObjects)
                UIObjects.Add(new PFEquationViewModel(dataObject));

            if (UIObjects.Count == 0)
                UIObjects.Add(new PFEquationViewModel());

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
                        PFEquationViewModel UIObject = item as PFEquationViewModel;
                        PFEquationsDataAccess.DeleteObject(UIObject.GetDataObject());
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
            PFEquationViewModel UIObject = sender as PFEquationViewModel;

            try
            {
                if (UIObject.Name != null && UIObject.Reservoir != null)
                {
                    int id = PFEquationsDataAccess.UpdateObject(UIObject.GetDataObject());
                    if (id != -1)
                        UIObject.Id = id;
                }
            }
            catch(Exception e)
            {
                UIObjects.Remove(UIObject);
                log.Fatal(e.Message);
                MessageBox.Show(MessageUtil.FormatMessage("ERROR.DuplicatedPFEquation", UIObject.Name, UIObject.Reservoir, UIObject.Case), 
                                "DHOG", MessageBoxButton.OK, MessageBoxImage.Exclamation);                
            }
        }
    }
}
