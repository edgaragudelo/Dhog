using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicHydroSystemsDataGrid.xaml
    /// </summary>
    public partial class PeriodicHydroSystemsDataGrid : BaseDataGridView
    {
        public PeriodicHydroSystemsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
