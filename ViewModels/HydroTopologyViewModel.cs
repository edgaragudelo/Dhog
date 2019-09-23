using DHOG_WPF.Models;
using DHOG_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DHOG_WPF.ViewModels
{
    public class HydroTopologyCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class HydroTopologyViewModel : BaseViewModel
    {
        HydroTopology hydroTopology;
        List<string> elementsList;
        List<string> typesList;

        public HydroTopologyViewModel()
        {
            hydroTopology = new HydroTopology();
            hydroTopology.System = HydroTopologyPanel.HydroSystemName;
            elementsList = new List<string>();
            typesList = new List<string>();
        }

        public HydroTopologyViewModel(HydroTopology hydroTopology)
        {
            this.hydroTopology = hydroTopology;
            elementsList = new List<string>();
            SetElementsList();
            SetTypesList();
        }

        public HydroTopology GetDataObject()
        {
            return hydroTopology;
        }

        public int Id
        {
            get
            {
                return hydroTopology.Id;
            }
            set
            {
                hydroTopology.Id = value;
            }
        }

        public string System
        {
            get
            {
                return hydroTopology.System;
            }
            set
            {
                hydroTopology.System = value;
                RaisePropertyChanged("System");
            }
        }

        public string Element
        {
            get
            {
                return hydroTopology.Element;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    hydroTopology.Element = value;
                    RaisePropertyChanged("Element");
                }
            }
        }

        public string Type
        {
            get
            {
                return hydroTopology.Type;
            }
            set
            {
                hydroTopology.Type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string ElementType
        {
            get
            {
                return hydroTopology.ElementType;
            }
            set
            {
                hydroTopology.ElementType = value;
                SetElementsList();
                SetTypesList();
                RaisePropertyChanged("ElementType");
            }
        }

        public void SetElementsList()
        {
            switch (hydroTopology.ElementType)
            {
                case "Embalse":
                    elementsList = EntitiesCollections.GetReservoirsScenario1();
                    break;
                case "RecursoHidro":
                    elementsList = EntitiesCollections.GetHydroPlantsScenario1();
                    break;
                case "ElementoHidro":
                    elementsList = EntitiesCollections.GetHydroElementsScenario1();
                    break;
                case "Rio":
                    elementsList = EntitiesCollections.GetRiversScenario1();
                    break;
            }
            RaisePropertyChanged("ElementsList");
        }

        public void SetTypesList()
        {
            //switch (hydroTopology.ElementType)
            //{
            //    case "Embalse":
            //        typesList = new List<string>()
            //        {
            //            "E",
            //            "V",
            //            "AV", 
            //            "A", 
            //            "AA"
            //        };
            //        break;
            //    case "RecursoHidro":
            //        typesList = new List<string>()
            //        {
            //            "TG",
            //            "AT"
            //        };
            //        break;
            //    case "ElementoHidro":
            //        typesList = new List<string>()
            //        {
            //            "T",
            //            "AT"
            //        };
            //        break;
            //    case "Rio":
            //        typesList = new List<string>()
            //        {
            //            "R"
            //        };
            //        break;
            //}
            typesList = new List<string>()
                    {
                        "E",
                        "V",
                        "AV",
                        "A",
                        "AA",
                        "AT",
                        "R",
                        "T",
                        "TG"

                    };
            RaisePropertyChanged("TypesList");
        }

        public List<string> ElementsList
        {
            get
            {
                return elementsList;
            }
            set
            {
                elementsList = value;
                RaisePropertyChanged("ElementsList");
            }
        }

        public List<string> TypesList
        {
            get
            {
                return typesList;
            }
            set
            {
                typesList = value;
                RaisePropertyChanged("TypesList");
            }
        }
    }
}
