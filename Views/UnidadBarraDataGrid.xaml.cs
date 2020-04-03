using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicBlocksDataGrid.xaml
    /// </summary>
    public partial class UnidadBarraDataGrid : BaseDataGridView
    {
        public UnidadBarraDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            Barra.IsReadOnly = true;
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = Barra;
            Barra.IsReadOnly = false;
        }
    }
}
