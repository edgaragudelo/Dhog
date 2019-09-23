using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicNonConventionalPlantsDataGrid.xaml
    /// </summary>
    public partial class PeriodicNonConventionalPlantsDataGrid : BaseDataGridView
    {
        public PeriodicNonConventionalPlantsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
