using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for NonConventionalPlantDataGrid.xaml
    /// </summary>
    public partial class NonConventionalPlantsDataGrid : BaseDataGridView
    {
        public NonConventionalPlantsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            NonConventionalPlantsCollectionViewModel items = ItemsSource as NonConventionalPlantsCollectionViewModel;
            if (items.Count == 1)
            {
                NonConventionalPlantViewModel item = items[0] as NonConventionalPlantViewModel;
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
            NameColumn.IsReadOnly = false;
            CaseColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            CaseColumn.IsReadOnly = true;
        }
    }
}
