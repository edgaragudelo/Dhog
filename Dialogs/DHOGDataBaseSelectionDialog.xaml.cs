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
     
       int TipoBD;
       string Rutain, Rutaout, bdentradasql, bdsalidasql,rutacodigo;

        public DHOGDataBaseSelectionDialog()
        {
            InitializeComponent();           
            ValidDBFile = false;
            LeerParametrosCarga();
        }

        private void LeerParametrosCarga()
        {
            Rutain = ConfigurationManager.AppSettings.Get("RutaEntrada");
            Rutaout = ConfigurationManager.AppSettings.Get("RutaSalida");
            rutacodigo= ConfigurationManager.AppSettings.Get("RutaCodigo");
            bdentradasql = ConfigurationManager.ConnectionStrings["DhogEntrada"].ConnectionString;
            bdsalidasql = ConfigurationManager.ConnectionStrings["DhogSalida"].ConnectionString;
        }

        private void LoadDBButton_Click(object sender, RoutedEventArgs e)
        {
            DHOGDataBaseViewModel dhogDataBaseViewModel = DataContext as DHOGDataBaseViewModel;
            if (TipoBD == 1)
            {
                if (File.Exists(DBFileTextBox.Text))
                {
                    dhogDataBaseViewModel.DBFolder = rutacodigo;
                    dhogDataBaseViewModel.TipoBD = "Access";
                    dhogDataBaseViewModel.InputDBFile = DBFileTextBox.Text;
                    dhogDataBaseViewModel.OutputDBFile = Rutaout;

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
            if (TipoBD == 2)
            {
                //dhogDataBaseViewModel.InputDBFile = bdentradasql;
                dhogDataBaseViewModel.DBFolder = rutacodigo;
                dhogDataBaseViewModel.TipoBD = "Sql Server";
                dhogDataBaseViewModel.OutputDBFile = bdsalidasql.ToString();
                dhogDataBaseViewModel.InputDBFile = DBFileTextBox.Text;                           
                ValidDBFile = true;
                Close();
            }
        }

        public bool ValidDBFile { get; set; } //TODO: Delete when testing is over!

        private void SelectDBFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (TipoBD == 1) //Access
            {
                string Nombrefile, Directorio = null;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Access Files|*.accdb";

                if (DBFileTextBox.Text != "") //selecciono un archivo
                {
                    System.Windows.MessageBoxResult vble = System.Windows.MessageBox.Show("Desea cambiar la base de datos?", "Carga de Base de Datos", MessageBoxButton.YesNo);
                    if (vble == MessageBoxResult.Yes)
                    {
                        if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                        {
                            DBFileTextBox.Text = openFileDialog.FileName;
                            Nombrefile = openFileDialog.SafeFileName;
                            Directorio = DBFileTextBox.Text.Replace(Nombrefile, "");
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

                //string Rutain, Rutaout;
                //Rutain = ConfigurationManager.AppSettings.Get("RutaEntrada");
                //Rutaout = ConfigurationManager.AppSettings.Get("RutaSalida");
                //Rutain = DBFileTextBox.Text;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["RutaEntrada"].Value = DBFileTextBox.Text;
                config.AppSettings.Settings["RutaSalida"].Value = Directorio + "DHOG_OUT.accdb"; //  DBFileTextBox.Text;
                config.Save(ConfigurationSaveMode.Modified);
                Rutain = DBFileTextBox.Text;
                Rutaout = Directorio + "DHOG_OUT.accdb";
            }

            if (TipoBD ==2)
            {
                DBFileTextBox.Text = bdentradasql;

            }


        }




        private void TipoBd(object sender, RoutedEventArgs e)
        {
            if (sender.ToString().Contains("Access"))
            {
                TipoBD = 1;
                DBFileTextBox.Text = Rutain;
            }
            if (sender.ToString().Contains("Sql Server"))
            {
                TipoBD = 2;
                DBFileTextBox.Text = bdentradasql;
            }
        }
    }
}

