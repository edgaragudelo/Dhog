using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for HydroSystemsDataGrid.xaml
    /// </summary>
    public partial class HydroSystemsDataGrid : BaseDataGridView
    {
        
        public HydroSystemsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            HydroSystemsCollectionViewModel items = ItemsSource as HydroSystemsCollectionViewModel;
            if (items.Count == 1)
            {
                HydroSystemViewModel item = items[0] as HydroSystemViewModel;
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
