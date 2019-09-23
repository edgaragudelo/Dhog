using DHOG_WPF.ViewModels;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for VariableHydroPlantsDataGrid.xaml
    /// </summary>
    public partial class VariableHydroPlantsDataGrid : BaseDataGridView
    {
        public VariableHydroPlantsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            VariableHydroPlantsCollectionViewModel items = ItemsSource as VariableHydroPlantsCollectionViewModel;
            if (items.Count == 1)
            {
                VariableHydroPlantViewModel item = items[0] as VariableHydroPlantViewModel;
                if (item.Name == null)
                {
                    CaseColumn.IsReadOnly = false;
                    SegmentColumn.IsReadOnly = false;
                    ReservoirColumn.IsReadOnly = false;
                    NameColumn.IsReadOnly = false;
                    CurrentColumn = NameColumn;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CaseColumn.IsReadOnly = false;
            SegmentColumn.IsReadOnly = false;
            ReservoirColumn.IsReadOnly = false;
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            CaseColumn.IsReadOnly = true;
            SegmentColumn.IsReadOnly = true;
            ReservoirColumn.IsReadOnly = true;
        }
    }
}
