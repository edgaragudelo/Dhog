using DHOG_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class InputEntityViewModel: ViewModelBase
    {
        public InputEntityViewModel(string name, FrameworkElement dataGridView, EntityType entityType)
        //public InputEntityViewModel(string name, RadGridView dataGridView, EntityType entityType)
        {
            Name = name;
            DataGridView = dataGridView;
            
            EntityType = entityType;
        }

        private bool isChecked;
        private bool isSelected;
        private bool wasImported;
        public String Name { get; }
        public FrameworkElement DataGridView { get; }
        //public RadGridView DataGridView { get; }

        public EntityType EntityType { get; }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public bool WasImported
        {
            get
            {
                return wasImported;
            }
            set
            {
                wasImported = value;
                OnPropertyChanged("WasImported");
            }
        }

        public string ShortName { get; internal set; }
    }
}
