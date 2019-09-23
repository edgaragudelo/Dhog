using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace DHOG_WPF.ViewModels
{
    public class OutputGroupViewModel: ViewModelBase
    {
        public OutputGroupViewModel(string name, ObservableCollection<OutputEntityViewModel> entities, Uri imageUri)
        {
            this.name = name;
            this.entities = entities;
            ImageUri = imageUri;
        }

        private string name;
        private bool isChecked;
        private ObservableCollection<OutputEntityViewModel> entities;

        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                name = value;
            }
        }

        public ObservableCollection<OutputEntityViewModel> Entities
        {
            get
            {
                return entities;
            }
            private set
            {
                entities = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                foreach (OutputEntityViewModel entity in Entities)
                {
                    entity.IsChecked = value;
                }
                RaisePropertyChanged("IsChecked");
            }
        }

        public Uri ImageUri { get; set; }
    }
}
