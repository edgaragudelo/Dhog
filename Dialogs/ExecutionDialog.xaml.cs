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


namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ProblemConfigurationDialog.xaml
    /// </summary>
    /// 




    public partial class ExecutionDialog : Window
    {
        int oplProcessId;
        string basedatos;
        private OleDbDataReader reader;
        int Tipobasedatos;       
        String ruta;
        String archivoBAT1;                                  //               'Ruta del archivo dhog.xls y bases de datos

        //              'variable para parar la macro cuando no existen las carpetas
        bool  Existerutamodelo;
                                                                    //               'variable que calcula el maximo de escenarios
        ExecutionParametersViewModel executionParametersViewModel;
        bool isExecuting;
        string dbFolder;

        public int numScenarios { get; set; }
        public int Ejecutado { get; set; }

        public ExecutionDialog(string dbFolder, string dbFile, string fechaIni,string tipobd )
        {
            InitializeComponent();
            Ejecutado = 0;
            executionParametersViewModel = new ExecutionParametersViewModel();
            ExecutionPanel.DataContext = executionParametersViewModel;
            this.dbFolder = dbFolder;           

            basedatos = dbFile;
            ruta = dbFolder;

            if (tipobd == "Sql Server")
            {
                int jj = basedatos.IndexOf("Initial Catalog=") + 16;
                basedatos = basedatos.Substring(basedatos.IndexOf("Initial Catalog=") + 16);
                Tipobasedatos = 2;
            }
            else
                Tipobasedatos = 1;
        }

        public double Calcularmaxescenarios(double valor1)
        {
            valor1 = 1;
            double Calcularmaxescenarios = 0;
            AccessDBReader accessDBReader = new AccessDBReader();
            string queryTmp = string.Format("SELECT numeroEscenarios FROM escenariosBasica " +
                                           "WHERE activo = 1");
            reader = DataBaseManager.ReadData(queryTmp);
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el calculo de maximo de escenarios", ex.Message);
                throw;
            }
            while (reader.Read()) valor1 = (valor1 * Convert.ToDouble(reader[0]));
            Calcularmaxescenarios = valor1;
            DataBaseManager.DbConnection.Close();
            numScenarios = executionParametersViewModel.IsCaseEnabled == true?1:(int)Calcularmaxescenarios;
             
            return Calcularmaxescenarios;
        }
        public void creabatdhog(string file)
        {

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("cd /D " + ruta);
                sw.WriteLine(" python dhog.py "+ Tipobasedatos.ToString() + " " + basedatos);            
                sw.WriteLine("pause");
            }
        }

       
        

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            Existerutamodelo = true;
            archivoBAT1 = ruta + "\\dhog.bat";
            creabatdhog(archivoBAT1);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ExecuteOPL;
            worker.RunWorkerCompleted += ExecutionCompleted;
            worker.RunWorkerAsync();  
        }

        private void ExecuteOPL(object sender, DoWorkEventArgs e)
        {
            isExecuting = true;
            executionParametersViewModel.ExecutionStatus = "";
            if (Existerutamodelo)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(dbFolder + "\\DHOG.bat")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                Process opl = new Process();
                opl.StartInfo = startInfo;
                opl.OutputDataReceived += new DataReceivedEventHandler(Opl_OutputDataReceived);
                opl.ErrorDataReceived += new DataReceivedEventHandler(Opl_ErrorDataReceived);


                opl.Start();
                oplProcessId = opl.Id;
                opl.BeginErrorReadLine();
                opl.BeginOutputReadLine();
                opl.WaitForExit();
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void ExecutionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isExecuting == true)
            {
                isExecuting = false;
                MessageBox.Show(MessageUtil.FormatMessage("INFO.ExecutionCompleted"),
                        MessageUtil.FormatMessage("LABEL.ExecutionDialog"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Opl_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                executionParametersViewModel.ExecutionStatus += e.Data + Environment.NewLine;
        }

        private void ExecutionStatusTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ExecutionStatusTextBox.ScrollToEnd();
        }

        private void Opl_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                executionParametersViewModel.ExecutionStatus += e.Data + Environment.NewLine;
                if (e.Data == "TERMINo EJECUCIoN")
                {
                    //MessageBox.Show("Termino la ejecución", "Ejecución Terminada");
                    Ejecutado = 1;
                }
            }


        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (isExecuting)
            {
                //MessageBox.Show(MessageUtil.FormatMessage("WARN.ExecutionInProgress"),
                //                MessageUtil.FormatMessage("LABEL.ExecutionDialog"),
                //                MessageBoxButton.OK, MessageBoxImage.Exclamation);

                this.Hide();
                try
                {

                    Process ps = Process.GetProcessById(oplProcessId);
                    if (ps != null)
                        ps.Kill();
                }
                catch (Exception)
                {
                }

                isExecuting = false;
                e.Cancel = true;
            }
        }
    }
}

