using DHOG_WPF.ViewModels;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PeriodicCompaniesDataGrid.xaml
    /// </summary>
    public partial class PeriodicCompaniesDataGrid : BaseDataGridView
    {
        public PeriodicCompaniesDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
