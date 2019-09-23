using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using SpreadsheetLight;

namespace DHOG_WPF.DataAccess { 

    public class ExcelExporter
    {

        public static void ExportDataTableToExcelSheet(string fileName, List<DataTable> dataTables, List<string> sheetNames)
        {
            //Workbook workbook = new Workbook();
            SLDocument excel1 = new SLDocument();
            //excel1.AddWorksheet(fileName);

            for (int i = 0; i < dataTables.Count; i++)
            {
                DataTable dataTable = dataTables[i];
                if (dataTable != null)
                {
                  
                    string sheetName = sheetNames[i];

                    //workbook.Worksheets.Add();
                    //Worksheet worksheet = workbook.Worksheets[i];
                    //worksheet.Name = sheetName;
                    Console.WriteLine("Worksheet: " + sheetName);

                    //List<string> columnDataTypes = new List<string>();
                    //int rowIndex = 0;

                    excel1.AddWorksheet(sheetName);

                    //excel1.SetDefinedName(sheetName,sheetName);

                    excel1.ImportDataTable(1,1, dataTable, true);

                    //for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                    //{
                    //    DataColumn dataColumn = dataTable.Columns[columnIndex];
                    //    worksheet.Cells[rowIndex, columnIndex].SetValue(dataColumn.ColumnName);
                    //    columnDataTypes.Add(dataColumn.DataType.ToString());
                    //}


                    //for (rowIndex = 1; rowIndex < dataTable.Rows.Count + 1; rowIndex++)
                    //{
                    //    for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                    //    {
                    //        if (!dataTable.Rows[rowIndex - 1][columnIndex].ToString().Equals(""))
                    //        {
                    //            switch (columnDataTypes[columnIndex])
                    //            {
                    //                case "System.String":
                    //                    worksheet.Cells[rowIndex, columnIndex].SetValue(dataTable.Rows[rowIndex - 1][columnIndex].ToString());
                    //                    break;
                    //                case "System.DateTime":
                    //                    worksheet.Cells[rowIndex, columnIndex].SetValue(dataTable.Rows[rowIndex - 1][columnIndex].ToString());
                    //                    break;
                    //                case "System.Int32":
                    //                    worksheet.Cells[rowIndex, columnIndex].SetValue(Convert.ToInt32(dataTable.Rows[rowIndex - 1][columnIndex]));
                    //                    break;
                    //                case "System.Int16":
                    //                    worksheet.Cells[rowIndex, columnIndex].SetValue(Convert.ToInt16(dataTable.Rows[rowIndex - 1][columnIndex]));
                    //                    break;
                    //                case "System.Double":
                    //                    worksheet.Cells[rowIndex, columnIndex].SetValue(Convert.ToDouble(dataTable.Rows[rowIndex - 1][columnIndex]));
                    //                    break;
                    //            }
                    //        }
                    //        else
                    //            worksheet.Cells[rowIndex, columnIndex].SetValue("");
                    //    }
                    //}
                }

                //IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();

                //using (Stream output = new FileStream(fileName, FileMode.OpenOrCreate))
                //{
                //    formatProvider.Export(workbook, output);
                //}
                
            }
            excel1.DeleteWorksheet("Sheet1");
            excel1.SaveAs(fileName);
        }
        
    }
}
