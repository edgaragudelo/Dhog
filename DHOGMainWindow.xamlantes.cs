﻿using DHOG_WPF.DataAccess;
using DHOG_WPF.ViewModels;
using System;
using System.Windows;
using System.Linq;
using DHOG_WPF.Views;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using static DHOG_WPF.DataTypes.Types;
using System.Collections.Generic;
using DHOG_WPF.Dialogs;
using DHOG_WPF.CustomControls;
using log4net.Config;
using System.ComponentModel;
using DHOG_WPF.Util;
using System.Windows.Media;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace DHOG_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DHOGMainWindow : Window
    {
        DHOGDataBaseViewModel dhogDataBaseViewModel;
        public static double DHOGVersion = 3.2;
        private string lastTabSelected;
        private bool caseLoaded;
        private bool convertingDB;
        private DHOGDataBaseSelectionDialog selectCaseDialog;
        private ObservableCollection<InputGroupViewModel> InputGroups;
        private ObservableCollection<OutputGroupViewModel> ChartOutputGroups;
        private ObservableCollection<OutputGroupViewModel> GridOutputGroups;
        private EntitiesCollections EntitiesCollections;
        private DataBaseConverter dbConverter;
        private string currentDBFile;
        private string currentDescription;

        public string FechaOriginal { get; private set; }

        public DHOGMainWindow()
        {
            StyleManager.ApplicationTheme = new Office2016Theme();
            Office2016Palette.Palette.FontSize = 14;

            convertingDB = false;
            InitializeComponent();
            XmlConfigurator.Configure();
            dhogDataBaseViewModel = DataContext as DHOGDataBaseViewModel;
            dhogDataBaseViewModel.CaseScenarioChanged += UpdateChartTabs;
            Show();

            DBConversionBusyIndicator.IsBusy = true;

            selectCaseDialog = new DHOGDataBaseSelectionDialog();
            selectCaseDialog.DataContext = DataContext;
            selectCaseDialog.Owner = this;

            //TODO: Testing code 
            /* CodeToDelete!!! */
            /*dhogDataBaseViewModel.InputDBFile = "E:\\Alejandra\\Documents\\Proyectos\\DHOG\\Pruebas\\20171230\\DHOG_MENSUAL.accdb";
            DHOGDataBaseDataAccess dhogDataBaseAccess = new DHOGDataBaseDataAccess(dhogDataBaseViewModel.InputDBFile, dhogDataBaseViewModel.OutputDBFile);
            selectCaseDialog.ValidDBFile = true;
            /* End CodeToDelete!!!*/

            selectCaseDialog.ShowDialog();

            if (selectCaseDialog.ValidDBFile)
                LoadCase();
            else
            {
                DBConversionBusyIndicator.IsBusy = false;
                dhogDataBaseViewModel.InputDBFile = currentDBFile;
                dhogDataBaseViewModel.Description = currentDescription;
            }
        }

        private void LoadCase()
        {
            DBConversionBusyIndicator.IsBusy = true;
            DisableMenus();
            caseLoaded = true;
            lastTabSelected = "";
            try
            {
                dhogDataBaseViewModel.UpdateDBVersionAndDescription();
                if (dhogDataBaseViewModel.Version != DHOGVersion)
                {
                    caseLoaded = false;
                    RadWindow.Confirm(new DialogParameters
                    {
                        Content = "El caso seleccionado fue creado con una versión anterior \n" +
                                  "del DHOG. Para poder continuar, se debe actualizar la \n" +
                                  "Base de Datos. \n" +
                                  "¿Desea continuar?",
                        Closed = new EventHandler<WindowClosedEventArgs>(OnConfirmClosed),
                    });
                }

                if (caseLoaded)
                {
                    currentDBFile = dhogDataBaseViewModel.InputDBFile;
                    currentDescription = dhogDataBaseViewModel.Description;
                    InitializeCaseWorkingSpace();
                    DBConversionBusyIndicator.IsBusy = false;

                    string text1 = dhogDataBaseViewModel.InitialDate;

                    FechaOriginal = text1;

                    DateTime value = Convert.ToDateTime(text1);

                    text1 = value.ToString("dddd, d\" de\" MMMM yyyy");

                    dhogDataBaseViewModel.InitialDate = text1;
                }
                else if (!convertingDB)
                {
                    selectCaseDialog = new DHOGDataBaseSelectionDialog();
                    selectCaseDialog.Owner = this;
                    selectCaseDialog.DataContext = DataContext;
                    selectCaseDialog.ShowDialog();

                    if (selectCaseDialog.ValidDBFile)
                        LoadCase();
                    else
                    {
                        DBConversionBusyIndicator.IsBusy = false;
                        EnableMenus();
                        dhogDataBaseViewModel.InputDBFile = currentDBFile;
                        dhogDataBaseViewModel.Description = currentDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                RadWindow.Alert(new DialogParameters
                {
                    Content = ex.Message,
                    //Content = MessageUtil.FormatMessage("FATAL.DBConnectionError"),
                    Owner = this
                });
                DBConversionBusyIndicator.IsBusy = false;
            }
        }

        private void SelectDBFileButton_Click(object sender, RoutedEventArgs e)
        {
            DBConversionBusyIndicator.IsBusy = true;
            currentDBFile = dhogDataBaseViewModel.InputDBFile;
            currentDescription = dhogDataBaseViewModel.Description;
            selectCaseDialog = new DHOGDataBaseSelectionDialog();
            selectCaseDialog.Owner = this;
            selectCaseDialog.DataContext = DataContext;
            selectCaseDialog.ShowDialog();

            if (selectCaseDialog.ValidDBFile)
            {
                CloseAllTabs();
                LoadCase();
            }
            else
            {
                DBConversionBusyIndicator.IsBusy = false;
                EnableMenus();
                dhogDataBaseViewModel.InputDBFile = currentDBFile;
                dhogDataBaseViewModel.Description = currentDescription;
            }
        }

        private void OnConfirmClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                convertingDB = true;
                DBConversionBusyIndicator.IsBusy = true;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += ConvertDB;
                worker.RunWorkerCompleted += DBConversionCompleted;
                worker.RunWorkerAsync();
            }
            else
            {
                dhogDataBaseViewModel.InputDBFile = currentDBFile;
                dhogDataBaseViewModel.Description = currentDescription;
                selectCaseDialog.ValidDBFile = false;
                DBConversionBusyIndicator.IsBusy = false;
            }
        }

        private void ConvertDB(object sender, DoWorkEventArgs e)
        {
            dbConverter = new DataBaseConverter();
            dbConverter.ConvertDBToCurrentVersion(dhogDataBaseViewModel);
        }

        private void DBConversionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DBConversionBusyIndicator.IsBusy = false;
            if (e.Error == null)
            {
                convertingDB = false;

                if (!caseLoaded)
                    LoadCase();

                if (dbConverter.Warnings > 0)
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Content = MessageUtil.FormatMessage("ALERT.WarningsDuringDBConversion", dbConverter.Warnings),
                        Owner = this
                    });
                }
            }
            else
            {
                System.Windows.MessageBox.Show(MessageUtil.FormatMessage("FATAL.DBConnectionError"),
                                MessageUtil.FormatMessage("LABEL.DBConversion"),
                                MessageBoxButton.OK, MessageBoxImage.Error);
                dhogDataBaseViewModel.InputDBFile = currentDBFile;
                dhogDataBaseViewModel.Description = currentDescription;
            }
        }

        private void CreateInputGroups()
        {
            InputGroups = new ObservableCollection<InputGroupViewModel>();

            ObservableCollection<InputEntityViewModel> entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("DemandaBloque", new PeriodicLoadBlocksDataGrid(EntitiesCollections), EntityType.PeriodicLoadBlock),
                new InputEntityViewModel("EscenariosBasica", new ScenariosDataGrid(EntitiesCollections), EntityType.Scenario),
                new InputEntityViewModel("PeriodoBasica", new PeriodsDataGrid(EntitiesCollections), EntityType.Period),
            };
            InputGroups.Add(new InputGroupViewModel("Periodo y Demanda", entitiesInformation, new Uri(@"../Images/Period.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("BloqueBasica", new BlocksDataGrid(EntitiesCollections), EntityType.Block),
                new InputEntityViewModel("BloquePeriodo", new PeriodicBlocksDataGrid(EntitiesCollections), EntityType.PeriodicBlock)
            };
            InputGroups.Add(new InputGroupViewModel("Bloques", entitiesInformation, new Uri(@"../Images/Blocks.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("AreaBasica", new AreasDataGrid(EntitiesCollections), EntityType.Area),
                new InputEntityViewModel("AreaPeriodo", new PeriodicAreasDataGrid(EntitiesCollections), EntityType.PeriodicArea)
            };
            InputGroups.Add(new InputGroupViewModel("Áreas", entitiesInformation, new Uri(@"../Images/Areas.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("EmpresaBasica", new CompaniesDataGrid(EntitiesCollections), EntityType.Company),
                new InputEntityViewModel("EmpresaPeriodo", new PeriodicCompaniesDataGrid(EntitiesCollections), EntityType.PeriodicCompany)
            };
            InputGroups.Add(new InputGroupViewModel("Empresas", entitiesInformation, new Uri(@"../Images/Companies.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("CombustibleBasica", new FuelsDataGrid(EntitiesCollections), EntityType.Fuel),
                new InputEntityViewModel("CombustiblePeriodo", new PeriodicFuelsDataGrid(EntitiesCollections), EntityType.PeriodicFuel)
            };
            InputGroups.Add(new InputGroupViewModel("Combustibles", entitiesInformation, new Uri(@"../Images/Fuels2.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("ContratoCombustibleBasica", new FuelContractsDataGrid(EntitiesCollections), EntityType.FuelContract),
                new InputEntityViewModel("ContratoCombustiblePeriodo", new PeriodicFuelContractsDataGrid(EntitiesCollections), EntityType.PeriodicFuelContract),
                 new InputEntityViewModel("ContratoCombustibleRecurso", new RecursoFuelContractsDataGrid(EntitiesCollections), EntityType.RecursoFuelContract),
            };
            InputGroups.Add(new InputGroupViewModel("Contratos Combustibles", entitiesInformation, new Uri(@"../Images/Contracts.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("EcuacionesFC", new PFEquationsDataGrid(EntitiesCollections), EntityType.PFEquation),
                new InputEntityViewModel("RecursoHidroBasica", new HydroPlantsDataGrid(EntitiesCollections), EntityType.HydroPlant),
                new InputEntityViewModel("RecursoHidroPeriodo", new PeriodicHydroPlantsDataGrid(EntitiesCollections), EntityType.PeriodicHydroPlant),
                new InputEntityViewModel("RecursoHidroVariable", new VariableHydroPlantsDataGrid(EntitiesCollections), EntityType.VariableHydroPlant),
            };
            InputGroups.Add(new InputGroupViewModel("Recursos Hidráulicos", entitiesInformation, new Uri(@"../Images/HydroPlants.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("aportesHidricos", new PeriodicInflowsDataGrid(EntitiesCollections), EntityType.PeriodicInflow),
                new InputEntityViewModel("ElementoHidraulicoBasica", new HydroElementsDataGrid(EntitiesCollections), EntityType.HydroElement),
                new InputEntityViewModel("ElementoHidraulicoPeriodo", new PeriodicHydroElementsDataGrid(EntitiesCollections), EntityType.PeriodicHydroElement),
                new InputEntityViewModel("EmbalseBasica", new ReservoirsDataGrid(EntitiesCollections), EntityType.Reservoir),
                new InputEntityViewModel("EmbalsePeriodo", new PeriodicReservoirsDataGrid(EntitiesCollections), EntityType.PeriodicReservoir),
                new InputEntityViewModel("SistemaHidroBasica", new HydroSystemsDataGrid(EntitiesCollections), EntityType.HydroSystem),
                new InputEntityViewModel("SistemaHidroPeriodo", new PeriodicHydroSystemsDataGrid(EntitiesCollections), EntityType.PeriodicHydroSystem),
                new InputEntityViewModel("TopologiaHidraulica", new HydroTopologyPanel(EntitiesCollections), EntityType.HydroTopology)
            };
            InputGroups.Add(new InputGroupViewModel("Elementos Hidráulicos", entitiesInformation, new Uri(@"../Images/HydroElements2.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("RecursosExcluyentes", new ExcludingPlantsDataGrid(EntitiesCollections), EntityType.ExcludingPlants),
                new InputEntityViewModel("RecursoTermicoBasica", new ThermalPlantsDataGrid(EntitiesCollections), EntityType.ThermalPlant),
                new InputEntityViewModel("RecursoTermicoPeriodo", new PeriodicThermalPlantsDataGrid(EntitiesCollections), EntityType.PeriodicThermalPlant),
                new InputEntityViewModel("RecursoTermicoVariable", new VariableThermalPlantsDataGrid(EntitiesCollections), EntityType.VariableThermalPlant),
            };
            InputGroups.Add(new InputGroupViewModel("Recursos Térmicos", entitiesInformation, new Uri(@"../Images/ThermalPlants.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("RecursoNoCoBasica", new NonConventionalPlantsDataGrid(EntitiesCollections), EntityType.NonConventionalPlant),
                new InputEntityViewModel("RecursoNoCoPeriodo", new PeriodicNonConventionalPlantsDataGrid(EntitiesCollections), EntityType.PeriodicNonConventionalPlant),
                new InputEntityViewModel("RecursoNoCoBloque", new NonConventionalPlantBlocksDataGrid(EntitiesCollections), EntityType.NonConventionalPlantBlock)
            };
            InputGroups.Add(new InputGroupViewModel("Recursos No Convencionales", entitiesInformation, new Uri(@"../Images/NonConventionalPlants.png", UriKind.RelativeOrAbsolute)));

            entitiesInformation = new ObservableCollection<InputEntityViewModel>
            {
                new InputEntityViewModel("ZonaBasica", new ZonesDataGrid(EntitiesCollections), EntityType.Zone),
                new InputEntityViewModel("ZonaEspecial", new EspecialZonesDataGrid(EntitiesCollections), EntityType.Zone),
                new InputEntityViewModel("ZonaPeriodo", new PeriodicZonesDataGrid(EntitiesCollections), EntityType.PeriodicZone),
                new InputEntityViewModel("ZonaRecurso", new ZonesPlantsPanel(EntitiesCollections), EntityType.Zone),
            };
            InputGroups.Add(new InputGroupViewModel("Zonas de Seguridad", entitiesInformation, new Uri(@"../Images/Zones2.png", UriKind.RelativeOrAbsolute)));

            InputEntitiesTreeView.ItemsSource = InputGroups;
        }

        private void CreateOutputGroups()
        {
            ObservableCollection<OutputGroupViewModel> AllOutputGroups = new ObservableCollection<OutputGroupViewModel>();

            ChartOutputGroups = new ObservableCollection<OutputGroupViewModel>();
            ObservableCollection<OutputEntityViewModel> entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Costo Marginal Por Bloques", new ChartPanelView("COSTO MARGINAL POR BLOQUES", ChartOptionsType.Scenario, new MarginalCostsByBlocksChart(0), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Costo Marginal Por Escenarios", new ChartPanelView("COSTO MARGINAL POR ESCENARIOS", ChartOptionsType.Scenario, new MarginalCostsByBlocksChart(1), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Evolución Embalses", new ChartPanelView("EVOLUCIÓN EMBALSE", ChartOptionsType.Reservoir, new ReservoirEvolutionChart(), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Generación Empresa", new ChartPanelView("GENERACIÓN EMPRESA POR ESCENARIO", ChartOptionsType.Scenario, new GenerationByCompanyChart(), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Evolución SIN - Embalses", new ChartPanelView("VOLUMEN ÚTIL SIN % GWh vs CAR", ChartOptionsType.Scenario, new SINReservoirsEvolutionChart(), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Generación SIN - Día", new ChartPanelView("GENERACIÓN DIARIA DEL SISTEMA POR TECNOLOGÍA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Daily), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Generación SIN - Etapa", new ChartPanelView("GENERACIÓN DEL SISTEMA POR TECNOLOGÍA POR ETAPA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Period), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Generación SIN - Hora", new ChartPanelView("GENERACIÓN HORARIA DEL SISTEMA POR TECNOLOGÍA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Hourly), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Generación Térmica", new ChartPanelView("GENERACIÓN TÉRMICA DIARIA", ChartOptionsType.Scenario, new DailyThermalGenerationChart(), EntitiesCollections), InfoType.Chart),
                new OutputEntityViewModel("Marginal Promedio", new ChartPanelView("MARGINAL PROMEDIO", ChartOptionsType.CandleStick, new MarginalAverageChart(), EntitiesCollections), InfoType.Chart),
            };
            OutputGroupViewModel outputGroup = new OutputGroupViewModel("Gráficas", entities, new Uri(@"../Images/Charts.png", UriKind.RelativeOrAbsolute));
            ChartOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            GridOutputGroups = new ObservableCollection<OutputGroupViewModel>();
            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Generación en energía por etapa [GWh/etapa]", "Gen_Rec_Ener_Etapa", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación en potencia [MWh/etapa]", "Despacho_Potencia", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación en potencia por empresa [MWh/etapa]", "Despacho_Potencia_Empresa", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación por recurso en energía por bloque", "Gen_Rec_Ener_Bloque", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación por tecnología [GWh/etapa]", "Generación_Tecnología_Etapa", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación por tecnología [GWh/día]", "Generación_Tecnología_Día", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Generación por tecnología [GWh/hora]", "Generación_Tecnología_Hora", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Intercambio entre áreas  [GWh/bloque]", "Intercambio_Áreas", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Racionamiento del Sistema", "Racionamiento_Sistema", new SimpleDataGridView("", true), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Generación", entities, new Uri(@"../Images/Generation.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Costo marginal de combustible [USD/MBTu]", "Costo_Marginal_Combustible", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal de contrato bilateral [USD/MWh]", "Costo_Marginal_Cont_Bilateral", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal de contrato combustible", "Costo_Marginal_Contrato", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal del agua [USD/HM3]", "Costo_Marginal_Agua", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal demanda [USD/MWh]", "Costo_Marginal_Demanda", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal demanda por áreas [USD/MWh]", "Costo_Marginal_Areas", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Costo marginal promedio [USD/MWh]", "Costo_Marginal_Promedio", new SimpleDataGridView("", true), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Costos Marginales", entities, new Uri(@"../Images/MarginalCosts.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Evolución embalses empresa", "Evol_Emb_Empresa", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Evolución embalses individual % útil", "Evol_Emb_Individual", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Evolución embalses útil SIN % útil", "Evol_Emb_Util_SIN", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Vertimientos [HM3]", "Vertimientos", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Volumen individual embalses [Hm3]", "Vol_Embalses", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Turbinamientos [m3/s]", "Turbinamientos", new SimpleDataGridView("", false), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Elementos Hidráulicos", entities, new Uri(@"../Images/HydroElements2.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Generación biomasa [MWh/Bloque]", "Generación_Biomasa", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Generación de menores por etapa [MWh/Bloque]", "Generación_Menores_Etapa", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Generación eólica [MWh/Bloque]", "Generación_Eólica", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Generación geotermia [MWh/Bloque]", "Generación_Geotermia", new SimpleDataGridView("", true), InfoType.Grid),
                new OutputEntityViewModel("Generación solar [MWh/Bloque]", "Generación_Solar", new SimpleDataGridView("", true), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Energía No Convencional", entities, new Uri(@"../Images/NonConventionalPlants.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Consumo combustible por recurso [MBTu]", "Consumo_Comb_Rec", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Consumo contratos de combustible [MBTu]", "Consumo_Contratos", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Consumo por centro de combustible [MBTu]", "Consumo_Comb_CC", new SimpleDataGridView("", false), InfoType.Grid),
                new OutputEntityViewModel("Cumplimiento contratos [%]", "Cumplimiento_Contratos", new SimpleDataGridView("", true), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Combustibles", entities, new Uri(@"../Images/Fuels.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            entities = new ObservableCollection<OutputEntityViewModel>
            {
                new OutputEntityViewModel("Probabilidad de Escenarios", "Probabilidad_Escenarios", new SimpleDataGridView("ESCENARIO_OUT", true), InfoType.Grid),
            };
            outputGroup = new OutputGroupViewModel("Tabla de Escenarios", entities, new Uri(@"../Images/Scenarios.png", UriKind.RelativeOrAbsolute));
            GridOutputGroups.Add(outputGroup);
            AllOutputGroups.Add(outputGroup);

            OutputEntitiesTreeView.ItemsSource = AllOutputGroups;
        }

        public CloseableTab GetTab(string tabTitle)
        {
            return InfoTabControl.Items.OfType<CloseableTab>().FirstOrDefault(n => n.Title.Equals(tabTitle));
        }

        public IEnumerable<CloseableTab> GetChartTabs()
        {
            return InfoTabControl.Items.OfType<CloseableTab>().Where(n => n.Type.Equals(InfoType.Chart));
        }

        private void InitializeCaseWorkingSpace()
        {
            EnableMenus();
            EntitiesCollections = new EntitiesCollections(dhogDataBaseViewModel);
            EntitiesCollections.BasicPeriodsCollectionImported += UpdateChartTabs;
            CreateInputGroups();
            CreateOutputGroups(); // TODO: Delete after testing!
        }

        private void EnableMenus()
        {
            DataManagementMenuItem.IsEnabled = true;
            ModelExecutionMenuItem.IsEnabled = true;
            InputEntitiesTreeView.IsEnabled = true;
        }

        private void DisableMenus()
        {
            DataManagementMenuItem.IsEnabled = false;
            ModelExecutionMenuItem.IsEnabled = false;
            InputEntitiesTreeView.IsEnabled = false;
        }

        private void InfoTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)

        {
            if (InfoTabControl.SelectedItem is CloseableTab tab)
            {
                lastTabSelected = tab.Title;
            }
        }

        private void ImportFromExcelMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (InfoTabControl.Items.Count == 0)
            {
                lastTabSelected = "";
            }

            foreach (InputGroupViewModel informationGroup in InputGroups)
            {
                informationGroup.IsChecked = false;
                foreach (InputEntityViewModel entity in informationGroup.Entities)
                {
                    if (entity.Name.Equals(lastTabSelected))
                        entity.IsChecked = true;
                    else
                        entity.IsChecked = false;
                }
            }

            ImportExcelFileSelectionDialog excelFileSelectionDialog = new ImportExcelFileSelectionDialog(InputGroups);
            excelFileSelectionDialog.ShowDialog();

            List<EntityType> importedEntityTypes;
            foreach (InputGroupViewModel group in InputGroups)
            {
                IEnumerable<InputEntityViewModel> importedEntities = group.Entities.Where(x => x.WasImported);
                importedEntityTypes = importedEntities.Select(entity => entity.EntityType).ToList();
                EntitiesCollections.UpdateCollections(importedEntityTypes);
            }
        }

        private void ExportToExcelMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            CreateOutputGroups();
            ExportDataSelectionDialog exportDataSelectionDialog = new ExportDataSelectionDialog(GridOutputGroups);
            exportDataSelectionDialog.ShowDialog();
        }

        private void InputEntitiesTreeView_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            InputEntitiesTreeView.BorderBrush = Brushes.Red;
            InputEntitiesTreeView.Foreground = Brushes.Red;
            try
            {
                InputEntityViewModel entity = (InputEntityViewModel)InputEntitiesTreeView.SelectedItem;
                CloseableTab tab = GetTab(entity.Name);
                if (tab == null)
                {
                    tab = new CloseableTab()
                    {
                        DataContext = entity,
                        Title = entity.Name,
                        Type = InfoType.Grid,
                        Content = entity.DataGridView
                    };
                    InfoTabControl.Items.Add(tab);
                }
                InputEntitiesTreeView.SelectedItem = false;
                entity.IsSelected = false;
                tab.IsSelected = true;
            }
            catch (InvalidCastException)
            {
                //Do Nothing!
            }
        }

        private void OutputEntitiesTreeView_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<EntityType> ColeccionUpdate = new List<EntityType>()
            {
                EntityType.Scenario
            };
            EntitiesCollections.UpdateCollections(ColeccionUpdate);

            try
            {
                OutputEntityViewModel entity = (OutputEntityViewModel)OutputEntitiesTreeView.SelectedItem;

                CloseableTab tab = GetTab(entity.Name);
                if (tab == null)
                {

                    switch (entity.Name)
                    {
                        // Gráficas
                        case "Costo Marginal Por Bloques":
                            entity = new OutputEntityViewModel("Costo Marginal Por Bloques", new ChartPanelView("COSTO MARGINAL POR BLOQUES", ChartOptionsType.Scenario, new MarginalCostsByBlocksChart(0), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Costo Marginal Por Escenarios":
                            entity = new OutputEntityViewModel("Costo Marginal Por Escenarios", new ChartPanelView("COSTO MARGINAL POR ESCENARIOS", ChartOptionsType.Scenario, new MarginalCostsByBlocksChart(1), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Evolución Embalses":
                            entity = new OutputEntityViewModel("Evolución Embalses", new ChartPanelView("EVOLUCIÓN EMBALSE", ChartOptionsType.Reservoir, new ReservoirEvolutionChart(), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Evolución SIN - Embalses":
                            entity = new OutputEntityViewModel("Evolución SIN - Embalses", new ChartPanelView("VOLUMEN ÚTIL SIN % GWh vs CAR", ChartOptionsType.Scenario, new SINReservoirsEvolutionChart(), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Generación SIN - Día":
                            entity = new OutputEntityViewModel("Generación SIN - Día", new ChartPanelView("GENERACIÓN DIARIA DEL SISTEMA POR TECNOLOGÍA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Daily), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Generación SIN - Etapa":
                            entity = new OutputEntityViewModel("Generación SIN - Etapa", new ChartPanelView("GENERACIÓN DEL SISTEMA POR TECNOLOGÍA POR ETAPA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Period), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Generación SIN - Hora":
                            entity = new OutputEntityViewModel("Generación SIN - Hora", new ChartPanelView("GENERACIÓN HORARIA DEL SISTEMA POR TECNOLOGÍA", ChartOptionsType.Scenario, new SINGenerationChart(SINGenerationType.Hourly), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Generación Térmica":
                            entity = new OutputEntityViewModel("Generación Térmica", new ChartPanelView("GENERACIÓN TÉRMICA DIARIA", ChartOptionsType.Scenario, new DailyThermalGenerationChart(), EntitiesCollections), InfoType.Chart);
                            break;
                        case "Generación Empresa":
                            entity = new OutputEntityViewModel("Generación Empresa por Escenarios", new ChartPanelView("GENERACIÓN EMPRESA POR ESCENARIO", ChartOptionsType.Company, new GenerationByCompanyChart(), EntitiesCollections), InfoType.Chart);
                            break;

                        case "Marginal Promedio":
                            entity = new OutputEntityViewModel("Marginal Promedio", new ChartPanelView("MARGINAL PROMEDIO", ChartOptionsType.CandleStick, new MarginalAverageChart(), EntitiesCollections), InfoType.Chart);
                            break;

                        //Grids
                        case "Generación por recurso en energía por bloque":
                            entity = new OutputEntityViewModel("Generación por recurso en energía por bloque", "Gen_Rec_Ener_Bloque", new SimpleDataGridView("DESPACHO_BLOQUE", false), InfoType.Grid);
                            break;
                        case "Generación en energía por etapa [GWh/etapa]":
                            entity = new OutputEntityViewModel("Generación en energía por etapa [GWh/etapa]", "Gen_Rec_Ener_Etapa", new SimpleDataGridView("DESPACHO", false), InfoType.Grid);
                            break;
                        case "Generación por tecnología [GWh/etapa]":
                            entity = new OutputEntityViewModel("Generación por tecnología [GWh/etapa]", "Generación_Tecnología_Etapa", new SimpleDataGridView("zz_ReporteGeneracion", false), InfoType.Grid);
                            break;
                        case "Generación por tecnología [GWh/día]":
                            entity = new OutputEntityViewModel("Generación por tecnología [GWh/día]", "Generación_Tecnología_Día", new SimpleDataGridView("zz_ReporteGeneracion_dia", false), InfoType.Grid);
                            break;
                        case "Generación por tecnología [GWh/hora]":
                            entity = new OutputEntityViewModel("Generación por tecnología [GWh/hora]", "Generación_Tecnología_Hora", new SimpleDataGridView("zz_ReporteGeneracion_hora", false), InfoType.Grid);
                            break;
                        case "Generación en potencia [MWh/etapa]":
                            entity = new OutputEntityViewModel("Generación en potencia [MWh/etapa]", "Despacho_Potencia", new SimpleDataGridView("DESPACHO_POTENCIA", false), InfoType.Grid);
                            break;
                        case "Generación en potencia por empresa [MWh/etapa]":
                            entity = new OutputEntityViewModel("Generación en potencia por empresa [MWh/etapa]", "Despacho_Potencia_Empresa", new SimpleDataGridView("zz_generacionEmpresa", false), InfoType.Grid);
                            break;
                        case "Racionamiento del Sistema":
                            entity = new OutputEntityViewModel("Racionamiento del Sistema", "Racionamiento_Sistema", new SimpleDataGridView("zz_racionamientoP_B", true), InfoType.Grid);
                            break;
                        case "Intercambio entre áreas  [GWh/bloque]":
                            entity = new OutputEntityViewModel("Intercambio entre áreas  [GWh/bloque]", "Intercambio_Áreas", new SimpleDataGridView("INTERCAMBIO_AREA_BLOQUE", true), InfoType.Grid);
                            break;
                        case "Costo marginal demanda [USD/MWh]":
                            entity = new OutputEntityViewModel("Costo marginal demanda [USD/MWh]", "Costo_Marginal_Demanda", new SimpleDataGridView("zz_costoMarginal", true), InfoType.Grid);
                            break;
                        case "Costo marginal promedio [USD/MWh]":
                            entity = new OutputEntityViewModel("Costo marginal promedio [USD/MWh]", "Costo_Marginal_Promedio", new SimpleDataGridView("zz_marginalPromedio", true), InfoType.Grid);
                            break;
                        case "Costo marginal del agua [USD/HM3]":
                            entity = new OutputEntityViewModel("Costo marginal del agua [USD/HM3]", "Costo_Marginal_Agua", new SimpleDataGridView("COSTOMARGINAL_AGUA", false), InfoType.Grid);
                            break;
                        case "Costo marginal de combustible [USD/MBTu]":
                            entity = new OutputEntityViewModel("Costo marginal de combustible [USD/MBTu]", "Costo_Marginal_Combustible", new SimpleDataGridView("COSTOMARGINAL_COMBUSTIBLE", false), InfoType.Grid);
                            break;
                        case "Costo marginal de contrato combustible":
                            entity = new OutputEntityViewModel("Costo marginal de contrato combustible", "Costo_Marginal_Contrato", new SimpleDataGridView("COSTOMARGINAL_CONTRATOCOMBUSTIBLE", false), InfoType.Grid);
                            break;
                        case "Costo marginal de contrato bilateral [USD/MWh]":
                            entity = new OutputEntityViewModel("Costo marginal de contrato bilateral [USD/MWh]", "Costo_Marginal_Cont_Bilateral", new SimpleDataGridView("COSTOMARGINAL_CONTRATO", true), InfoType.Grid);
                            break;
                        case "Costo marginal demanda por áreas [USD/MWh]":
                            entity = new OutputEntityViewModel("Costo marginal demanda por áreas [USD/MWh]", "Costo_Marginal_Areas", new SimpleDataGridView("zz_costoMarginalAreas", true), InfoType.Grid);
                            break;
                        case "Evolución embalses individual % útil":
                            entity = new OutputEntityViewModel("Evolución embalses individual % útil", "Evol_Emb_Individual", new SimpleDataGridView("VOLUMEN_UTIL_EMBALSE", false), InfoType.Grid);
                            break;
                        case "Evolución embalses útil SIN % útil":
                            entity = new OutputEntityViewModel("Evolución embalses útil SIN % útil", "Evol_Emb_Util_SIN", new SimpleDataGridView("zz_EvolucionEmbalseSIN", true), InfoType.Grid);
                            break;
                        case "Evolución embalses empresa":
                            entity = new OutputEntityViewModel("Evolución embalses empresa", "Evol_Emb_Empresa", new SimpleDataGridView("zz_EvolucionEmbalseSIN_E", true), InfoType.Grid);
                            break;
                        case "Vertimientos [HM3]":
                            entity = new OutputEntityViewModel("Vertimientos [HM3]", "Vertimientos", new SimpleDataGridView("VERTIMIENTO", false), InfoType.Grid);
                            break;
                        case "Volumen individual embalses [Hm3]":
                            entity = new OutputEntityViewModel("Volumen individual embalses [Hm3]", "Vol_Embalses", new SimpleDataGridView("VOLUMEN_HM3_EMBALSE", false), InfoType.Grid);
                            break;
                        case "Turbinamientos [m3/s]":
                            entity = new OutputEntityViewModel("Turbinamientos [m3/s]", "Turbinamientos", new SimpleDataGridView("TURBINAMIENTO_BLOQUE", false), InfoType.Grid);
                            break;
                        case "Generación de menores por etapa [MWh/Bloque]":
                            entity = new OutputEntityViewModel("Generación de menores por etapa [MWh/Bloque]", "Generación_Menores_Etapa", new SimpleDataGridView("zz_generacionMenor", true), InfoType.Grid);
                            break;
                        case "Generación eólica [MWh/Bloque]":
                            entity = new OutputEntityViewModel("Generación eólica [MWh/Bloque]", "Generación_Eólica", new SimpleDataGridView("zz_generacionEolica", true), InfoType.Grid);
                            break;
                        case "Generación geotermia [MWh/Bloque]":
                            entity = new OutputEntityViewModel("Generación geotermia [MWh/Bloque]", "Generación_Geotermia", new SimpleDataGridView("zz_generacionGeotermia", true), InfoType.Grid);
                            break;
                        case "Generación solar [MWh/Bloque]":
                            entity = new OutputEntityViewModel("Generación solar [MWh/Bloque]", "Generación_Solar", new SimpleDataGridView("zz_generacionSolar", true), InfoType.Grid);
                            break;
                        case "Generación biomasa [MWh/Bloque]":
                            entity = new OutputEntityViewModel("Generación biomasa [MWh/Bloque]", "Generación_Biomasa", new SimpleDataGridView("zz_generacionBiomasa", true), InfoType.Grid);
                            break;
                        case "Cumplimiento contratos [%]":
                            entity = new OutputEntityViewModel("Cumplimiento contratos [%]", "Cumplimiento_Contratos", new SimpleDataGridView("CUMPLIMIENTO_CONTRATO_E", true), InfoType.Grid);
                            break;
                        case "Consumo combustible por recurso [MBTu]":
                            entity = new OutputEntityViewModel("Consumo combustible por recurso [MBTu]", "Consumo_Comb_Rec", new SimpleDataGridView("CONSUMO_COMBUSTIBLE_RECURSO_BLOQUE", false), InfoType.Grid);
                            break;
                        case "Consumo por centro de combustible [MBTu]":
                            entity = new OutputEntityViewModel("Consumo por centro de combustible [MBTu]", "Consumo_Comb_CC", new SimpleDataGridView("COMSUMO_COMBUSTIBLE", false), InfoType.Grid);
                            break;
                        case "Consumo contratos de combustible [MBTu]":
                            entity = new OutputEntityViewModel("Consumo contratos de combustible [MBTu]", "Consumo_Contratos", new SimpleDataGridView("CONSUMO_CONTRATO_BLOQUE", false), InfoType.Grid);
                            break;
                        case "Probabilidad de Escenarios":
                            entity = new OutputEntityViewModel("Probabilidad de Escenarios", "Probabilidad_Escenarios", new SimpleDataGridView("ESCENARIO_OUT", true), InfoType.Grid);
                            break;
                    }

                    tab = new CloseableTab()
                    {
                        DataContext = entity,
                        Title = entity.Name,
                        Type = entity.Type,
                    };

                    switch (entity.Type)
                    {
                        case InfoType.Chart:
                            try
                            {
                                entity.ChartPanel.UpdateChart();
                                tab.Content = entity.ChartPanel;
                            }
                            catch (Exception ex)
                            {
                                tab = null;
                                RadWindow.Alert(new DialogParameters
                                {
                                    Content = ex.Message,
                                    Owner = this
                                });
                            }
                            break;
                        case InfoType.Grid:
                            tab.Content = entity.Grid;
                            break;
                    }

                    if (tab != null)
                    {
                        InfoTabControl.Items.Add(tab);
                        tab.IsSelected = true;
                    }
                }
                OutputEntitiesTreeView.SelectedItem = false;
                entity.IsSelected = false;
                tab.IsSelected = true;
            }
            catch (InvalidCastException)
            {
                //Do Nothing!
            }
        }

        public void UpdateChartTabs()
        {
            IEnumerable<CloseableTab> chartTabs = GetChartTabs();
            foreach (CloseableTab chartTab in chartTabs)
            {
                OutputEntityViewModel entity = chartTab.DataContext as OutputEntityViewModel;
                try
                {
                    entity.ChartPanel.UpdateChart();
                }
                catch (Exception e)
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Content = e.Message,
                        Owner = this
                    });
                }
            }
        }

        private void CloseAllTabs()
        {
            for (int i = InfoTabControl.Items.Count - 1; i >= 0; i--)
                InfoTabControl.Items.Remove(InfoTabControl.Items[i]);
        }

        private void ImportFromSDDPMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ImportSDDPFilesSelectionDialog importSDDPFilesSelectionDialog = new ImportSDDPFilesSelectionDialog(EntitiesCollections);
            importSDDPFilesSelectionDialog.ShowDialog();
            List<EntityType> importedEntityTypes = new List<EntityType>()
            {
                EntityType.Block,
                EntityType.PeriodicLoadBlock,
                EntityType.Period,
                EntityType.PeriodicInflow,
                EntityType.HydroPlant,
                EntityType.ThermalPlant,
                EntityType.PeriodicHydroPlant,
                EntityType.PeriodicThermalPlant,
                EntityType.Reservoir,
                EntityType.PeriodicReservoir,
                EntityType.Fuel,
                EntityType.PeriodicFuel,
                EntityType.FuelContract,
                EntityType.PeriodicFuelContract
            };
            EntitiesCollections.UpdateCollections(importedEntityTypes);
        }

        private void ProblemConfigurationMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            string FechaFormateada = dhogDataBaseViewModel.InitialDate;
            ProblemConfigurationDialog problemConfigurationDialog = new ProblemConfigurationDialog(dhogDataBaseViewModel.DBFolder, dhogDataBaseViewModel.InputDBFile, dhogDataBaseViewModel.InitialDate, FechaOriginal, dhogDataBaseViewModel.Description);
            problemConfigurationDialog.ShowDialog();

            List<EntityType> ColeccionUpdate = new List<EntityType>()
            {
                EntityType.Period
            };
            EntitiesCollections.UpdateCollections(ColeccionUpdate);
            dhogDataBaseViewModel.InitialDate = FechaFormateada;
        }

        private void ExecutionMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ExecutionDialog executionDialog = new ExecutionDialog(dhogDataBaseViewModel.DBFolder, dhogDataBaseViewModel.InputDBFile, dhogDataBaseViewModel.InitialDate);
            CloseAllTabs();
            executionDialog.ShowDialog();
            var a = executionDialog.numScenarios;
        }

        private void RutasMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ExecutionDialog executionDialog = new ExecutionDialog(dhogDataBaseViewModel.DBFolder, dhogDataBaseViewModel.InputDBFile, dhogDataBaseViewModel.InitialDate);
            executionDialog.ShowDialog();
        }

        private void UserManualMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Images\\DHOGv3.0_Manual_Usuario.pdf";
            System.Diagnostics.Process.Start(filePath);
        }

        private void AboutMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            AboutDHOGDialog aboutDialog = new AboutDHOGDialog();
            aboutDialog.ShowDialog();
        }

        private void GuardarCaso_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            string FileInput;
            string FileOutput;
            FileInput = dhogDataBaseViewModel.InputDBFile;
            FileOutput = dhogDataBaseViewModel.OutputDBFile;
            GuardarCaso(FileInput, "Archivo de Entrada");
            GuardarCaso(FileOutput, "Archivo de Salida");
        }

        private void GuardarCaso(string Filename, string TipoArchivo)
        {
            string FileInput;
            string FileOutput;

            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Access Files|*.accdb";
            openFileDialog.Title = TipoArchivo;

            if (openFileDialog.ShowDialog().ToString().Equals("OK"))
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    FileInput = Filename;
                    FileOutput = openFileDialog.FileName;
                    File.Copy(FileInput, FileOutput, true);

                }
            }
        }
    }
}

