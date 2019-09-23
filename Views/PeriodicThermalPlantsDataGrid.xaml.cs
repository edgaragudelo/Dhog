using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicThermalPlantsDataGrid.xaml
    /// </summary>
    public partial class PeriodicThermalPlantsDataGrid : BaseDataGridView
    {
        public PeriodicThermalPlantsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
