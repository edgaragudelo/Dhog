using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for BlocksDataGrid.xaml
    /// </summary>
    public partial class BlocksDataGrid : BaseDataGridView
    {
        public BlocksDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            BlocksCollectionViewModel items = ItemsSource as BlocksCollectionViewModel;
            if (items.Count == 1)
            {
                BlockViewModel item = items[0] as BlockViewModel;
                if (item.Name == 0)
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
