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

        String ruta;                                     //               'Ruta del archivo dhog.xls y bases de datos
        String rutaOpl1;                                     //            'Ruta de los archivos del OPL
        String rutaModelo;                                     //         'Ruta de los archivos del modelo matemático
        String rutatemp;
        String archivoBAT, archivoBAT1;                                     //         'archivo del .bat para ejecutar modelo
        String archivoDat;                                     //         'archivo .dat del modelo
        String archivoDatUp;                                     //       'archivo .dat del modelo updates
        String archivoDatUpEsc;                                     //    'archivo .dat del modelo updates por escenario
        String archivoDatDelete;                                     //   'archivo .dat del modelo borrar tablas
        String archivoDatTemp;                                     //     'archivo .dat temporal del modelo
        String archivoDatDam;                                     //      'archivo .dat del modelo del despacho
        String archivoDatDamUp;                                     //    'archivo .dat del modelo del despacho updates
                                                                   //              'variable para parar la macro cuando no existen las carpetas
        bool  Existerutamodelo, Existerutaopl;
                                                                    //               'variable que calcula el maximo de escenarios
        ExecutionParametersViewModel executionParametersViewModel;
        bool isExecuting;
        string dbFolder;

        public int numScenarios { get; set; }

        public ExecutionDialog(string dbFolder, string dbFile, string fechaIni)
        {
            InitializeComponent();
            executionParametersViewModel = new ExecutionParametersViewModel();
            ExecutionPanel.DataContext = executionParametersViewModel;
            this.dbFolder = dbFolder;
            basedatos = dbFile;
            ruta = dbFolder;
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
                //sw.WriteLine("cd " + ruta);
                //sw.WriteLine(rutaOpl1 + "\\oplrun.exe " + rutaModelo + "\\DHOG.mod " + rutaModelo + "\\DHOG.dat " + rutaModelo + "\\DHOG_UP.dat");

                sw.WriteLine("cd /D " + ruta);
                sw.WriteLine(" python dhog.py "+basedatos);
                sw.WriteLine("pause");


            }
        }

        public void ReadFiles(string fileName, String TipoConexion)
        {
            int i, swcambios = 0;

            if (File.Exists(fileName))
            {

                string[] lineas = File.ReadAllLines(fileName, Encoding.Default);

                for (i = 0; i < lineas.Length; i++)
                {
                    if (fileName == archivoDat)
                    {
                        //if (lineas[i].IndexOf("dbexcel") != 0 && (lineas[i].IndexOf("odbc") != -1))
                        //{
                        //    lineas[i] = "DBConnection " + TipoConexion + "(" + (char)(34) + "odbc" + (Char)(34) + "," + (Char)(34) + "DRIVER={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ=" + rutatemp + ";Readonly=False" + (Char)(34) + ");";
                        //    swcambios = 1;
                        //}
                        if (lineas[i].IndexOf("access") != 0 && (lineas[i].IndexOf("access") != -1))
                        {
                            //DBConnection db("access","F:\\rightside\\DHOG\\modeloDHOG_v3.0copia\\DHOG_MENSUAL.accdb");
                            lineas[i] = "DBConnection " + TipoConexion + "(" + (char)(34) + "access" + (Char)(34) + "," + (Char)(34) + basedatos + (Char)(34) + ");";
                            swcambios = 1;
                        }
                    }

                    if //(lineas[i].IndexOf("ArchivoLp") != 0 && 
                        (lineas[i].IndexOf("ArchivoLp") != -1)
                    {
                        lineas[i] = "ArchivoLp = " + (Char)(34) + ruta + "\\\\DHOG" + (Char)(34) + ";";
                    }

                    if // (lineas[i].IndexOf("ArchivoPenalizaciones") != 0 && 
                        (lineas[i].IndexOf("ArchivoPenalizaciones") != -1)
                    {
                        lineas[i] = "ArchivoPenalizaciones = " + (Char)(34) + ruta + "\\\\DHOG_PENALIZACIONES.TXT" + (Char)(34) + ";";
                    }

                    if //(lineas[i].IndexOf("DBConnection") != 0 && 
                        (lineas[i].IndexOf("DBConnection") != -1)
                    {
                        if (swcambios == 0)
                            lineas[i] = "DBConnection " + TipoConexion + "(" + (Char)(34) + "access" + (Char)(34) + "," + (Char)(34) + ruta + "\\\\DHOG_OUT.accdb" + (Char)(34) + ");";
                    }

                    if (lineas[i].IndexOf("Pi") != -1)
                    {
                        lineas[i] = "Pi = " + this.PInicial + ";";
                        lineas[i] = lineas[i].Replace("System.Windows.Controls.TextBox: ", "");

                    }

                    if (lineas[i].IndexOf("Pf") != -1)
                    {
                        lineas[i] = "Pf = " + this.Pfinal + ";";
                        lineas[i] = lineas[i].Replace("System.Windows.Controls.TextBox: ", ""); ;
                    }

                    if (lineas[i].IndexOf("MaxEsc") != -1)
                    {
                        double maxe = Calcularmaxescenarios(0);
                        lineas[i] = "MaxEsc = " + maxe + ";";

                    }
                }

                File.WriteAllLines(fileName, lineas, Encoding.Default);
            }
        }

        void DefinirRutasArchivos(string Rutabd)
        {
            string query = null;
            rutaModelo = null;

            query = "SELECT Id, RutaModelo, RutaEjecutable, RutaBD, RutaSalida, RutaSolver FROM RutasDhog";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                rutaModelo = reader.GetString(1);
                Rutabd = reader.GetString(3);

            }

            if (rutaModelo==null)
            {
                rutaModelo = Rutabd + "\\modelo"; //Sheets("EJECUTAR").Range("nombreCarpetaModelo").value

            }

            DataBaseManager.DbConnection.Close();

            ruta = Rutabd;                                          //        'Ruta del archivo dhog.xls y bases de datos

            rutaOpl1 = Rutabd + "\\opl"; // Sheets("EJECUTAR").Range("nombreCarpetaOPL").value
           
                                              //rutaArchivoSDDP = Rutabd; //Sheets("EJECUTAR").Range("nombreCarpetaSDDP").value
                                              //rutaArchivoXM = Rutabd; //Sheets("EJECUTAR").Range("nombreCarpetaXM").value
            rutatemp = Rutabd + "\\temp";

            Existerutamodelo = false;
            Existerutaopl = false;

            if (!Directory.Exists(rutaOpl1))
            {
                Existerutaopl = false;
                Existerutamodelo = false;
            }
            else
            {
                Existerutaopl = true;
                Existerutamodelo = true;
                if (Directory.Exists(rutaModelo))
                {
                    archivoBAT = Rutabd + "\\DHOG.bat";
                    archivoBAT1 = archivoBAT;
                    archivoDat = rutaModelo + "\\DHOG.dat";                              //  '.dat del modelo DHOG
                    archivoDatUp = rutaModelo + "\\DHOG_UP.dat";                        //   '.dat del modelo DHOG updates
                    archivoDatUpEsc = rutaModelo + "\\DHOG_UP_ESC.dat";                //    '.dat del modelo DHOG updates por escenario
                    archivoDatDelete = rutaModelo + "\\DHOG_delete.dat";             //     '.dat del modelo DHOG borrado de tablas
                    archivoDatTemp = rutaModelo + "\\DHOGtemp.dat";                  //      '.dat temporal del modelo DHOG
                    archivoDatDam = rutaModelo + "\\DAM.dat";                       //       '.dat del modelo DAM
                    archivoDatDamUp = rutaModelo + "\\DAM_UP.dat";
                }
                else
                {
                    Existerutamodelo = false;
                }
            }

            if (!Existerutamodelo)
                MessageBox.Show("DHOG", "Error, no existe una carpeta con el nombre Modelo y/o Opl en la ruta de la base de datos seleccionada");
            else
            {
                creabatdhog(archivoBAT1);
                //rutatemp = rutatemp.Replace("\\", "\\\\");
                //rutaOpl1 = rutaOpl1.Replace("\\", "\\\\");
                //ruta = ruta.Replace("\\", "\\\\");
                //basedatos = basedatos.Replace("\\", "\\\\");
                //ReadFiles(archivoDat, "db");
                //ReadFiles(archivoDatUp, "db1");
                //ReadFiles(archivoDatUpEsc, "db2");
                //ReadFiles(archivoDatDelete, "db3");
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {

            Existerutamodelo = true;

            archivoBAT1 = ruta + "\\dhog.bat";
            creabatdhog(archivoBAT1);

            //DefinirRutasArchivos(dbFolder);
            //if (Existerutamodelo)
            //{
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += ExecuteOPL;
                worker.RunWorkerCompleted += ExecutionCompleted;
                worker.RunWorkerAsync();
            //}


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
                executionParametersViewModel.ExecutionStatus += e.Data + Environment.NewLine;


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

