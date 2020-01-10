using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicBlocksDataGrid.xaml
    /// </summary>
    public partial class ZonaUnidadDataGrid : BaseDataGridView
    {
        public ZonaUnidadDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
