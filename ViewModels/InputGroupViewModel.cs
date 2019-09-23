using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace DHOG_WPF.ViewModels
{
    public class InputGroupViewModel : ViewModelBase
    {
        public InputGroupViewModel(string name, ObservableCollection<InputEntityViewModel> entities, Uri imageUri)
        {
            this.name = name;
            this.entities = entities;
            ImageUri = imageUri;
        }

        private string name;
        private bool isChecked;
        private ObservableCollection<InputEntityViewModel> entities;

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

        public ObservableCollection<InputEntityViewModel> Entities
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
                foreach(InputEntityViewModel entity in Entities)
                {
                    entity.IsChecked = value;
                }
                RaisePropertyChanged("IsChecked");
            }
        }

        public Uri ImageUri { get; set; }
    }
}
