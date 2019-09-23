using DHOG_WPF.DataAccess;
using DHOG_WPF.ViewModels;
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

        OleDbDataReader reader;

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
                     
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Access Files|*.accdb";

                if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                    DBFileTextBox.Text = openFileDialog.FileName;
      
            }
    }
}

