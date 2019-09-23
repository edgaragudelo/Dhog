using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicHydroPlantsDataGrid.xaml
    /// </summary>
    public partial class PeriodicHydroPlantsDataGrid : BaseDataGridView
    {
        public PeriodicHydroPlantsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
