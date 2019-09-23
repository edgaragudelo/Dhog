using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicReservoirsDataGrid.xaml
    /// </summary>
    public partial class PeriodicReservoirsDataGrid : BaseDataGridView
    {
        public PeriodicReservoirsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
