using DHOG_WPF.DataAccess;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;

namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ExportDataSelectionDialog.xaml
    /// </summary>
    public partial class ExportDataSelectionDialog : Window
    {
        ObservableCollection<OutputGroupViewModel> outputGroups;

        List<DataTable> dataTables;
        List<string> sheetNames;
        bool isExporting;

               
        public ExportDataSelectionDialog(ObservableCollection<OutputGroupViewModel> outputGroups)
        {
            InitializeComponent();
            this.outputGroups = outputGroups;

            int column = 0;
            int row = -1;

            foreach (OutputGroupViewModel outputGroup in outputGroups)
            {
                if (column % 3 == 0)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    InformationGroupsPanel.RowDefinitions.Add(rowDefinition);
                    row++;
                    column = 0;
                }

                List<OutputGroupViewModel> informationGroupObservable = new List<OutputGroupViewModel>();
                informationGroupObservable.Add(outputGroup);

                RadTreeView treeView = new RadTreeView
                {
                    ItemsSource = informationGroupObservable,
                    ItemTemplate = Application.Current.FindResource("GroupTemplate") as HierarchicalDataTemplate,
                    Margin = new Thickness(8)
                };

                Grid.SetRow(treeView, row);
                Grid.SetColumn(treeView, column);
                if (outputGroup.Name.Equals("Elementos Hidráulicos"))
                    Grid.SetRowSpan(treeView, 2);
                else
                    Grid.SetRowSpan(treeView, 1);

                InformationGroupsPanel.Children.Add(treeView);
                column++;
            }

            SelectAllCheckBox.IsChecked = true;
        }



       



        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            dataTables = new List<DataTable>();
            sheetNames = new List<string>();
            foreach (OutputGroupViewModel group in outputGroups)
                foreach (OutputEntityViewModel entity in group.Entities)
                    if (entity.IsChecked)
                    {
                        dataTables.Add(entity.Grid.ItemsSource as DataTable);
                        sheetNames.Add(entity.ShortName);
                    }

            if (dataTables.Any())
            {
                SaveFileDialog dialog = new SaveFileDialog();
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                dialog.FileName = "ResultadosDHOG";
                dialog.Filter = String.Format("{0} files|*{1}|All files (*.*)|*.*", "xlsx", formatProvider.SupportedExtensions.First());
                if (dialog.ShowDialog() == true)
                {
                    ExportBusyIndicator.IsBusy = true;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += ExportToExcel;
                    worker.RunWorkerCompleted += ExportCompleted;
                    worker.RunWorkerAsync(dialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó la información a exportar.", MessageUtil.FormatMessage("LABEL.ExportToExcel"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportToExcel(object sender, DoWorkEventArgs e)
        {
            isExporting = true;
            ExcelExporter.ExportDataTableToExcelSheet(e.Argument.ToString(), dataTables, sheetNames);
        }

        private void ExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isExporting = false;
            ExportBusyIndicator.IsBusy = false;
            if (e.Error == null)
            {
                MessageBox.Show("El proceso de exportación ha terminado satisfactoriamente.", MessageUtil.FormatMessage("LABEL.ExportToExcel"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Se presentó el siguiente error durante el proceso de exportación: " + Environment.NewLine + e.Error.Message, MessageUtil.FormatMessage("LABEL.ExportToExcel"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (isExporting)
            {
                MessageBox.Show(MessageUtil.FormatMessage("WARN.ExportInProgress"),
                                MessageUtil.FormatMessage("LABEL.ExportToExcel"),
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (OutputGroupViewModel outputGroup in outputGroups)
                outputGroup.IsChecked = true;
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (OutputGroupViewModel outputGroup in outputGroups)
                outputGroup.IsChecked = false;
        }
    }
}
