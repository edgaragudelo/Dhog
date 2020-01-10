using DHOG_WPF.ViewModels;

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
    }
}
