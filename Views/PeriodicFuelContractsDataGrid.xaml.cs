using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicFuelContractsDataGrid.xaml
    /// </summary>
    public partial class PeriodicFuelContractsDataGrid : BaseDataGridView
    {
        public PeriodicFuelContractsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
