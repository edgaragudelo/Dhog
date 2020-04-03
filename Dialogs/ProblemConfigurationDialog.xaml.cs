using DHOG_WPF.DataProviders;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DHOG_WPF.DataAccess;




using System.Diagnostics;
using System.IO;
using System.Text;
using log4net;


using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using DHOG_WPF.DataTypes;
using static DHOG_WPF.DataTypes.Types;
using System.Globalization;
using System.Windows.Threading;
using System.Threading;



namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ProblemConfigurationDialog.xaml
    /// </summary>
    public partial class ProblemConfigurationDialog : Window
    {
        //
        public bool CaseDone = false;
        string basedatos;
        private OleDbDataReader reader;
        string periodoinicial;
        int IdPeriodicidad = 0;
        int EscenarioDem = 0;

        RadioButton BotonResolucion, BotonTeles,BotonDemanda;
        DateTime Fecha1;
        int NumeroDias, j = 0;
        Boolean JustChecked;

      
        ExecutionParametersViewModel executionParametersViewModel;

        bool isExecuting;
        Boolean BorrarPB = true;
        string dbFolder;

        string Carga = "Cargando Regitros:";
        BackgroundWorker worker = new BackgroundWorker();
        private static readonly ILog log = LogManager.GetLogger(typeof(DataBaseManager));
        
        string dbFile,Dato,Filtro,FechaIni;
        TextBox parameterValueTextBox23 ;
        TextBox parameterValueTextBox22 = new TextBox
       {
           VerticalAlignment = VerticalAlignment.Top,
           HorizontalAlignment = HorizontalAlignment.Left,
           IsEnabled = false,

       };

        TextBox parameterValueTextBox2 = new TextBox
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,

        };

        TextBox parameterValueTextBox3 = new TextBox
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            //Width = 350,
        };

        TextBox parameterValueTextBox4 = new TextBox
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            //Width = 350,
        };

        TextBox parameterValueTextBox5 = new TextBox
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,

        };

        Binding binding = new Binding("RutaModelo");
        Binding binding1 = new Binding("RutaEjecutable");
        Binding binding2 = new Binding("RutaBD");
        Binding binding3 = new Binding("RutaSalida");
        Binding binding4 = new Binding("RutaSolver");
        string FechaInicial;

        public ProblemConfigurationDialog(string dbFolder,string dbFile1, string FechaIni1, string FechaOriginal, string Description)
        {
    
            InitializeComponent();
            CreateConstraintsGrid();
            CreateConfigurationParametersGrid();
            CreateCplexParametersGrid();
            //CreateConstraintsGridRutas();
           // CreateRutasDhogParametersGrid();
            this.dbFolder = dbFolder;
            this.dbFile = dbFile1;
            this.FechaIni = FechaIni1;

            FechaInicial = FechaIni;
            executionParametersViewModel = new ExecutionParametersViewModel();

          

            ConfigCasoPanel.DataContext = executionParametersViewModel;
            Calendario.Visibility = Visibility.Hidden;
            StatusProgress.Visibility = Visibility.Hidden;
            Progress.Visibility = Visibility.Hidden;
            //this.dbFolder = dbFolder;
            basedatos = dbFile;
            this.PInicial.Text = executionParametersViewModel.InitialPeriod.ToString();
            this.Pfinal.Text = executionParametersViewModel.FinalPeriod.ToString();

            this.FechaInicial1.Text = FechaOriginal; // FechaInicial.ToString();
            this.Description1.Text = Description.ToString();
            //FechaInicial1.Text = string.Format("{0:d}", FechaInicial);



        }

        private void CreateConstraintsGrid()
        {
            ProblemConfigurationParametersCollectionViewModel contraintsCollection = ProblemConfigurationParametersDataProvider.GetConstraints();
            int column = -2;
            int row = 0;
            bool addedRow = true;
            foreach (ProblemConfigurationParameterViewModel constraint in contraintsCollection)
            {
                if (row % 6 == 0 && addedRow)
                {
                    row = 0;
                    column += 2;
                }

                Label contraintLabel = new Label();
                contraintLabel.Content = constraint.Name;
                
                Binding binding;
                if (constraint.DBName.Equals("DEMANDA") || constraint.DBName.Equals("FCVARIABLE") || constraint.DBName.Equals("FTVARIABLE"))
                {
                    addedRow = false;
                    int tmpRow = 0;
                    ComboBox constraintComboBox = new ComboBox();
                    binding = new Binding("Value");
                    binding.Source = constraint;
                    constraintComboBox.SetBinding(ComboBox.SelectedValueProperty, binding);

                    switch (constraint.DBName)
                    {
                        case "DEMANDA":
                            tmpRow = 0;
                            binding = new Binding("LoadTypes");
                            binding.Source = contraintsCollection;
                            constraintComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);
                            constraintComboBox.ToolTip = MessageUtil.FormatMessage("TOOLTIP.LoadRestrictionTypes");
                            break;
                        case "FCVARIABLE":
                            tmpRow = 1;
                            binding = new Binding("ConversionFactorTypes");
                            binding.Source = contraintsCollection;
                            constraintComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);
                            constraintComboBox.ToolTip = MessageUtil.FormatMessage("TOOLTIP.ConversionFactorRestrictionTypes");
                            break;
                        case "FTVARIABLE":
                            tmpRow = 2;
                            binding = new Binding("ConsumptionFactorTypes");
                            binding.Source = contraintsCollection;
                            constraintComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);
                            constraintComboBox.ToolTip = MessageUtil.FormatMessage("TOOLTIP.ConsumptionFactorRestrictionTypes");
                            break;
                    }

                    Grid.SetRow(contraintLabel, tmpRow);
                    Grid.SetColumn(contraintLabel, 4);
                    ConstraintsGrid.Children.Add(contraintLabel);

                    Grid.SetRow(constraintComboBox, tmpRow);
                    Grid.SetColumn(constraintComboBox, 5);
                    ConstraintsGrid.Children.Add(constraintComboBox);
                }
                else
                {
                    addedRow = true;

                    if (((row==0) || (row==1) || (row==2)) & ((column==4) || (column==5)))
                    {
                        row=row+3;column=column=column+2;
                    }

                    if (column >= 6) column = 4;

                    Grid.SetRow(contraintLabel, row);
                    Grid.SetColumn(contraintLabel, column);
                    ConstraintsGrid.Children.Add(contraintLabel);

                    CheckBox contraintCheckBox = new CheckBox();
                    contraintCheckBox.VerticalAlignment = VerticalAlignment.Center;
                    contraintCheckBox.HorizontalAlignment = HorizontalAlignment.Left;
                    binding = new Binding("BoolValue");
                    binding.Source = constraint;
                    contraintCheckBox.SetBinding(CheckBox.IsCheckedProperty, binding);
                    Grid.SetRow(contraintCheckBox, row);
                    Grid.SetColumn(contraintCheckBox, column + 1);
                    ConstraintsGrid.Children.Add(contraintCheckBox);
                    row++;
                }
            }
        }

        private void CreateConfigurationParametersGrid()
        {
            ProblemConfigurationParametersCollectionViewModel contraintsCollection = ProblemConfigurationParametersDataProvider.GetConfigurationParameters();
            int column = -3;
            int row = 0;

            foreach (ProblemConfigurationParameterViewModel constraint in contraintsCollection)
            {
                if (row % 4 == 0)
                {
                    row = 0;
                    column += 3;
                }

                Label contraintLabel = new Label();
                contraintLabel.Content = constraint.Name;
                Grid.SetRow(contraintLabel, row);
                Grid.SetColumn(contraintLabel, column);
                ConfigurationParametersGrid.Children.Add(contraintLabel);

                CheckBox contraintCheckBox = new CheckBox();
                contraintCheckBox.VerticalAlignment = VerticalAlignment.Center;
                contraintCheckBox.HorizontalAlignment = HorizontalAlignment.Left;
                Binding binding = new Binding("BoolValue");
                binding.Source = constraint;
                contraintCheckBox.SetBinding(CheckBox.IsCheckedProperty, binding);
                Grid.SetRow(contraintCheckBox, row);
                Grid.SetColumn(contraintCheckBox, column + 1);
                ConfigurationParametersGrid.Children.Add(contraintCheckBox);
                row++;
            }
        }

        private void CreateCplexParametersGrid()
        {
            CplexParametersCollectionViewModel cplexParametersCollection = new CplexParametersCollectionViewModel();
            
            int row = 0;
            int column = -2;
            foreach (CplexParameterViewModel cplexParameter in cplexParametersCollection)
            {
                if (row % 5 == 0)
                {
                    row = 0;
                    column += 2;
                }

                Label parameterNameLabel = new Label
                {
                    Content = cplexParameter.Name,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    ToolTip = cplexParameter.Description
                };
                Grid.SetRow(parameterNameLabel, row + 1);
                Grid.SetColumn(parameterNameLabel, column);
                CplexParametersGrid.Children.Add(parameterNameLabel);
                
                TextBox parameterValueTextBox = new TextBox
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 100,
                    ToolTip = cplexParameter.Description
                };
                Binding binding = new Binding("Value");
                binding.Source = cplexParameter;
                parameterValueTextBox.SetBinding(TextBox.TextProperty, binding);
                Grid.SetRow(parameterValueTextBox, row + 1);
                Grid.SetColumn(parameterValueTextBox, column + 1);
                CplexParametersGrid.Children.Add(parameterValueTextBox);

                row++;
            }
        }

        


        private void CreateConstraintsGridRutas()
        {
            RutasDhogParametersCollectionViewModel contraintsCollectionRutas = new RutasDhogParametersCollectionViewModel(); //   RutasDhogParametersDataProvider.GetConstraints();
         
            int column = -2;
            int row = 0;
            bool addedRow = true;
            
            foreach (RutasDhogParameterViewModel constraint in contraintsCollectionRutas)
            {
                if (row % 6 == 0 && addedRow)
                {
                    row = 0;
                    column += 2;
                }

                //Label parameterNameLabel = new Label
                //{
                //    Content = constraint.ID,
                //    VerticalAlignment = VerticalAlignment.Top,
                //    HorizontalAlignment = HorizontalAlignment.Left,
                //   // Width = 15,
                // };
                //Grid.SetRow(parameterNameLabel, row+1);
                //Grid.SetColumn(parameterNameLabel, column);
                //RutasDhogParametersGrid.Children.Add(parameterNameLabel);

                Button RutaModeloboton = new Button()
                {
                    Content = "....",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Background = "#FFF6C65B",

            };
              
                Filtro = "*.*";
                Dato = null;
                
                RutaModeloboton.Click += new RoutedEventHandler(SelectDBFileButton_Click);
                parameterValueTextBox22.TextChanged += new TextChangedEventHandler(Evento1);

                binding.Path = new PropertyPath("RutaModelo");
                binding.Source = constraint;
                //binding.Mode = BindingMode.TwoWay;
                parameterValueTextBox22.SetBinding(TextBox.TextProperty, binding);
                parameterValueTextBox23 = parameterValueTextBox22;
                Grid.SetRow(parameterValueTextBox22, row + 1);
                Grid.SetColumn(parameterValueTextBox22, column + 1);
                RutasDhogParametersGrid.Children.Add(parameterValueTextBox22);

                Grid.SetRow(RutaModeloboton, row + 1);
                Grid.SetColumn(RutaModeloboton, column +2);
                RutasDhogParametersGrid.Children.Add(RutaModeloboton);

                //----------------      

                binding1.Path = new PropertyPath("RutaEjecutable");
                binding1.Source = constraint;
                parameterValueTextBox2.SetBinding(TextBox.TextProperty, binding1);

                //parameterValueTextBox2.Text = constraint.RutaEjecutable;

                Grid.SetRow(parameterValueTextBox2, row + 1);
                Grid.SetColumn(parameterValueTextBox2, column + 3);
                RutasDhogParametersGrid.Children.Add(parameterValueTextBox2);

                Button RutaModeloboton2 = new Button()
                {
                    Content = "....",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Background = "#FFF6C65B",

                };

                Filtro = "*.*";
                Dato = null;

                Grid.SetRow(RutaModeloboton2, row + 1);
                Grid.SetColumn(RutaModeloboton2, column + 4);
                RutasDhogParametersGrid.Children.Add(RutaModeloboton2);

                RutaModeloboton2.Click += new RoutedEventHandler(SelectDBFileButton_Click2);
                parameterValueTextBox2.TextChanged += new TextChangedEventHandler(Evento2);

                //--------------------------------
                binding2.Path = new PropertyPath("RutaBD");
                binding2.Source = constraint;
                parameterValueTextBox3.SetBinding(TextBox.TextProperty, binding2);
                //parameterValueTextBox3.Text = constraint.RutaBD;
                Grid.SetRow(parameterValueTextBox3, row + 3);
                Grid.SetColumn(parameterValueTextBox3, column+1);
                RutasDhogParametersGrid.Children.Add(parameterValueTextBox3);

                Button RutaModeloboton3 = new Button()
                {
                    Content = "....",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Background = "#FFF6C65B",

                };

                Filtro = "*.*";
                Dato = null;

                Grid.SetRow(RutaModeloboton3, row + 3);
                Grid.SetColumn(RutaModeloboton3, column + 2);
                RutasDhogParametersGrid.Children.Add(RutaModeloboton3);

                RutaModeloboton3.Click += new RoutedEventHandler(SelectDBFileButton_Click3);
                parameterValueTextBox3.TextChanged += new TextChangedEventHandler(Evento3);

                //--------------------------------



                //row++;

                binding3.Path = new PropertyPath("RutaSalida");
                binding3.Source = constraint;
                parameterValueTextBox4.SetBinding(TextBox.TextProperty, binding3);

                //parameterValueTextBox4.Text = constraint.RutaSalida;
                Grid.SetRow(parameterValueTextBox4, row + 3);
                Grid.SetColumn(parameterValueTextBox4, column + 3);
                RutasDhogParametersGrid.Children.Add(parameterValueTextBox4);

                Button RutaModeloboton4 = new Button()
                {
                    Content = "....",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Background = "#FFF6C65B",

                };

                Filtro = "*.*";
                Dato = null;

                Grid.SetRow(RutaModeloboton4, row + 3);
                Grid.SetColumn(RutaModeloboton4, column + 4);
                RutasDhogParametersGrid.Children.Add(RutaModeloboton4);

                RutaModeloboton4.Click += new RoutedEventHandler(SelectDBFileButton_Click4);
                parameterValueTextBox4.TextChanged += new TextChangedEventHandler(Evento4);

                //--------------------------------

                binding4.Path = new PropertyPath("RutaSolver");
                binding4.Source = constraint;
                parameterValueTextBox5.SetBinding(TextBox.TextProperty, binding4);

                //parameterValueTextBox5.Text = constraint.RutaSolver;
                Grid.SetRow(parameterValueTextBox5, row + 5);
                Grid.SetColumn(parameterValueTextBox5, column + 1);
                RutasDhogParametersGrid.Children.Add(parameterValueTextBox5);

                Button RutaModeloboton5 = new Button()
                {
                    Content = "....",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Background = "#FFF6C65B",

                };

                Filtro = "*.*";
                Dato = null;

                Grid.SetRow(RutaModeloboton5, row + 5);
                Grid.SetColumn(RutaModeloboton5, column + 2);
                RutasDhogParametersGrid.Children.Add(RutaModeloboton5);

                RutaModeloboton5.Click += new RoutedEventHandler(SelectDBFileButton_Click5);
                parameterValueTextBox5.TextChanged += new TextChangedEventHandler(Evento5);

                //--------------------------------


            }


        }

    
        private void SelectDBFileButton_Click(object sender, EventArgs e)
      
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Access Files|*.*";
            //openFileDialog.Filter =Filtro;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))
               
             
                Dato = openFileDialog.SelectedPath;
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                Dato = openFileDialog.SelectedPath;
                parameterValueTextBox22.Text = openFileDialog.SelectedPath;
                
            }
        }

        private void SelectDBFileButton_Click2(object sender, EventArgs e)

        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Access Files|*.*";
            //openFileDialog.Filter =Filtro;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))


                Dato = openFileDialog.SelectedPath;
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                Dato = openFileDialog.SelectedPath;
                parameterValueTextBox2.Text = openFileDialog.SelectedPath;

            }
        }


        private void SelectDBFileButton_Click3(object sender, EventArgs e)

        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Access Files|*.*";
            //openFileDialog.Filter =Filtro;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))


                Dato = openFileDialog.SelectedPath;
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                Dato = openFileDialog.SelectedPath;
                parameterValueTextBox3.Text = openFileDialog.SelectedPath;

            }
        }

        private void SelectDBFileButton_Click4(object sender, EventArgs e)

        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Access Files|*.*";
            //openFileDialog.Filter =Filtro;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))


                Dato = openFileDialog.SelectedPath;
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                Dato = openFileDialog.SelectedPath;
                parameterValueTextBox4.Text = openFileDialog.SelectedPath;

            }
        }

        private void SelectDBFileButton_Click5(object sender, EventArgs e)

        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Access Files|*.*";
            //openFileDialog.Filter =Filtro;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))


                Dato = openFileDialog.SelectedPath;
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                Dato = openFileDialog.SelectedPath;
                parameterValueTextBox5.Text = openFileDialog.SelectedPath;

            }
        }


        private void Evento1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Dato))
            {
                BindingExpression be = parameterValueTextBox22.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void Evento2(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Dato))
            {
                BindingExpression be = parameterValueTextBox2.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void ConfigCaso(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window owner = this.Owner;
            Close();
            ConfigCasoDialog configCasoDialog = new ConfigCasoDialog(dbFolder, dbFile, FechaIni);
            ExecutionDialog executionDialog = new ExecutionDialog(dbFolder, dbFile, FechaIni);
            configCasoDialog.Owner = owner;
            configCasoDialog.ShowDialog();
            executionDialog.Owner = owner;
            executionDialog.ShowDialog();
        }

        private void Evento3(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Dato))
            {
                BindingExpression be = parameterValueTextBox3.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }


        private void Evento4(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Dato))
            {
                BindingExpression be = parameterValueTextBox4.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void Evento5(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Dato))
            {
                BindingExpression be = parameterValueTextBox5.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }


        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            Window owner = this.Owner;
            Close();
            ExecutionDialog executionDialog = new ExecutionDialog(dbFolder,dbFile,FechaIni);
            executionDialog.Owner = owner;
            executionDialog.ShowDialog();
        }


        private void MostrarCalendar(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //Calendario.HorizontalAlignment = HorizontalAlignment.Left;
            Calendario.Visibility = Visibility.Visible;

            //Calendario.Visibility = Visibility.Collapsed;

            Calendario.Width = 300;
            Calendario.Height = 300;


        }


        private void worker_Dowork(object sender, DoWorkEventArgs e)
        {

            worker.RunWorkerAsync();
            //Procesar();


        }
        private void Procesar()
        {
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Dowork;
            // worker.DoWork += new DoWorkEventHandler(worker_Dowork);
            worker.RunWorkerCompleted += worker_runworkedcompleto;
            // worker.DoWork += new DoWorkEventHandler(Avance);
            worker.ProgressChanged += CambioAvance;
            worker.ProgressChanged += new ProgressChangedEventHandler(CambioAvance);






            string query,query3 = "";
            int totPeriodo, totid;
            if (BotonResolucion == null)
            {
                MessageBox.Show("Debe seleccionar una resolución, para realizar el proceso", "Error de Datos");
                return;
            }

            if (BotonDemanda == null)
            {
                MessageBox.Show("Debe seleccionar un escenario de Demanda, para realizar el proceso", "Error de Datos");
                return;
            }

            //this.Progress.Visibility=Visibility.Visible;
            //this.StatusProgress.Visibility = Visibility.Visible;

            //this.Progress.Value = 0;
            //this.StatusProgress.Text = Carga;
            if (BorrarPB)
            {

              //  entitiesCollections.PeriodsCollection.RemoveAt
                //DataBaseManager.DbConnection.Open();
                query = "delete from  [periodoBasica]";

                query3 = "delete from [Embalseperiodo]";


                try
                {
                    //DataBaseManager.DbConnection.Close();
                    DataBaseManager.ExecuteQuery(query);
                    DataBaseManager.ExecuteQuery(query3);
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
                IdPeriodicidad = 1;
                
                EstablecerEscenarioDemanda(out EscenarioDem);
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo, IdPeriodicidad,EscenarioDem);
            }
            if (BotonResolucion.Name == "RbMes")
            {
                NumeroDias = 30;
                IdPeriodicidad = 2;
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo, IdPeriodicidad, EscenarioDem);
            }

            if (BotonResolucion.Name == "RbSem")
            {
                NumeroDias = 7;
                IdPeriodicidad = 3;
                CalculaDemasResoluciones(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo, IdPeriodicidad, EscenarioDem);
            }
            if (BotonResolucion.Name == "RbHor")
            {
                NumeroDias = 1;
                IdPeriodicidad = 4;
                CalcularHoras(Fecha1, PInicial.Text.Trim(), Pfinal.Text.Trim(), NumeroDias, totid, totPeriodo, IdPeriodicidad, EscenarioDem);
            }
            //  ConfiCaso_Click(sender,EventoC)
            GuardarHorizonte();
            this.StatusProgress.Text = "Proceso Terminado";

            CaseDone = true;

        }

        private void EstablecerEscenarioDemanda(out int Escenariodemanda)
        {
            Escenariodemanda = 1;
            if (BotonDemanda.Name == "RbDemAlta")
                Escenariodemanda = 1;
            if (BotonDemanda.Name == "RbDemMedia")
                Escenariodemanda = 2;
            if (BotonDemanda.Name == "RbDemBaja")
                Escenariodemanda = 3;



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




        private void CalcularHoras(DateTime Fecha, String Pinicio, String Pfin, int Dias, int totid1, int totperiodo1, int IdPeriodi, int EscenaDem)
        {
            int i, h, j;
            String query, query2, query3;


            int.TryParse(Pinicio.Trim(), out int pini);

            int.TryParse(Pfin.Trim(), out int pfinal);

            DateTime fecha2 = Convert.ToDateTime(Fecha);

            query = ""; query2 = ""; query3 = "";
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
                for (i = pini; j < pfinal + 1; i++)
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

                            query2 = "INSERT INTO [Embalseperiodo] SELECT ValoresNepPeriodo.Embalse as Nombre, ValoresNepPeriodo.Periodo, ValoresNepPeriodo.VolumenMinimo,ValoresNepPeriodo.VolumenMaximo";
                            query2 = query2 + " FROM ValoresNepPeriodo WHERE (ValoresNepPeriodo.Fecha=#" + Fecha.ToString("MM/dd/yyyy") + "#";
                            query2 = query2 + " AND ValoresNepPeriodo.IdTemporalidad = " + IdPeriodi + ")"; 

                            query3 = "delete from [Embalseperiodo] where Embalse in (select distinct Embalse from ValoresNepPeriodo where ";
                            query3 = query3 + "(ValoresNepPeriodo.Fecha =#" + Fecha.ToString("MM/dd/yyyy") + "#";
                            query3 = query3 + " AND ValoresNepPeriodo.IdTemporalidad = " + IdPeriodi + ")";




                            try
                            {

                                DataBaseManager.ExecuteQuery(query3);
                                DataBaseManager.ExecuteQuery(query2);
                                // command.ExecuteNonQuery();
                                //int rowsAffected = command.ExecuteNonQuery(); 

                            }


                            catch (Exception e)
                            {
                                log.Fatal(e.Message);
                                DataBaseManager.DbConnection.Close();
                                throw;
                            }
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
                    MaxPeriodo = Convert.ToInt32(reader.GetValue(0)) + 1;
                    MaxIds = Convert.ToInt32(reader.GetValue(1)) + 1;
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


        private void CalculaDemasResoluciones(DateTime Fecha, String Pinicio, String Pfin, int Dias, int totid1, int totperiodo1, int periodicidad, int escenariodeman)
        {
            int i;
            String query,query2,query3;


            int.TryParse(Pinicio.Trim(), out int pini);
            int.TryParse(Pfin.Trim(), out int pfinal);

            DateTime fecha2 = Convert.ToDateTime(Fecha);
            query = "";
            query2 = "";

            j = 1;
            for (i = pini; i < pfinal + 1; i++)
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



                // controlar si existe el registro

                //query2 = "select * FROM ValoresNepPeriodo WHERE (ValoresNepPeriodo.Fecha=#" + Fecha.ToString("MM/dd/yyyy") + "#";
                //query2 = query2 + " AND ValoresNepPeriodo.IdTemporalidad = " + periodicidad + ")";

                //try
                //{
                //    reader = DataBaseManager.ReadData(query2);
                //    while (reader.Read())
                //    {
                //       // Embalse = Convert.ToInt32(reader.GetValue(0)) + 1;
                //       // MaxIds = Convert.ToInt32(reader.GetValue(1)) + 1;
                //    }
                //    //DataBaseManager.DbConnection.Close();
                //}

                //catch (Exception e)
                //{
                //    log.Fatal(e.Message);
                //    DataBaseManager.DbConnection.Close();
                //    throw;
                //}


                //query2 = "Update [Embalseperiodo] set VolumenMinimo=";



                query2 = "INSERT INTO [Embalseperiodo] SELECT ValoresNepPeriodo.Embalse as Nombre, ValoresNepPeriodo.Periodo, ValoresNepPeriodo.VolumenMinimo,ValoresNepPeriodo.VolumenMaximo";
                query2 = query2 + " FROM ValoresNepPeriodo WHERE (ValoresNepPeriodo.Fecha=#" + Fecha.ToString("MM/dd/yyyy") + "#";
                query2 = query2 + " AND ValoresNepPeriodo.IdTemporalidad = " + periodicidad + ")"; // " and periodo=" + totperiodo1 + " )";

                query3 = "delete from [Embalseperiodo] where Embalse in (select distinct Embalse from ValoresNepPeriodo where ";
                query3=query3 + "(ValoresNepPeriodo.Fecha =#" + Fecha.ToString("MM/dd/yyyy") + "#";
                query3 = query3 + " AND ValoresNepPeriodo.IdTemporalidad = " + periodicidad + ")";

                totperiodo1 = totperiodo1 + 1;
                totid1 = totid1 + 1;

                try
                {
                    // DataBaseManager.DbConnection.Close();
                    DataBaseManager.ExecuteQuery(query);
                    DataBaseManager.ExecuteQuery(query3);
                    //int rowsAffected = command.ExecuteNonQuery(); 
                    DataBaseManager.ExecuteQuery(query2);

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

        private void RadioButton_CheckedDemanda(object sender, RoutedEventArgs e)
        {
            // ... Get RadioButton reference.
            var button = sender as RadioButton;

            BotonDemanda = button;
            var pp = PInicial.Text.Trim() as String;
            Fecha1 = DateTime.Parse(FechaInicial1.Text.Trim());
            Progress.Visibility = Visibility.Hidden;
            StatusProgress.Text = null;
            StatusProgress.Visibility = Visibility.Hidden;
        }


        void Avance(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {

                (sender as BackgroundWorker).ReportProgress(i + 1);
                //Thread.Sleep(100);
            }
        }

        void CambioAvance(object sender, ProgressChangedEventArgs e)
        {
            this.Progress.Value = e.ProgressPercentage;
        }


        private void GuardarHorizonte()
        {
            // Guardar los datos de configuración del caso en la tabla horizonte

            string query = string.Format("UPDATE horizonte SET EtapaInicial = " + PInicial.Text+",EtapaFinal="+Pfinal.Text);
            query = query + ",Descripcion='"+Description1.Text + "'";
            try
            {
                //DataBaseManager.DbConnection.Close();
                DataBaseManager.ExecuteQuery(query);
               
                //int rowsAffected = command.ExecuteNonQuery(); 
            }

            catch (Exception ex)
            {
                log.Fatal(ex.Message);
                DataBaseManager.DbConnection.Close();
                throw;
            }

            query = string.Format("UPDATE InfoBd SET Descripcion='" + Description1.Text + "'");
            try
            {
                //DataBaseManager.DbConnection.Close();
                DataBaseManager.ExecuteQuery(query);

                //int rowsAffected = command.ExecuteNonQuery(); 
            }

            catch (Exception ex)
            {
                log.Fatal(ex.Message);
                DataBaseManager.DbConnection.Close();
                throw;
            }

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
            var s = sender as RadioButton;
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


    //


   


    //




}
