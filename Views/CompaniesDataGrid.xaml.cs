using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for CompaniesDataGrid.xaml
    /// </summary>
    public partial class CompaniesDataGrid : BaseDataGridView
    {
        
        public CompaniesDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();

            CompaniesCollectionViewModel items = ItemsSource as CompaniesCollectionViewModel;
            if (items.Count == 1)
            {
                CompanyViewModel item = items[0] as CompanyViewModel;
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
            CurrentColumn = NameColumn;
            CaseColumn.IsReadOnly = false;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            CaseColumn.IsReadOnly = true;
        }
    }
}
