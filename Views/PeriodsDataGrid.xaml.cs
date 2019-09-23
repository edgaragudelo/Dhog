using DHOG_WPF.ViewModels;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for BasicPeriodsDataGrid.xaml
    /// </summary>
    public partial class PeriodsDataGrid : BaseDataGridView
    {
        public PeriodsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {

          //  ObservableCollection

            InitializeComponent();

            //DataTable Datatable = new DataTable();

            //var entityType = typeof(EntitiesCollections);
            //var dataTable = new DataTable(entityType);
          //  Datatable.NewRow();

           // ItemsSource =   entitiesCollections.PeriodsCollection;
        
            //entitiesCollections.PeriodsCollection.CollectionChanged += PeriodsCollection_CollectionChanged;
        }

        private void PeriodsCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MessageBox.Show("Modificaron datos");
        }

        private void OnTargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            MessageBox.Show("Modificaron datos");
        }
    }
}
