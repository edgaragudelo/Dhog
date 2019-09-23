using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicFuelsDataGrid.xaml
    /// </summary>
    public partial class PeriodicFuelsDataGrid : BaseDataGridView
    {
        public PeriodicFuelsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
