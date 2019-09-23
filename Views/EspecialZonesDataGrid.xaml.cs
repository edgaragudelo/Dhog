using DHOG_WPF.DataAccess;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ZonesDataGrid.xaml
    /// </summary>
    public partial class EspecialZonesDataGrid : BaseDataGridView
    {
        private System.Data.OleDb.OleDbDataReader reader;
        public EspecialZonesDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {

            
        InitializeComponent();


            string query = null;
            string Contract = null;
           

        List<string> listazonas = new List<string>();
            query = "SELECT DISTINCT(Nombre) FROM ZonaBasica";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                Contract = (reader.GetString(0));

                listazonas.Add(Contract);
            }
            DataBaseManager.DbConnection.Close();
            // Contrato.DataContext = lista;
             (NameColumn).ItemsSource = listazonas; // Country.GetCountries();



            EspecialZonesCollectionViewModel items = ItemsSource as EspecialZonesCollectionViewModel;
            if (items.Count == 1)
            {
                EspecialZoneViewModel item = items[0] as EspecialZoneViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = NameColumn;
                    NameColumn.IsReadOnly = false;
                    IndiceIniColumn.IsReadOnly = false;
                    IndiceFinColumn.IsReadOnly = false;
                }
            }
        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = NameColumn;
            NameColumn.IsReadOnly = false;
            IndiceIniColumn.IsReadOnly = false;
            IndiceFinColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            //NameColumn.IsReadOnly = true;
            //IndiceIniColumn.IsReadOnly = true;
            //IndiceFinColumn.IsReadOnly = true;

            NameColumn.IsReadOnly = false;
            IndiceIniColumn.IsReadOnly = false;
            IndiceFinColumn.IsReadOnly = false;
        }
    }
}
