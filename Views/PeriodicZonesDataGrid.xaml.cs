using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicZonesDataGrid.xaml
    /// </summary>
    public partial class PeriodicZonesDataGrid : BaseDataGridView
    {
        public PeriodicZonesDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
