using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicAreasDataGrid.xaml
    /// </summary>
    public partial class PeriodicAreasDataGrid : BaseDataGridView
    {
        public PeriodicAreasDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
