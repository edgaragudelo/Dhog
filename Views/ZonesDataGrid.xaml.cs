using DHOG_WPF.ViewModels;
using System;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ZonesDataGrid.xaml
    /// </summary>
    public partial class ZonesDataGrid : BaseDataGridView
    {
        public ZonesDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            ZonesCollectionViewModel items = ItemsSource as ZonesCollectionViewModel;
            if (items.Count == 1)
            {
                ZoneViewModel item = items[0] as ZoneViewModel;

                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                    TypeColumn.IsReadOnly = false;

                }
            }
        }

        [STAThread]
        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
            TypeColumn.IsReadOnly = false;
        }
        [STAThread]
        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            NameColumn.IsReadOnly = true;
            TypeColumn.IsReadOnly = true;
        }
    }
}
