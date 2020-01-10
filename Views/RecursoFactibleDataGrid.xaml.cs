using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicBlocksDataGrid.xaml
    /// </summary>
    public partial class RecursoFactibleDataGrid : BaseDataGridView
    {
        public RecursoFactibleDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
