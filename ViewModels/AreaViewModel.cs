using DHOG_WPF.Models;
using System;
using System.Collections.ObjectModel;

namespace DHOG_WPF.ViewModels
{
    public class AreasCollectionViewModel: CollectionBaseViewModel
    {
        
    }

    public class AreaViewModel : BaseViewModel
    {
        Area area;

        /* Constructor needed to add rows in the RadGridView control */
        public AreaViewModel()
        {
            area = new Area();
        }

        public AreaViewModel(Area area)
        {
            this.area = area;
        }

        public Area GetDataObject()
        {
            return area;
        }

        public int Id
        {
            get
            {
                return area.Id;
            }
            set
            {
                area.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return area.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    area.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double BaseLoad
        {
            get
            {
                return area.BaseLoad;
            }
            set
            {
                area.BaseLoad = value;
                RaisePropertyChanged("BaseLoad");
            }
        }

        public double ImportationLimit
        {
            get
            {
                return area.ImportationLimit;
            }
            set
            {
                area.ImportationLimit = value;
                RaisePropertyChanged("ImportationLimit");
            }
        }


        public double ExportationLimit
        {
            get
            {
                return area.ExportationLimit;
            }
            set
            {
                area.ExportationLimit = value;
                RaisePropertyChanged("ExportationLimit");
            }
        }
    }
}
