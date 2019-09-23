using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for NonConventionalPlantBlocksDataGrid.xaml
    /// </summary>
    public partial class NonConventionalPlantBlocksDataGrid : BaseDataGridView
    {
        public NonConventionalPlantBlocksDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            NonConventionalPlantBlocksCollectionViewModel items = ItemsSource as NonConventionalPlantBlocksCollectionViewModel;
            if (items.Count == 1)
            {
                NonConventionalPlantBlockViewModel item = items[0] as NonConventionalPlantBlockViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                    BlockColumn.IsReadOnly = false;
                    ReductionFactor.IsReadOnly = false;
                    Case.IsReadOnly = false;
                    Id.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
            BlockColumn.IsReadOnly = false;
            ReductionFactor.IsReadOnly = false;
            Case.IsReadOnly = false;
            Id.IsReadOnly = false;

        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            //NameColumn.IsReadOnly = true;
            //BlockColumn.IsReadOnly = true;
            //ReductionFactor.IsReadOnly = true;
            //Case.IsReadOnly = true;
            //Id.IsReadOnly = true;

            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
            BlockColumn.IsReadOnly = false;
            ReductionFactor.IsReadOnly = false;
            Case.IsReadOnly = false;
            Id.IsReadOnly = false;
        }
    }
}
