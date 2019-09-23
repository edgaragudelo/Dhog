using DHOG_WPF.ViewModels;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for VariableThermalPlantsDataGrid.xaml
    /// </summary>
    public partial class VariableThermalPlantsDataGrid : BaseDataGridView
    {
        public VariableThermalPlantsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            VariableConventionalPlantsCollectionViewModel items = ItemsSource as VariableConventionalPlantsCollectionViewModel;
            if (items.Count == 1)
            {
                VariableConventionalPlantViewModel item = items[0] as VariableConventionalPlantViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                }
            }

        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            SegmentColumn.IsReadOnly = false;
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            SegmentColumn.IsReadOnly = true;
        }
    }
}
