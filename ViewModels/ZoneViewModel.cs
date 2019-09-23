using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace DHOG_WPF.ViewModels
{
    public class ZonesCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class ZoneViewModel: BaseViewModel
    {
        Zone zone;
        ObservableCollection<string> plants;

        public ZoneViewModel()
        {
            zone = new Zone();
            Type = "MN";
            Plants = new ObservableCollection<string>();
        }

        public ZoneViewModel(Zone zone)
        {
            this.zone = zone;
            SetPlants();
        }

        public Zone GetDataObject()
        {
            return zone;
        }

        public int Id
        {
            get
            {
                return zone.Id;
            }
            set
            {
                zone.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return zone.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    zone.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double Value
        {
            get
            {
                return zone.Value;
            }
            set
            {
                zone.Value = value;
                RaisePropertyChanged("Value");
            }
        }

        public string Type
        {
            get
            {
                return zone.Type;
            }
            set
            {
                if (value == null)
                    zone.Type = "";
                else
                    zone.Type = value;

                RaisePropertyChanged("Type");
            }
        }

        private void SetPlants()
        {
            Plants = new ObservableCollection<string>();
            foreach (string plant in zone.Plants)
                Plants.Add(plant);
        }

        public ObservableCollection<string> Plants 
        {
            get
            {
                return plants;
            }
            set
            {
                plants = value;                
                plants.CollectionChanged += Plants_CollectionChanged;
                RaisePropertyChanged("Plants");
            }
        }

        private void Plants_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is string newPlant)
                        ZonesDataAccess.AddPlantToZone(Name, newPlant);
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is string removedPlant)
                        ZonesDataAccess.DeletePlantFromZone(Name, removedPlant);
                }
            }
        }
    }
}
