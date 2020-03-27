using DHOG_WPF.DataAccess;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Telerik.Windows.Controls;



namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectDHOGCaseDialog.xaml
    /// </summary>
    public partial class DHOGDataBaseSelectionDialog : Window
    {

        //OleDbDataReader reader;
       int TipoDespacho;

        public DHOGDataBaseSelectionDialog()
        {
            InitializeComponent();           
            ValidDBFile = false;

           
           


        }

        private void LoadDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(DBFileTextBox.Text))
            {
                DHOGDataBaseViewModel dhogDataBaseViewModel = DataContext as DHOGDataBaseViewModel;
                dhogDataBaseViewModel.InputDBFile = DBFileTextBox.Text;

                if (Economico.IsChecked == true) TipoDespacho = 0;
                if (Hidrotermico.IsChecked == true) TipoDespacho =1;
               

                if (File.Exists(dhogDataBaseViewModel.OutputDBFile)) //TODO: Delete when testing is over!
                {
                    ValidDBFile = true;
                    Close();
                }
                else
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Content = "No existe el archivo " + dhogDataBaseViewModel.OutputDBFile + " en la ruta seleccionada.",
                        Owner = this
                    });
                }  
            }
            else
            {
                RadWindow.Alert(new DialogParameters
                {
                    Content = "No existe el archivo " + DBFileTextBox.Text + ".",
                    Owner = this
                });
            }
        }

        public bool ValidDBFile { get; set; } //TODO: Delete when testing is over!

        private void SelectDBFileButton_Click(object sender, RoutedEventArgs e)
        {
            string Nombrefile,Directorio = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Access Files|*.accdb";


            if (DBFileTextBox.Text != "")
            {
              System.Windows.MessageBoxResult vble =   System.Windows.MessageBox.Show("Desea cambiar la base de datos?", "Carga de Base de Datos", MessageBoxButton.YesNo);
                if (vble == MessageBoxResult.Yes)
                {                   
                    if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                    {
                        DBFileTextBox.Text = openFileDialog.FileName;
                        Nombrefile = openFileDialog.SafeFileName;
                        Directorio = DBFileTextBox.Text.Replace(Nombrefile,"");
                    }
                }
            }
            else
            {
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "Access Files|*.accdb";
                if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                {
                    DBFileTextBox.Text = openFileDialog.FileName;
                    Nombrefile = openFileDialog.SafeFileName;
                    Directorio = DBFileTextBox.Text.Replace(Nombrefile, "");
                }
            }

            string Rutain, Rutaout;
            Rutain = ConfigurationManager.AppSettings.Get("RutaEntrada");
            Rutaout = ConfigurationManager.AppSettings.Get("RutaSalida");
            Rutain = DBFileTextBox.Text;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["RutaEntrada"].Value = DBFileTextBox.Text;
            config.AppSettings.Settings["RutaSalida"].Value = Directorio + "DHOG_OUT.accdb"; //  DBFileTextBox.Text;
            config.Save(ConfigurationSaveMode.Modified);

        }
    }
}

