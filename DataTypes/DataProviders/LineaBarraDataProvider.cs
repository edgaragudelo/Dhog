using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DHOG_WPF.DataProviders
{
    public class LineaBarraDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LineaBarraDataProvider));

        public LineaBarraCollectionViewModel GetObjects()
        {
            LineaBarraCollectionViewModel UIObjects = new LineaBarraCollectionViewModel();

            //   List<LineaBarra> dataObjects = LineaBarraDataAccess.GetLineaBarra();
            List<LineaBarra> dataObjects = LineaBarraDataAccess.GetObjects();
            foreach (LineaBarra dataObject in dataObjects)
                UIObjects.Add(new LineaBarraViewModel(dataObject));
                 

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
                        LineaBarraViewModel UIObject = item as LineaBarraViewModel;                        
                        LineaBarraDataAccess.DeleteObject(UIObject.GetDataObject());
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
            LineaBarraViewModel UIObject = sender as LineaBarraViewModel;

            try
            {
                LineaBarraDataAccess.UpdateObject(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
