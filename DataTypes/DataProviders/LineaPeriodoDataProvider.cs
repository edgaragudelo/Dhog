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
    public class LineaPeriodoDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LineaPeriodoDataProvider));

        public LineaPeriodoCollectionViewModel GetObjects()
        {
            LineaPeriodoCollectionViewModel UIObjects = new LineaPeriodoCollectionViewModel();

            //   List<LineaPeriodo> dataObjects = LineaPeriodoDataAccess.GetLineaPeriodo();
            List<LineaPeriodo> dataObjects = LineaPeriodoDataAccess.GetLineaPeriodo();
            foreach (LineaPeriodo dataObject in dataObjects)
                UIObjects.Add(new LineaPeriodoViewModel(dataObject));
                 

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
                        LineaPeriodoViewModel UIObject = item as LineaPeriodoViewModel;
                        LineaPeriodoDataAccess.DeleteLineaPeriodo(UIObject.GetDataObject());
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
            LineaPeriodoViewModel UIObject = sender as LineaPeriodoViewModel;

            try
            {
                LineaPeriodoDataAccess.UpdateLineaPeriodo(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
