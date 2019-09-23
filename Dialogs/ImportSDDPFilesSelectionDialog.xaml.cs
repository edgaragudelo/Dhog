using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using DHOG_WPF.SDDPFilesReader;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportSDDPFilesSelectionDialog.xaml
    /// </summary>
    public partial class ImportSDDPFilesSelectionDialog : Window
    {
        FilesReadingParametersViewModel filesReadingParametersViewModel;
        int Errors;
        int Warnings;
        bool isImporting;

        public ImportSDDPFilesSelectionDialog(EntitiesCollections entitiesCollections)
        {
            InitializeComponent();

            Binding binding = new Binding();
            binding.Source = entitiesCollections;
            MappingTablesTab.SetBinding(DataContextProperty, binding);

            filesReadingParametersViewModel = FilesReadingParametersTab.DataContext as FilesReadingParametersViewModel;
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            RadOpenFolderDialog openFolderDialog = new RadOpenFolderDialog();
            openFolderDialog.ShowDialog();
            SDDPFilesFolderTextBox.Text = openFolderDialog.FileName;
            filesReadingParametersViewModel.InputFilesPath = openFolderDialog.FileName;
        }

        private void CreateScenariosCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ScenariosCreationGrid.Visibility = Visibility.Visible;
        }

        private void CreateScenariosCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScenariosCreationGrid.Visibility = Visibility.Hidden;
        }

        private void ImportSDDPFilesButton_Click(object sender, RoutedEventArgs e)
        {
            Errors = 0;
            Warnings = 0;
            bool canImportStart = false;
            if (Directory.Exists(filesReadingParametersViewModel.InputFilesPath))
            {
                canImportStart = true;

                if (filesReadingParametersViewModel.CreateScenario)
                {
                    string scenarioFolder = filesReadingParametersViewModel.GetDataObject().ScenarioFolderName + 1;
                    if (!Directory.Exists(scenarioFolder))
                    {
                        canImportStart = false;
                        MessageBox.Show(MessageUtil.FormatMessage("ERROR.InvalidFolderName", scenarioFolder),
                                        MessageUtil.FormatMessage("LABEL.SDDPFilesImportDialog"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (canImportStart)
                {
                    ImportBusyIndicator.IsBusy = true;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += ImportSDDPFiles;
                    worker.RunWorkerCompleted += ImportCompleted;
                    worker.RunWorkerAsync();
                }
            }
            else
                RadWindow.Alert(MessageUtil.FormatMessage("ERROR.InvalidFolder"));
        }

        private void ImportSDDPFiles(object sender, DoWorkEventArgs e)
        {
            isImporting = true;
            filesReadingParametersViewModel.SetRiversToOmit();
            FilesReadingParameters filesReadingParameters = filesReadingParametersViewModel.GetDataObject();
            try
            {
                InputFilesReader inputFilesReader = new InputFilesReader(filesReadingParameters);
                filesReadingParametersViewModel.ProgressValue = 0;
                filesReadingParametersViewModel.BusyContent = MessageUtil.FormatMessage("INFO.FilesReadingParametersLoaded");
                filesReadingParametersViewModel.ProgressValue = 2;

                AccessDBReader accessDBReader = new AccessDBReader();
                AccessDBWriter accessDBWriter = new AccessDBWriter(filesReadingParameters);

                int lastCaseNumber = 1;
                if (filesReadingParameters.CreateScenario)
                    lastCaseNumber = GetLastCaseNumber(filesReadingParameters.ScenarioFolderName);
                
                String fileName;
                List<Block> loadBlocks = null;
                double progressStep = 98.0 / (lastCaseNumber * 10);
                double progressValue = 2 + progressStep;
                for (int caseNumber = 1; caseNumber <= lastCaseNumber; caseNumber++)
                {
                    string scenarioInputPath;
                    if (filesReadingParameters.CreateScenario)
                    {
                        scenarioInputPath = filesReadingParameters.ScenarioFolderName + caseNumber + "\\";
                        filesReadingParameters.InputFilesPath = scenarioInputPath;
                        inputFilesReader = new InputFilesReader(filesReadingParameters);
                    }

                    if (caseNumber == 1)
                    {
                        if (filesReadingParameters.Model.Equals(Model.Middle))
                            fileName = "dese05co.dat";
                        else
                            fileName = "deme05co.dat";

                        loadBlocks = inputFilesReader.ReadLoadBlocks(fileName);
                        if (loadBlocks != null)
                        {
                            accessDBWriter.WriteLoadBlocks(loadBlocks);
                            filesReadingParametersViewModel.BusyContent = MessageUtil.FormatMessage("INFO.FilesReadingParametersLoaded");
                        }
                    }

                    int scenario = -1;
                    string caseTitle = "";
                    if (filesReadingParameters.CreateScenario)
                    {
                        filesReadingParametersViewModel.BusyContent = MessageUtil.FormatMessage("INFO.LoadingCase", caseNumber);
                        caseTitle = MessageUtil.FormatMessage("INFO.CaseTitle", caseNumber);
                    }

                    if (!filesReadingParameters.CreateScenario)
                        scenario = -1;
                    else if (filesReadingParameters.AutomaticScenarioCreation)
                        scenario = -1;
                    else if (filesReadingParameters.InitialScenarioToCreate != -1)
                        scenario = filesReadingParameters.InitialScenarioToCreate + caseNumber - 1;
                    else if (filesReadingParameters.InitialScenarioToCreate == -1 && !filesReadingParameters.AutomaticScenarioCreation)
                        scenario = caseNumber;

                    String fileName2;
                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "dese05co.dat";
                    else
                        fileName = "deme05co.dat";

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName2 = "dese05ec.dat";
                    else
                        fileName2 = "deme05ec.dat";

                    double[,] load;
                    if (filesReadingParameters.ReadEcuadorLoad)
                        load = inputFilesReader.ReadPeriodicLoad(fileName, fileName2);
                    else
                        load = inputFilesReader.ReadPeriodicLoad(fileName);
                    accessDBWriter.WritePeriodicLoad(load, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.LoadLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    Period[] basicPeriod = inputFilesReader.ReadBasicPeriod(load, loadBlocks[0].DurationFactor);
                    accessDBWriter.WriteBasicPeriod(basicPeriod, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.BasicPeriodsLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "hinflw_w.dat";
                    else
                        fileName = "hinflw.dat";

                    Dictionary<int, string> riversMapping = accessDBReader.ReadRiversMapping();
                    List<River> rivers = null;
                    rivers = inputFilesReader.ReadPeriodicRiversInflows(fileName, riversMapping);
                    if (rivers != null)
                    {
                        accessDBWriter.WritePeriodicRiversInflows(rivers, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                        filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.InflowsLoaded");
                    }
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    List<ConventionalPlant> hydroPlants = accessDBReader.ReadConventionalPlants("recursoHidroBasica", scenario);
                    Dictionary<string, ConventionalPlant> hydroPlantsMapping = accessDBReader.ReadConventionalPlantsMapping("MapeoRecursosHidro", hydroPlants);
                    inputFilesReader.ReadHydroPlantsProductionFactorAndMax("chidroco.dat", hydroPlants, hydroPlantsMapping);
                    accessDBWriter.UpdateHydroPlantsProductionFactorAndMax(hydroPlants, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.HydroPlantsUpdated");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    List<ConventionalPlant> thermalPlants = accessDBReader.ReadConventionalPlants("recursoTermicoBasica", scenario);
                    Dictionary<string, ConventionalPlant> thermalPlantsMapping = accessDBReader.ReadConventionalPlantsMapping("MapeoRecursosTermicos", thermalPlants);
                    inputFilesReader.ReadThermalPlantsParameters("ctermico.dat", thermalPlants, thermalPlantsMapping);
                    accessDBWriter.UpdateThermalPlantsParameters(thermalPlants, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.ThermalPlantsUpdated");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "pmhiseco.dat";
                    else
                        fileName = "pmhimeco.dat";

                    inputFilesReader.ReadPeriodicPlantsMaintenances(fileName, hydroPlantsMapping);
                    accessDBWriter.WritePeriodicPlantsMaintenances("recursoHidroPeriodo", hydroPlants, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.HydroPlantsMantainancesLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "pmtrseco.dat";
                    else
                        fileName = "pmtrmeco.dat";

                    inputFilesReader.ReadPeriodicPlantsMaintenances(fileName, thermalPlantsMapping);
                    accessDBWriter.WritePeriodicPlantsMaintenances("recursoTermicoPeriodo", thermalPlants, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.ThermalPlantsMantainancesLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "vminseco.dat";
                    else
                        fileName = "vminmeco.dat";

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName2 = "vespseco.dat";
                    else
                        fileName2 = "vespmeco.dat";

                    List<Reservoir> reservoirs = accessDBReader.ReadReservoirs(scenario);
                    Dictionary<string, Reservoir> reservoirsMapping = accessDBReader.ReadReservoirsMapping(reservoirs);
                    inputFilesReader.ReadReservoirsLevels("chidroco.dat", reservoirs, reservoirsMapping);
                    inputFilesReader.ReadPeriodicReservoirsLevels(fileName, LevelType.Min, reservoirsMapping);
                    inputFilesReader.ReadPeriodicReservoirsLevels(fileName2, LevelType.Max, reservoirsMapping);
                    accessDBWriter.UpdateReservoirsLevels(reservoirs, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    accessDBWriter.WritePeriodicReservoirsLevels(reservoirsMapping.Values.ToList(), filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.ReservoirsLevelsLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "combseco.dat";
                    else
                        fileName = "combmeco.dat";

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName2 = "furaseco.dat";
                    else
                        fileName2 = "furameco.dat";

                    List<Fuel> fuels = accessDBReader.ReadFuels();
                    inputFilesReader.ReadFuelsCosts("ccombuco.dat", fuels);
                    inputFilesReader.ReadPeriodicFuelsCosts(fileName, fuels);
                    inputFilesReader.ReadPeriodicFuelsCapacity(fileName2, fuels);
                    accessDBWriter.UpdateFuelsCosts(fuels);
                    accessDBWriter.WritePeriodicFuelsCostsAndCapacity(fuels, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.FuelsCostsAndCapacityLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName = "fccsseco.dat";
                    else
                        fileName = "fccsmeco.dat";

                    if (filesReadingParameters.Model.Equals(Model.Middle))
                        fileName2 = "ccse05co.dat";
                    else
                        fileName2 = "ccme05co.dat";

                    List<FuelContract> fuelContracts = accessDBReader.ReadFuelContracts();
                    inputFilesReader.ReadFuelContractsCostsAndCapacity("fuecntco.dat", fuelContracts);
                    inputFilesReader.ReadPeriodicFuelContractsCosts(fileName, fuelContracts);
                    inputFilesReader.ReadPeriodicFuelContractsCapacity(fileName2, fuelContracts);
                    accessDBWriter.UpdateFuelContracts(fuelContracts);
                    accessDBWriter.WritePeriodicFuelContractsCostsAndCapacity(fuelContracts, filesReadingParameters.CreateScenario, filesReadingParameters.AutomaticScenarioCreation, scenario);
                    filesReadingParametersViewModel.BusyContent = caseTitle + MessageUtil.FormatMessage("INFO.FuelContractsCostsAndCapacityLoaded");
                    filesReadingParametersViewModel.ProgressValue = progressValue;
                    progressValue += progressStep;

                    Errors += inputFilesReader.Errors;
                    Warnings += inputFilesReader.Wanings;
                }
            }
            catch
            {
                throw;
            }
            
        }

        private void ImportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ImportBusyIndicator.IsBusy = false;
            isImporting = false;
            if (e.Error == null)
                if(Errors > 0 || Warnings > 0)
                    MessageBox.Show(MessageUtil.FormatMessage("INFO.SDDPFilesImportedWithErrors", Errors, Warnings, filesReadingParametersViewModel.InputFilesPath),
                                    MessageUtil.FormatMessage("LABEL.SDDPFilesImportDialog"),
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                        MessageBox.Show(MessageUtil.FormatMessage("INFO.SDDPFilesImportedSuccessful"),
                                        MessageUtil.FormatMessage("LABEL.SDDPFilesImportDialog"), 
                                        MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(MessageUtil.FormatMessage("ERROR.ImportationError", e.Error.Message),
                                MessageUtil.FormatMessage("LABEL.SDDPFilesImportDialog"), 
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private int GetLastCaseNumber(string scenarioFolderName)
        {
            int lastCaseNumber = 1;
            string scenarioInputPath = scenarioFolderName + lastCaseNumber + "\\";
            while (Directory.Exists(scenarioInputPath))
            {
                lastCaseNumber++;
                scenarioInputPath = scenarioFolderName + lastCaseNumber + "\\";
            }
            lastCaseNumber--;
            return lastCaseNumber;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (isImporting)
            {
                MessageBox.Show(MessageUtil.FormatMessage("WARN.ImportInProgress"),
                                MessageUtil.FormatMessage("LABEL.SDDPFilesImportDialog"),
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }

        private void RiversMappingDataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            RiversMappingDataGrid.CurrentColumn = RiversMappingNameColumn;
            RiversMappingNameColumn.IsReadOnly = false;
        }

        private void RiversMappingDataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            RiversMappingNameColumn.IsReadOnly = true;
        }
    }
}
