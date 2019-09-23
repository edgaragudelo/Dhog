using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class ExcludingPlantsCollectionViewModel: CollectionBaseViewModel
    {
    }

    public class ExcludingPlantsViewModel : BaseViewModel
    {
        ExcludingPlants excludingPlants;

        public ExcludingPlantsViewModel()
        {
            excludingPlants = new ExcludingPlants();
        }

        public ExcludingPlantsViewModel(ExcludingPlants excludingPlants)
        {
            this.excludingPlants = excludingPlants;
        }

        public ExcludingPlants GetDataObject()
        {
            return excludingPlants;
        }

        public string Plant1
        {
            get
            {
                return excludingPlants.Plant1;
            }
            set
            {
                excludingPlants.Plant1 = value;
                RaisePropertyChanged("Plant1");
            }
        }

        public string Plant2
        {
            get
            {
                return excludingPlants.Plant2;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    excludingPlants.Plant2 = value;
                    RaisePropertyChanged("Plant2");
                }
            }
        }
    }
}
