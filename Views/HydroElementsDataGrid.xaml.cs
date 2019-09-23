using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for HydroElementsDataGrid.xaml
    /// </summary>
    public partial class HydroElementsDataGrid : BaseDataGridView
    {
        
        public HydroElementsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            HydroElementsCollectionViewModel items = ItemsSource as HydroElementsCollectionViewModel;
            if (items.Count == 1)
            {
                HydroElementViewModel item = items[0] as HydroElementViewModel;
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
