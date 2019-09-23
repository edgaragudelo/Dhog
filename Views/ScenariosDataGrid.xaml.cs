using DHOG_WPF.DataAccess;
using DHOG_WPF.ViewModels;
using System;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ScenariosDataGrid.xaml
    /// </summary>
    public partial class ScenariosDataGrid : BaseDataGridView
    {
        public ScenariosDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();
        }

       
    }
}
