using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicBlocksDataGrid.xaml
    /// </summary>
    public partial class RecursoRampaDataGrid : BaseDataGridView
    {
        public RecursoRampaDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
