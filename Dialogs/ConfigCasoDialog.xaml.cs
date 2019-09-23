using DHOG_WPF.DataAccess;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using log4net;


using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using DHOG_WPF.DataTypes;
using static DHOG_WPF.DataTypes.Types;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Threading;
using System.Threading;

namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ProblemConfigurationDialog.xaml
    /// </summary>
    /// 




    public partial class ConfigCasoDialog : Window
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(DataBaseManager));



        string basedatos;
        private OleDbDataReader reader;
        string periodoinicial;

        RadioButton BotonResolucion, BotonTeles;
        DateTime Fecha1;
        int NumeroDias,j = 0;
        Boolean JustChecked;

        ExecutionParametersViewModel executionParametersViewModel;
        bool isExecuting;
        Boolean BorrarPB = true;
        string dbFolder;
      
        string Carga = "Cargando Regitros:";
        BackgroundWorker worker = new BackgroundWorker();

        public ConfigCasoDialog(string dbFolder, string dbFile, string FechaInicial)
        {
            InitializeComponent();

            

            executionParametersViewModel = new ExecutionParametersViewModel();
            ConfigCasoPanel.DataContext = executionParametersViewModel;
            Calendario.Visibility = Visibility.Hidden;
            StatusProgress.Visibility = Visibility.Hidden;
            Progress.Visibility=Visibility.Hidden;
            this.dbFolder = dbFolder;
            basedatos = dbFile;
            this.FechaInicial1.Text = FechaInicial.ToString();
            FechaInicial1.Text = string.Format("{0:d}", FechaInicial);
         

            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Dowork;
           // worker.DoWork += new DoWorkEventHandler(worker_Dowork);
            worker.RunWorkerCompleted += worker_runworkedcompleto;
           // worker.DoWork += new DoWorkEventHandler(Avance);
            worker.ProgressChanged += CambioAvance;
            // worker.ProgressChanged += new ProgressChangedEventHandler(CambioAvance);
        }

        private void worker_Dowork(object sender, DoWorkEventArgs e)
        {
       
            worker.RunWorkerAsync();
            //Procesar();

       
        }

        private void Procesar()
        {
            string query = "";
            int totPeriodo, totid;
            if (BotonResolucion == null)
            {
                MessageBox.Show("Debe seleccionar una resolución, para realizar el proceso", "Error de Datos");
                return;
            }

            //this.Progress.Visibility=Visibility.Visible;
            //this.StatusProgress.Visibility = Visibility.Visible;

            //this.Progress.Value = 0;
            //this.StatusProgress.Text = Carga;
            if (BorrarPB)
            {
                //DataBaseManager.DbConnection.Open();
                query = "delete from  [periodoBasica]";
                try
                {
                    //DataBaseManager.DbConnection.Close();
                    DataBaseManager.ExecuteQuery(query);
                    totPeriodo = 1; totid = 1;
                    //int rowsAffected = command.ExecuteNonQuery(); 
                }

                catch (Exception ex)
                {
                    log.Fatal(ex.Message);
                    DataBaseManager.DbConnection.Close();
                    throw;
                }
            }
            else
            {
                MaximosIds(out totPeriodo, out totid);

            }
            
            if (BotonResolucion.Name == "RbDia")
            {
                NumeroDias = 1;
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo);
            }
            if (BotonResolucion.Name == "RbMes")
            {
                NumeroDias = 30;
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo);
            }

            if (BotonResolucion.Name == "RbSem")
            {
                NumeroDias = 7;
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo);
            }
            if (BotonResolucion.Name == "RbHor")
            {
                NumeroDias = 1;
                CalcularHoras(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo);
            }
            //  ConfiCaso_Click(sender,EventoC)
            this.StatusProgress.Text = "Proceso Terminado";

        }
        private void worker_runworkedcompleto(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Progress.Visibility = Visibility.Hidden;
                StatusProgress.Text = "Operación Cancelada";

            }
            else
            {
                Progress.Visibility = Visibility.Visible;
                //StatusProgress.Visibility = Visibility.Visible;
                //StatusProgress.Text = "Proceso Terminado";

            }
        }

        private void CambioFEcha(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DateTime fecha = Calendario.SelectedDate.Value;
            FechaInicial1.Text = fecha.ToString("d");
            Calendario.Visibility = Visibility.Hidden;
        }


       

        private void CalcularHoras(DateTime Fecha, String Pinicio, String Pfin, int Dias, int totid1, int totperiodo1)
        {
            int   i,h,j;
            String query;

        
            int.TryParse(Pinicio.Trim(), out int pini);

            int.TryParse(Pfin.Trim(), out int pfinal);

            DateTime fecha2 = Convert.ToDateTime(Fecha);
            
            query = "";
            j = 1;

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Fecha", OleDbType.VarChar);
                command.Parameters.Add("@nombre", OleDbType.Numeric);
                command.Parameters.Add("@demanda", OleDbType.Numeric);
                command.Parameters.Add("@duracionHoras", OleDbType.Numeric);
                command.Parameters.Add("@ReservaAGC", OleDbType.Numeric);
                command.Parameters.Add("@CostoRacionamiento", OleDbType.Numeric);
                command.Parameters.Add("@CAR", OleDbType.Numeric);
                command.Parameters.Add("@demandaInternacional", OleDbType.Numeric);
                command.Parameters.Add("@tasaDescuento", OleDbType.Numeric);
                command.Parameters.Add("@Escenario", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

   
                query = "INSERT INTO [periodoBasica] ([Fecha],[nombre],[demanda],[duracionHoras],[ReservaAGC],[CostoRacionamiento]";
                query = query + ",[CAR],[demandaInternacional],[tasaDescuento],[Escenario],[Id]) VALUES(@Fecha,@nombre,@demanda,@duracionHoras,@ReservaAGC,@CostoRacionamiento,";
                query = query + "@CAR,@demandaInternacional,@tasaDescuento,@Escenario,@Id)";
                DataBaseManager.DbConnection.Open();
                command.CommandText = query;
                //DataBaseManager.DbConnection.Close();
                //totPeriodo = 1;
                for (i = pini; j < pfinal+1; i++)
                {
                    Fecha = fecha2;
                    fecha2 = fecha2.AddDays(Dias);
                    worker.ReportProgress(j);
                    //this.Progress.Value = i;
                    // Thread.Sleep(1000);

                    for (h = 1; h < 25; h++)
                    {
                        if (j < pfinal + 1)
                        {
                            //query = "INSERT INTO [periodoBasica] ([Fecha],[nombre],[demanda],[duracionHoras],[ReservaAGC],[CostoRacionamiento]";
                            //query = query + ",[CAR],[demandaInternacional],[tasaDescuento],[Escenario],[Id]) VALUES(";// @Fecha,@nombre,@demanda,@duracionHoras,@ReservaAGC,@CostoRacionamiento,";
                            ////query = query + "Fecha,nombre,demanda,duracionHoras,ReservaAGC,CostoRacionamiento,";
                            //query = query + ",CAR,demandaInternacional,tasaDescuento,Escenario,Id)";
                            // query = query + "'" +  Fecha.ToString("dd/MM/yyyy") + "'" + "," + totPeriodo + "," + 0 + "," + 1 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 1 + "," + totid + ")";
                            command.Parameters["@Fecha"].Value = Fecha.ToString("dd/MM/yyyy");
                            command.Parameters["@nombre"].Value = totperiodo1;
                            command.Parameters["@demanda"].Value = 0;
                            command.Parameters["@duracionHoras"].Value = 1;
                            command.Parameters["@ReservaAGC"].Value = 0;
                            command.Parameters["@CostoRacionamiento"].Value = 0;
                            command.Parameters["@CAR"].Value = 0;
                            command.Parameters["@demandaInternacional"].Value = 0;
                            command.Parameters["@tasaDescuento"].Value = 0;
                            command.Parameters["@Escenario"].Value = 1;
                            command.Parameters["@Id"].Value = totid1;

                            command.ExecuteNonQuery();


                            //try
                            //{

                            //    //DataBaseManager.ExecuteQuery(query);
                            //    command.ExecuteNonQuery();
                            //    //int rowsAffected = command.ExecuteNonQuery(); 

                            //}


                            //catch (Exception e)
                            //{
                            //    log.Fatal(e.Message);
                            //    DataBaseManager.DbConnection.Close();
                            //    throw;
                            //}
                            totperiodo1 = totperiodo1 + 1;
                            j = j + 1;
                            totid1 = totid1 + 1;
                            query = null;
                        }
                        else
                            break;
                    }
                }

                DataBaseManager.DbConnection.Close();
               



            }

        }

        private void MaximosIds(out int MaxPeriodo, out int MaxIds)
        {
            String query = null;
            MaxPeriodo = 1;
            MaxIds = 1;
            //DataBaseManager.DbConnection.Close();
            query = "select max(nombre), max(id) from  [periodoBasica]";
            try
            {
                  reader = DataBaseManager.ReadData(query);
                while (reader.Read())
                {
                    MaxPeriodo = Convert.ToInt32(reader.GetValue(0))+1;
                    MaxIds = Convert.ToInt32(reader.GetValue(1))+1;                    
                }
                //DataBaseManager.DbConnection.Close();
            }

            catch (Exception e)
            {
                    log.Fatal(e.Message);
                    DataBaseManager.DbConnection.Close();
                    throw;
            }
            DataBaseManager.DbConnection.Close();
        }


        private void CalculaDemasResoluciones(DateTime Fecha, String Pinicio, String Pfin, int Dias,int totid1,int totperiodo1)
        {
            int i;
            String query;


            int.TryParse(Pinicio.Trim(), out int pini);
            int.TryParse(Pfin.Trim(), out int pfinal);

            DateTime fecha2 = Convert.ToDateTime(Fecha);
            query = "";

            //using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            //{
           
            

            query = "";

            j = 1;
            for (i = pini; i < pfinal+1; i++)
             {
                Fecha = fecha2;
                //this.Progress.Value = i;
                //Thread.Sleep(1000);
                worker.ReportProgress(j);
                if (Dias == 30) fecha2 = fecha2.AddMonths(1);
                else fecha2 = fecha2.AddDays(Dias);
                    TimeSpan ts = fecha2 - Fecha;
                    var differenceInHours = ts.TotalHours;
                    //command.ExecuteNonQuery();
                    

                    query = "INSERT INTO [periodoBasica] ([Fecha],[nombre],[demanda],[duracionHoras],[ReservaAGC],[CostoRacionamiento]";
                    query = query + ",[CAR],[demandaInternacional],[tasaDescuento],[Escenario],[Id]) VALUES(";
                    query = query + "'" + Fecha.ToString("dd/MM/yyyy") + "'" + "," + totperiodo1 + "," + 0 + "," + differenceInHours + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 1 + "," + totid1 + ")";

                totperiodo1 = totperiodo1 + 1;
                totid1 = totid1 + 1;

                try
                {
                       // DataBaseManager.DbConnection.Close();
                        DataBaseManager.ExecuteQuery(query);
                        //int rowsAffected = command.ExecuteNonQuery(); 

                    }


                    catch (Exception e)
                    {
                        log.Fatal(e.Message);
                        DataBaseManager.DbConnection.Close();
                        throw;
                    }


                j++;
                Carga = Carga + j.ToString();
                //this.StatusProgress.Visibility = Visibility.Visible;
                //StatusProgress.Text = Carga;
                }

                DataBaseManager.DbConnection.Close();
            this.StatusProgress.Text = "Proceso Terminado";

            //}

        }
        private void MostrarCalendar(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
      
            //Calendario.HorizontalAlignment = HorizontalAlignment.Left;
            Calendario.Visibility=Visibility.Visible;
            
            //Calendario.Visibility = Visibility.Collapsed;
            
            Calendario.Width = 300;
            Calendario.Height = 300;
   
           
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // ... Get RadioButton reference.
            var button = sender as RadioButton;

            BotonResolucion = button;
            var pp = PInicial.Text.Trim() as String;
            Fecha1 = DateTime.Parse(FechaInicial1.Text.Trim());
            Progress.Visibility = Visibility.Hidden;
            StatusProgress.Text = null;
            StatusProgress.Visibility = Visibility.Hidden;
        }

        void Avance(object sender, DoWorkEventArgs e)
        {
            for (int i=0; i<100; i++)
            {

                (sender as BackgroundWorker).ReportProgress(i + 1);
                //Thread.Sleep(100);
            }
        }

        void CambioAvance(object sender, ProgressChangedEventArgs e)
        {
            this.Progress.Value = e.ProgressPercentage;
        }

        private void ConfiCaso_Click(object sender, RoutedEventArgs e)
        {
            //
            StatusProgress.Text = "Cargando....";
            StatusProgress.Visibility = Visibility.Visible;
            //worker.RunWorkerAsync();
            Procesar();
            if (worker.IsBusy)
            {
                StatusProgress.Visibility = Visibility.Visible;
                Progress.Value = j;
            }
            else
            {
                Progress.Visibility = Visibility.Visible;
                this.StatusProgress.Text = "Proceso Terminado";


            }
            
        }

        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //this.Close();
            if (isExecuting)
            {
                MessageBox.Show(MessageUtil.FormatMessage("WARN.ExecutionInProgress"),
                                MessageUtil.FormatMessage("LABEL.ExecutionDialog"),
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;

            }
        }

        private void Telescopicos_Click(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            var s=sender as RadioButton;
            if (s.IsChecked == true)
            {
                s.IsChecked = false;
                BorrarPB = true;
            }
        }

        private void Telescopicos_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
        

            BotonTeles = button;
            if (BotonTeles.IsChecked == true) BorrarPB = false;
            else BorrarPB = true;
            JustChecked = true;
        }
    }
}

