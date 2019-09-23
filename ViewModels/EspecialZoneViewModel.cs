using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace DHOG_WPF.ViewModels
{
    public class EspecialZonesCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class EspecialZoneViewModel: BaseViewModel
    {
        EspecialZone zone;
        ObservableCollection<string> plants;

        public EspecialZoneViewModel()
        {
            zone = new EspecialZone();
            IndiceIni = 1;
            IndiceFin = 1;
            Plants = new ObservableCollection<string>();
        }

        public EspecialZoneViewModel(EspecialZone zone)
        {
            this.zone = zone;
            //if (zone.Plants !=null)
            //SetPlants();
        }

        public EspecialZone GetDataObject()
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

        public double IndiceIni
        {
            get
            {
                return zone.IndiceIni;
            }
            set
            {
                zone.IndiceIni = value;
                RaisePropertyChanged("IndiceIni");
            }
        }

        public double IndiceFin
        {
            get
            {
                return zone.IndiceFin;
            }
            set
            {
                zone.IndiceFin = value;
                RaisePropertyChanged("IndiceFin");
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
