using System;
using System.Collections.Generic;
using DHOG_WPF.DataAccess;
using System.Data;
using DHOG_WPF.ViewModels;
using System.Collections.ObjectModel;
using DHOG_WPF.Util;
using DHOG_WPF.DataTypes;
using static DHOG_WPF.DataTypes.Types;
using System.Linq;
using System.Collections;

namespace DHOG_WPF.DataProviders
{
    public class ResultsDataProvider
    {
        public static List<CandleDataInfo> GetMarginaAverageDataSeries()
        {
            List<CandleDataInfo> chartData = new List<CandleDataInfo>();
            DataTable dataTable = ResultsReader.ReadMarginalAverage();

            var scenarios = dataTable.AsEnumerable()
                .GroupBy(r => new { scenario = r["escenario"] })
                .Select(g => g.OrderBy(r => r["escenario"]).First()).Count();

            for (int i = 1; i < scenarios + 1; i++)
            {
                DataRow[] rows = dataTable.Select("escenario = " + i);

                CandleDataInfo dataItem = new CandleDataInfo()
                {
                    //Date = DateTime.Now.AddDays(1),
                    High = Convert.ToDouble(rows[0][2]),
                    Low = Convert.ToDouble(rows[1][2]),
                    Open = Convert.ToDouble(rows[2][2]),
                    Close = Convert.ToDouble(rows[3][2])
                };

                chartData.Add(dataItem);
            }
            
            return chartData;
        }

        public static List<DataSeriesViewModel> GetGenerationByCompanyDataSeries(int scenario, string selctedCompany)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();

            DataTable dataTable = ResultsReader.ReadGenerationByCompany(scenario, selctedCompany);

            dataPoints = new ObservableCollection<ChartDataPoint>();

            foreach (DataRow row in dataTable.Rows)
            {
                int period = Convert.ToInt32(row[2]);
                string periodDate = "";
                if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                    periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                else
                {
                    throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                }
                //dataPoints.Add(new ChartDataPoint(row[2].ToString(), Convert.ToDouble(row[3])));
                dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[3])));
            }

            dataSeries = new DataSeriesViewModel("Generación", dataPoints);
            dataSeriesList.Add(dataSeries);

            //Generación Objetivo
            dataTable = ResultsReader.ReadObjectiveGenerationByCompany(scenario, selctedCompany);

            dataPoints = new ObservableCollection<ChartDataPoint>();
            foreach (DataRow row in dataTable.Rows)
            {
                int period = Convert.ToInt32(row[2]);
                string periodDate = "";
                if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                    periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                else
                {
                    throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                }
                //dataPoints.Add(new ChartDataPoint(row[2].ToString(), Convert.ToDouble(row[3])));
                dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[3])));
            }
            dataSeries = new DataSeriesViewModel("Generación Objetivo", dataPoints);
            dataSeriesList.Add(dataSeries);

            return dataSeriesList;
        }

        public static List<DataSeriesViewModel> GetSINGenerationDataSeries(int scenario, SINGenerationType generationType)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();
            DataTable dataTable = ResultsReader.ReadSINGeneration(scenario, generationType);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                DataColumn column = dataTable.Columns[i];

                dataPoints = new ObservableCollection<ChartDataPoint>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int period = Convert.ToInt32(row[0]);
                    string periodDate = "";
                    if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                        periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                    else
                    {
                        throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                    }
                    dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[column])));
                }

                dataSeries = new DataSeriesViewModel(column.ColumnName, dataPoints);
                dataSeriesList.Add(dataSeries);
            }

            dataTable = ResultsReader.ReadMarginalCost(scenario);
            dataPoints = new ObservableCollection<ChartDataPoint>();
            foreach (DataRow row in dataTable.Rows)
            {
                int period = Convert.ToInt32(row[0]);
                string periodDate = "";
                if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                    periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                else
                    throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));

                dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[1])));
            }
            dataSeries = new DataSeriesViewModel(dataTable.Columns[1].ColumnName, dataPoints);
            dataSeriesList.Add(dataSeries);

            return dataSeriesList;
        }

        public static List<DataSeriesViewModel> GetDailyThermalGenerationDataSeries(int scenario)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();
            DataTable dataTable = ResultsReader.ReadDailyThermalGeneration(scenario);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                DataColumn column = dataTable.Columns[i];

                dataPoints = new ObservableCollection<ChartDataPoint>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int period = Convert.ToInt32(row[0]);
                    string periodDate = "";
                    if (DHOGDataBaseViewModel.PeriodsDate != null)
                    {
                        if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                            periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                        else
                        {
                            throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                        }
                        dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[column])));
                    }
                }

                dataSeries = new DataSeriesViewModel(column.ColumnName, dataPoints);
                dataSeriesList.Add(dataSeries);
            }

            return dataSeriesList;
        }

        public static List<DataSeriesViewModel> GetSINReservoirsEvolutionDataSeries(int scenario)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();
            DataTable dataTable = ResultsReader.ReadSINReservoirsEvolution(scenario);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                DataColumn column = dataTable.Columns[i];

                dataPoints = new ObservableCollection<ChartDataPoint>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int period = Convert.ToInt32(row[0]);
                    string periodDate = "";
                    if (DHOGDataBaseViewModel.PeriodsDate != null)
                    {
                        if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                            periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                        else
                        {
                            throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                        }
                        dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[column])));
                    }
                }

                dataSeries = new DataSeriesViewModel(column.ColumnName, dataPoints);
                dataSeriesList.Add(dataSeries);
            }

            return dataSeriesList;
        }

        public static List<DataSeriesViewModel> GetReservoirEvolutionDataSeries(int scenario, string selectedReservoir)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();
            DataTable dataTable = ResultsReader.ReadReservoirEvolution(scenario, selectedReservoir);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                DataColumn column = dataTable.Columns[i];

                dataPoints = new ObservableCollection<ChartDataPoint>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int period = Convert.ToInt32(row[0]);
                    string periodDate = "";
                    if (DHOGDataBaseViewModel.PeriodsDate != null)
                    {
                        if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                            periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                        else
                        {
                            throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                        }
                        dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[column])));
                    }
                }

                dataSeries = new DataSeriesViewModel(column.ColumnName, dataPoints);
                dataSeriesList.Add(dataSeries);
            }

            return dataSeriesList;
        }

        public static List<DataSeriesViewModel> GetMarginalCostsByBlocksDataSeries(int scenario, int tipo)
        {
            ObservableCollection<ChartDataPoint> dataPoints;
            DataSeriesViewModel dataSeries;
            List<DataSeriesViewModel> dataSeriesList = new List<DataSeriesViewModel>();
            DataTable dataTable = ResultsReader.ReadMarginalCostsByBlocks(scenario, tipo);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                DataColumn column = dataTable.Columns[i];

                dataPoints = new ObservableCollection<ChartDataPoint>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int period = Convert.ToInt32(row[0]);
                    string periodDate = "";
                    if (DHOGDataBaseViewModel.PeriodsDate != null)
                    {
                        if (period <= DHOGDataBaseViewModel.PeriodsDate.GetLength(0))
                            periodDate = DHOGDataBaseViewModel.PeriodsDate[period - 1];
                        else
                        {
                            throw new Exception(MessageUtil.FormatMessage("ERROR.ChartDateNotFound", "Generación SIN", period));
                        }
                        dataPoints.Add(new ChartDataPoint(periodDate, Convert.ToDouble(row[column])));
                    }
                }

                if (tipo == 1)
                    dataSeries = new DataSeriesViewModel("Escenario" + column.ColumnName, dataPoints);
                else
                    dataSeries = new DataSeriesViewModel("Bloque" + column.ColumnName, dataPoints);

                dataSeriesList.Add(dataSeries);
            }

            return dataSeriesList;
        }
    }
}
