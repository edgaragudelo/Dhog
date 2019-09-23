using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for AreasDataGrid.xaml
    /// </summary>
    public partial class AreasDataGrid : BaseDataGridView
    {
        public AreasDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            AreasCollectionViewModel items = ItemsSource as AreasCollectionViewModel;
            if (items.Count == 1)
            {
                AreaViewModel item = items[0] as AreaViewModel;
                if(item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
        }
    }
}
