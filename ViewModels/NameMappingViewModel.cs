using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class NamesMappingCollectionViewModel: CollectionBaseViewModel
    {
        public void RemoveItem(string DHOGName)
        {
            for(int i = 0; i < Items.Count; i++)
            {
                NameMappingViewModel item = Items[i] as NameMappingViewModel;
                if (item.DHOGName.Equals(DHOGName))
                {
                    Remove(item);
                    break;
                }
            }
        }
    }

    public class NameMappingViewModel : BaseViewModel
    {
        NameMapping nameMapping;

        public NameMappingViewModel()
        {
            nameMapping = new NameMapping();
        }

        public NameMappingViewModel(NameMapping nameMapping)
        {
            this.nameMapping = nameMapping;
        }

        public NameMapping GetDataObject()
        {
            return nameMapping;
        }

        public string DHOGName
        {
            get
            {
                return nameMapping.DHOGName;
            }
            set
            {
                nameMapping.DHOGName = value;
                RaisePropertyChanged("DHOGName");
            }
        }

        public string SDDPName
        {
            get
            {
                return nameMapping.SDDPName;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    nameMapping.SDDPName = value;
                    RaisePropertyChanged("SDDPName");
                }
            }
        }

        public int SDDPNumber
        {
            get
            {
                return nameMapping.SDDPNumber;
            }
            set
            {
                nameMapping.SDDPNumber = value;
                RaisePropertyChanged("SDDPNumber");
            }
        }
    }
}
