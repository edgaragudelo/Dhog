using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for FuelDataGrid.xaml
    /// </summary>
    public partial class FuelsDataGrid : BaseDataGridView
    {
        public FuelsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            FuelsCollectionViewModel items = ItemsSource as FuelsCollectionViewModel;
            if (items.Count == 1)
            {
                FuelViewModel item = items[0] as FuelViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
        }
    }
}
