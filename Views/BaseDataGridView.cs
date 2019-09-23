using System;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using DHOG_WPF.ViewModels;
using System.Windows.Data;
using System.Data;
using System.Threading;

namespace DHOG_WPF.Views
{
    public abstract class BaseDataGridView : RadGridView
    {
        int contador = 0;
        
        public BaseDataGridView(EntitiesCollections entitiesCollections)
        {
            //DataTable dataTable;
           // Width = 0.5;
            AutoGenerateColumns = false;
            IsReadOnly = false;
            CanUserInsertRows = true;
            //ColumnWidth = new GridViewLength(1, GridViewLengthUnitType.Star);
           // MessageBox.Show("Culture despues:", Thread.CurrentThread.CurrentCulture.ToString());
            //  GridViewImageColumn.

            
            ColumnWidth = new GridViewLength(.5, GridViewLengthUnitType.Auto);
            
            GroupRenderMode = GroupRenderMode.Flat;

            
            NewRowPosition = GridViewNewRowPosition.Bottom;
            ClipboardCopyMode = GridViewClipboardCopyMode.Cells;
            ClipboardPasteMode = GridViewClipboardPasteMode.Default;
             SelectionUnit = GridViewSelectionUnit.Mixed;
            SelectionMode = SelectionMode.Extended;
            FilteringMode = Telerik.Windows.Controls.GridView.FilteringMode.Popup;

            GroupPanelBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE6E6E6"));
            GroupPanelForeground = new SolidColorBrush(Colors.Black);
            Pasting += BaseDataGrid_Pasting;
            
            RowLoaded += BaseDataGrid_Formating;
            //RowLoaded += dataGridView1_CellFormatting;

           // this.BaseDataGrid_Formating += new RowFormattingEventHandler(radGridView1_RowFormatting);

            Binding binding = new Binding();
            binding.Source = entitiesCollections;
            binding.BindsDirectlyToSource = true;
            //binding.ConverterCulture = new System.Globalization.CultureInfo("en-US");
            //binding.ConverterCulture.NumberFormat.NumberDecimalSeparator = ".";
            SetBinding(DataContextProperty, binding);
             

        }


        public void BaseDataGrid_Formating(object sender, RowLoadedEventArgs e)
        {

            contador = contador + 1;
            System.Type Tipo = e.Row.Cells.GetType();

            if ((contador % 2) == 0)
                e.Row.Background = new SolidColorBrush(Colors.LightGray);
            else
                e.Row.Background = new SolidColorBrush(Colors.WhiteSmoke);

    
            bool Valor;
            Valor = Tipo.IsValueType;

            if (Valor)
            {
                e.Row.Cells.ToString();
                    //column.DataFormatString = "{0:F0}";
                
            }   
            
        }

        private void dataGridView1_CellFormatting(object sender, RowLoadedEventArgs e)
        {
            // If the column is the Artist column, check the
            // value.
           // MessageBox.Show("tipo celda", e.Row.Cells(0).); 
            //if (this.dataGridView1.Columns[e.ColumnIndex].Name == "Artist")
            //{
            //    if (e.Value != null)
            //    {
            //        // Check for the string "pink" in the cell.
            //        string stringValue = (string)e.Value;
            //        stringValue = stringValue.ToLower();
            //        if ((stringValue.IndexOf("pink") > -1))
            //        {
            //            e.CellStyle.BackColor = Color.Pink;
            //        }

            //    }
            //}
            //else if (this.dataGridView1.Columns[e.ColumnIndex].Name == "Release Date")
            //{
            //    ShortFormDateFormat(e);
            //}
        }

        public void BaseDataGrid_Pasting(object sender, GridViewClipboardEventArgs e)
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            string clipboardString = (string)dataObj.GetData(DataFormats.Text);
            string[] lines = clipboardString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (lines.Length > 200)
            {
                RadWindow.Alert(new DialogParameters
                {
                    Content = "La cantidad de filas a pegar excede el máximo permitido (200). \n" +
                              "Utilice la opción Gestión de Datos/Importar datos desde Excel para \n" +
                              "importar la información deseada.",
                    Owner = Application.Current.MainWindow
                });
                e.Cancel = true;
            }
        }
    }
}
