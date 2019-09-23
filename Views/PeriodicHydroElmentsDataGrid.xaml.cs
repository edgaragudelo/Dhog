using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicHydroElementsDataGrid.xaml
    /// </summary>
    public partial class PeriodicHydroElementsDataGrid : BaseDataGridView
    {
        public PeriodicHydroElementsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
