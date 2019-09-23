using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ReservoirsDataGrid.xaml
    /// </summary>
    public partial class ReservoirsDataGrid : BaseDataGridView
    {
        public ReservoirsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            ReservoirsCollectionViewModel items = ItemsSource as ReservoirsCollectionViewModel;
            if (items.Count == 1)
            {
                ReservoirViewModel item = items[0] as ReservoirViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = CaseColumn;
            CaseColumn.IsReadOnly = false;
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            CaseColumn.IsReadOnly = true;
        }
    }
}
