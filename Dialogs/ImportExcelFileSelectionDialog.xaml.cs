using DHOG_WPF.DataAccess;
using DHOG_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using DHOG_WPF.Util;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ExcelFileSelectionDialog.xaml
    /// </summary>
    public partial class ImportExcelFileSelectionDialog : Window
    {
        ObservableCollection<InputGroupViewModel> informationGroups;
        List<InputEntityViewModel> entitiesToImport;
        bool isImporting;

        public ImportExcelFileSelectionDialog(ObservableCollection<InputGroupViewModel> informationGroups)
        {
            InitializeComponent();
            this.informationGroups = informationGroups;

            int column = 0;
            int row = -1;
            foreach (InputGroupViewModel informationGroup in informationGroups)
            {
                if(column % 4 == 0)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    InformationGroupsPanel.RowDefinitions.Add(rowDefinition);
                    row++;
                    column = 0;
                }

                List<InputGroupViewModel> informationGroupObservable = new List<InputGroupViewModel>();
                informationGroupObservable.Add(informationGroup);

                RadTreeView treeView = new RadTreeView
                {
                    ItemsSource = informationGroupObservable,
                    ItemTemplate = Application.Current.FindResource("GroupTemplate") as HierarchicalDataTemplate,
                    Margin = new Thickness(8)
                };
                
                Grid.SetRow(treeView, row);
                Grid.SetColumn(treeView, column);
                if (informationGroup.Name.Equals("Elementos Hidráulicos"))
                    Grid.SetRowSpan(treeView, 2);
                else
                    Grid.SetRowSpan(treeView, 1);

                InformationGroupsPanel.Children.Add(treeView);
                column++;
            }

            SelectAllCheckBox.IsChecked = true;
            //ExcelFileTextBox.Text = "E:\\Alejandra\\Documents\\Proyectos\\DHOG\\Pruebas\\20171229\\dhog_2.2.xlsm";
        }

        private void ImportExcelFileButton_Click(object sender, RoutedEventArgs e)
        {
            entitiesToImport = new List<InputEntityViewModel>();
            foreach (InputGroupViewModel group in informationGroups)
                foreach (InputEntityViewModel entity in group.Entities)
                    if (entity.IsChecked)
                        entitiesToImport.Add(entity);

            if (entitiesToImport.Any())
            {
                if (File.Exists(ExcelFileTextBox.Text))
                {
                    ExcelImporter.SetFileName(ExcelFileTextBox.Text);
                    RadWindow.Confirm(new DialogParameters
                    {
                        Content = "Por favor tenga en cuenta que esta operación elimininará toda \n" +
                                  "la información de las tablas seleccionadas e importará los \n" +
                                  "datos de las hojas de Excel. Las celdas vacías en Excel serán \n" +
                                  "reemplazadas con un valor de cero (0) en la base de datos. \n\n" +
                                  "¿Desea continuar?",
                        Closed = new EventHandler<WindowClosedEventArgs>(OnConfirmClosed),
                        Owner = this
                    });
                }
                else
                {
                    MessageBox.Show("No existe el archivo seleccionado.", MessageUtil.FormatMessage("LABEL.ExcelImportDialog"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó la información a cargar.", MessageUtil.FormatMessage("LABEL.ExcelImportDialog"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnConfirmClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                ImportBusyIndicator.IsBusy = true;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += ImportFromExcel;
                worker.RunWorkerCompleted += ImportCompleted;
                worker.RunWorkerAsync();
            }
        }


        private void ImportFromExcel(object sender, DoWorkEventArgs e)
        {
            isImporting = true;
            foreach (InputEntityViewModel entity in entitiesToImport)
            {
                ExcelImporter.ImportDataFomExcel(entity.Name);
                entity.WasImported = true;
            }
        }

        private void ImportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isImporting = false;
            ImportBusyIndicator.IsBusy = false;
            if(e.Error == null)
            {
                MessageBox.Show("El proceso de importación ha terminado satisfactoriamente.", MessageUtil.FormatMessage("LABEL.ExcelImportDialog"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string entitiesNotImported = "";
                bool foundSheetWithError = false;
                string sheetWithError = null;
                foreach (InputEntityViewModel entity in entitiesToImport)
                {
                    if (!entity.WasImported)
                    {
                        if (!foundSheetWithError)
                        {
                            sheetWithError = entity.Name;
                            foundSheetWithError = true;
                        }
                        entitiesNotImported += "- " + entity.Name + Environment.NewLine;
                    }
                }
                
                MessageBox.Show("Se presentó el siguiente error durante el proceso de importación: " + Environment.NewLine +
                                 e.Error.Message + ". Revise la hoja " + sheetWithError + "." + Environment.NewLine + Environment.NewLine +
                                 "No se importaron la siguientes entidades: " + Environment.NewLine +
                                 entitiesNotImported, MessageUtil.FormatMessage("LABEL.ExcelImportDialog"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                ExcelFileTextBox.Text = openFileDialog.FileName;
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (InputGroupViewModel informationGroup in informationGroups)
                informationGroup.IsChecked = true;
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (InputGroupViewModel informationGroup in informationGroups)
                informationGroup.IsChecked = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (isImporting)
            {
                MessageBox.Show(MessageUtil.FormatMessage("WARN.ImportInProgress"),
                                MessageUtil.FormatMessage(MessageUtil.FormatMessage("LABEL.ExcelImportDialog")),
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }
    }
}
