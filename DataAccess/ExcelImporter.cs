using System;
using System.Linq;
using System.Data.OleDb;
using System.Data;
using DHOG_WPF.Util;

namespace DHOG_WPF.DataAccess
{
    public class ExcelImporter
    {
        private static string ConnectionString;
        private static string FileName;

        public static void SetFileName(string fileName)
        {
            FileName = fileName;
            ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 XML;IMEX=1'";
        }

        public static void ImportDataFomExcel(string sheetName)
        {
            
            try
            {
                using (OleDbConnection excelConnection = new OleDbConnection(ConnectionString))
                {
                    try
                    {
                        excelConnection.Open();
                    }
                    catch
                    {
                        throw new Exception("No se pudo abrir el archivo " + FileName);
                    }

                    DataTable dataTable = new DataTable();
                    OleDbDataAdapter excelAdapter;
                    string query;
                    if (sheetName.Equals("PeriodoBasica"))
                    {
                        query = string.Format("SELECT FORMAT([Fecha], 'dd/MM/yyyy') as [Fecha], nombre, demanda, duracionHoras, ReservaAGC, CostoRacionamiento, CAR, demandaInternacional, tasaDescuento, Escenario " +
                                              "FROM[{0}$]", sheetName);
                        excelAdapter = new OleDbDataAdapter(query, excelConnection);
                        try
                        {
                            excelAdapter.Fill(dataTable);
                        }
                        catch
                        {
                            throw new Exception(MessageUtil.FormatMessage("ERROR.DataSheetNotFound", sheetName, FileName));
                        }

                        if (dataTable.Rows.Count == 0)
                            throw new Exception(MessageUtil.FormatMessage("ERROR.NoDataInBasicPeriodSheet"));
                        else {
                            bool firstPeriodFound = false;
                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (row[1].ToString().Equals("1") && row[9].ToString().Equals("1"))
                                {
                                    firstPeriodFound = true;
                                    break;
                                }
                            }
                            if(!firstPeriodFound)
                                throw new Exception(MessageUtil.FormatMessage("ERROR.FirsPeriodNotFoundInBasicPeriodSheet"));
                        }
                    }
                    else
                    {
                        query = string.Format("SELECT * FROM [{0}$]", sheetName);
                        excelAdapter = new OleDbDataAdapter(query, excelConnection);
                        try
                        {
                            excelAdapter.Fill(dataTable);
                        }
                        catch
                        {
                            throw new Exception("No existe la hoja: " + sheetName + " en el archivo: " + FileName);
                        }
                    }


                    query = string.Format("DELETE FROM [{0}]", sheetName);
                    using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                    {
                        DataBaseManager.DbConnection.Open();
                        command.ExecuteNonQuery();
                        DataBaseManager.DbConnection.Close();
                    }

                    OleDbDataAdapter accessAdapter = new OleDbDataAdapter();
                    accessAdapter.SelectCommand = new OleDbCommand(string.Format("SELECT * FROM [{0}]", sheetName), DataBaseManager.DbConnection);

                    OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(accessAdapter);
                    commandBuilder.GetInsertCommand();
                    commandBuilder.GetUpdateCommand();

                    bool[] modifiedRows = new bool[dataTable.Rows.Count];
                    int rowsToSkipIndex = -1;
                    for(int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow row = dataTable.Rows[i];
                        for (int j = 0; j < row.ItemArray.Count(); j++)
                        {
                            if (row.ItemArray[j].ToString().Equals(""))
                                rowsToSkipIndex = i;
                            else
                            {
                                rowsToSkipIndex = -1;
                                break;
                            }
                        }
                        if (rowsToSkipIndex == -1)
                        {
                            for (int j = 0; j < row.ItemArray.Count(); j++)
                            {
                                if (row.ItemArray[j].ToString().Equals(""))
                                {
                                    row[j] = "0";
                                    modifiedRows[i] = true;
                                }
                            }
                        }
                        else
                            break;
                    }                    

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (i == rowsToSkipIndex)
                            break;
                        else if (!modifiedRows[i])
                            dataTable.Rows[i].SetAdded();
                        else
                        {
                            dataTable.Rows[i].AcceptChanges();
                            dataTable.Rows[i].SetAdded();
                        }
                    }

                    DataBaseManager.DbConnection.Open();
                    accessAdapter.Update(dataTable);
                    DataBaseManager.DbConnection.Close();
                }
                
            }
            catch
            {
                DataBaseManager.DbConnection.Close();
                throw;
            }
        }
    }
}
