using DHOG_WPF.ViewModels;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for PFEquationsDataGrid.xaml
    /// </summary>
    public partial class PFEquationsDataGrid : BaseDataGridView
    {
        public PFEquationsDataGrid(EntitiesCollections entitiesCollections): base(entitiesCollections)
        {
            InitializeComponent();
        }
    }
}
