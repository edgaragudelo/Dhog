using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicInflowsDataGrid.xaml
    /// </summary>
    public partial class PeriodicInflowsDataGrid : BaseDataGridView
    {
        public PeriodicInflowsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
