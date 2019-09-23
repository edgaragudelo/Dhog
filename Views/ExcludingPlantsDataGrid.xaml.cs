using DHOG_WPF.ViewModels;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ExcludingPlantsDataGrid.xaml
    /// </summary>
    public partial class ExcludingPlantsDataGrid : BaseDataGridView
    {
        public ExcludingPlantsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
