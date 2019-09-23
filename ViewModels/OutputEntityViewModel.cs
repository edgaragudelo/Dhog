using DHOG_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class OutputEntityViewModel: ViewModelBase
    {
        public OutputEntityViewModel(string name, ChartPanelView chartPanel, InfoType type)
        {
            Name = name;
            ChartPanel = chartPanel;
            Type = type;
        }

        public OutputEntityViewModel(string name, string shortName, RadGridView grid, InfoType type)
        {
            Name = name;
            ShortName = shortName;
            Grid = grid;
            Type = type;
        }

        private bool isChecked;
        private bool isSelected;
        public ChartPanelView ChartPanel { get; }
        public RadGridView Grid { get; }
        public String Name { get; }
        public String ShortName { get; }
        public InfoType Type { get; }

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
    }
}
