using DHOG_WPF.ViewModels;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for HydroPlantsDataGrid.xaml
    /// </summary>
    public partial class HydroPlantsDataGrid : BaseDataGridView
    {
        public HydroPlantsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            HydroPlantsCollectionViewModel items = ItemsSource as HydroPlantsCollectionViewModel;
            if (items.Count == 1)
            {
                HydroPlantViewModel item = items[0] as HydroPlantViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                    CaseColumn.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CaseColumn.IsReadOnly = false;
            NameColumn.IsReadOnly = false;
            CurrentColumn = NameColumn;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            CaseColumn.IsReadOnly = true;
        }
    }
}
