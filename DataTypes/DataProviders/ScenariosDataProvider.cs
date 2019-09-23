using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DHOG_WPF.DataProviders
{
    public class ScenariosDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ScenariosDataProvider));
        private ScenariosActivosCollectionViewModel UIObjects;

        public ScenariosActivosCollectionViewModel GetObjects()
        {
            UIObjects = new ScenariosActivosCollectionViewModel();

            List<Scenario> dataObjects = ScenariosDataAccess.GetScenarios();
            foreach (Scenario dataObject in dataObjects)
                UIObjects.Add(new ScenarioViewModel(dataObject));
            
            UIObjects.ItemEndEdit += new ItemEndEditEventHandler(ObjectsItemEndEdit);
         


            return UIObjects;
        }

        public ScenariosActivosCollectionViewModel GetObjectsScenariosActivos()
        {
            UIObjects = new ScenariosActivosCollectionViewModel();

            List<ScenariosActivos> dataObjects = ScenariosDataAccess.GetScenariosActivos();
            foreach (ScenariosActivos dataObject in dataObjects)
                UIObjects.Add(new ScenariosActivosViewModel(dataObject));

            UIObjects.ItemEndEdit += new ItemEndEditEventHandler(ObjectsItemEndEdit);



            return UIObjects;
        }


        void ObjectsItemEndEdit(IEditableObject sender)
        {
            ScenarioViewModel UIObject = sender as ScenarioViewModel;

            try
            {
                if (UIObject.Variable != null)
                    ScenariosDataAccess.UpdateScenario(UIObject.GetDataObject());
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
