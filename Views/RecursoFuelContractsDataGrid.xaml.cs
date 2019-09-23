using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Data.OleDb;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for FuelContractDataGrid.xaml
    /// </summary>
    public partial class RecursoFuelContractsDataGrid : BaseDataGridView
    {
        private OleDbDataReader reader;
        public RecursoFuelContractsDataGrid(EntitiesCollections entitiesCollections) : base(entitiesCollections)
        {
            InitializeComponent();

            //Contrato.ItemsSource
           
            string query = null;
            string Contract = null;
         
            List<string> lista = new List<string>();
            query = "SELECT DISTINCT(Nombre)FROM ContratoCombustibleBasica";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                Contract = (reader.GetString(0));
               
                lista.Add(Contract);
            }
            DataBaseManager.DbConnection.Close();
           // Contrato.DataContext = lista;
            (Contrato).ItemsSource = lista; // Country.GetCountries();

            

            RecursoFuelContractsCollectionViewModel items = ItemsSource as RecursoFuelContractsCollectionViewModel;
            if (items.Count == 1)
            {
                RecursoFuelContractViewModel item = items[0] as RecursoFuelContractViewModel;
                if (item.Name == null)
                {
                    CurrentColumn = Contrato; 
                    NameColumn.IsReadOnly = false;
                    Contrato.IsReadOnly = false;
                    
                }
            }
        }

       

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            CurrentColumn = Contrato;
            NameColumn.IsReadOnly = false;
            Contrato.IsReadOnly = false;


            //CurrentColumn = NameColumn;
            //NameColumn.IsReadOnly = false;
        }

        private void DataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {

            Contrato.IsReadOnly = false;
            NameColumn.IsReadOnly = false;
        }

        private void LLenarDatos(object sender, System.Windows.RoutedEventArgs e)
        {
            //Contrato.ItemsSource
            List<FuelContract> fuelContracts = new List<FuelContract>();
            string query = null;
            string Contract = null;
            query = "SELECT DISTINCT(Nombre)FROM ContratoCombustibleBasica";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
              // this.Contrato=(reader.GetString(0));
            Contrato.ItemsSourceBinding.Source = Contract;

            DataBaseManager.DbConnection.Close();
        }

       
    }
}
