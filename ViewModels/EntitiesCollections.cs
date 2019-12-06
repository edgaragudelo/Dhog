using DHOG_WPF.DataAccess;
using DHOG_WPF.DataProviders;
using DHOG_WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public delegate void ZonesCollectionImportedHandler();
    public delegate void BasicPeriodsCollectionImportedHandler();

    public class EntitiesCollections : ViewModelBase
    {
        public DHOGDataBaseViewModel DHOGCaseViewModel { get; private set; }
        private CompaniesDataProvider CompaniesDataProvider;
        public CompaniesCollectionViewModel CompaniesCollection { get; private set; }
        public ObservableCollection<string> CompaniesScenario1 { get; private set; }
        private HydroPlantsDataProvider HydroPlantsDataProvider;
        public HydroPlantsCollectionViewModel HydroPlantsCollection { get; private set; }
        public static List<string> hydroPlantsScenario1 { get; private set; }
        public ObservableCollection<string> HydroPlantsScenario1 { get; private set; }
        private ThermalPlantsDataProvider ThermalPlantsDataProvider;
        public ThermalPlantsCollectionViewModel ThermalPlantsCollection { get; private set; }
        public ObservableCollection<string> ThermalPlantsScenario1 { get; private set; }
        public ObservableCollection<string> PlantsCollectionScenario1 { get; private set; }
        private PeriodicInflowsDataProvider PeriodicInflowsDataProvider;
        public PeriodicInflowsCollectionViewModel PeriodicInflowsCollection { get; private set; }
        private ZonesDataProvider ZonesDataProvider;

        private EspecialZonesDataProvider EspecialZonesDataProvider;

        public ObservableCollection<string> Contratos { get; private set; }

        public ZonesCollectionViewModel ZonesCollection { get; private set; }

        public EspecialZonesCollectionViewModel EspecialZonesCollection { get; private set; }

      

        public List<string> ZoneTypes { get; private set; }
        private PeriodicZonesDataProvider PeriodicZonesDataProvider;
        public PeriodicZonesCollectionViewModel PeriodicZonesCollection { get; private set; }
        public List<string> HydroElementsTypes { get; private set; }
        private AreasDataProvider AreasDataProvider;
        public AreasCollectionViewModel AreasCollection { get; private set; }
        private BlocksDataProvider BlocksDataProvider;
        public BlocksCollectionViewModel BlocksCollection { get; private set; }
        private PeriodsDataProvider PeriodsDataProvider;
        public PeriodsCollectionViewModel PeriodsCollection { get; private set; }
        private PeriodicAreasDataProvider PeriodicAreasDataProvider;
        public PeriodicAreasCollectionViewModel PeriodicAreasCollection { get; private set; }
        private PeriodicBlocksDataProvider PeriodicBlocksDataProvider;
        public PeriodicBlocksCollectionViewModel PeriodicBlocksCollection { get; private set; }
        private FuelsDataProvider FuelsDataProvider;
        public FuelsCollectionViewModel FuelsCollection { get; private set; }
        private PeriodicFuelsDataProvider PeriodicFuelsDataProvider;
        public PeriodicFuelsCollectionViewModel PeriodicFuelsCollection { get; private set; }
        private FuelContractsDataProvider FuelContractsDataProvider;

        private RecursoFuelContractsDataProvider RecursoFuelContractsDataProvider;

        public FuelContractsCollectionViewModel FuelContractsCollection { get; private set; }

        public RecursoFuelContractsCollectionViewModel RecursoFuelContractsCollection { get; private set; }

        private PeriodicFuelContractsDataProvider PeriodicFuelContractsDataProvider;
        public PeriodicFuelContractsCollectionViewModel PeriodicFuelContractsCollection { get; private set; }
        private PeriodicLoadBlocksDataProvider PeriodicLoadBlocksDataProvider;
        public PeriodicLoadBlocksCollectionViewModel PeriodicLoadBlocksCollection { get; private set; }
        private HydroElementsDataProvider HydroElementsDataProvider;
        public HydroElementsCollectionViewModel HydroElementsCollection { get; private set; }
        public static List<string> HydroElementsScenario1;
        private PeriodicHydroElementsDataProvider PeriodicHydroElementsDataProvider;
        public PeriodicHydroElementsCollectionViewModel PeriodicHydroElementsCollection { get; private set; }
        private ReservoirsDataProvider ReservoirsDataProvider;
        public ReservoirsCollectionViewModel ReservoirsCollection { get; private set; }
        public ObservableCollection<string> ReservoirsScenario1 { get; private set; }
        public static List<string> reservoirsScenario1 { get; private set; }
        private PeriodicReservoirsDataProvider PeriodicReservoirsDataProvider;
        public PeriodicReservoirsCollectionViewModel PeriodicReservoirsCollection { get; private set; }
        private PeriodicCompaniesDataProvider PeriodicCompaniesDataProvider;
        public PeriodicCompaniesCollectionViewModel PeriodicCompaniesCollection { get; private set; }
        private ScenariosDataProvider ScenariosDataProvider;
        public ScenariosActivosCollectionViewModel ScenariosCollection { get; private set; }
        public ObservableCollection<int> ActiveScenarios { get; private set; }
        private PeriodicHydroPlantsDataProvider PeriodicHydroPlantsDataProvider;
        public PeriodicConventionalPlantsCollectionViewModel PeriodicHydroPlantsCollection { get; private set; }
        public List<string> NonConventionalPlantsTypes { get; private set; }
        private NonConventionalPlantsDataProvider NonConventionalPlantsDataProvider;
        public NonConventionalPlantsCollectionViewModel NonConventionalPlantsCollection { get; private set; }
        public ObservableCollection<string> NonConventionalPlantsScenario1 { get; private set; }
        private NonConventionalPlantBlocksDataProvider NonConventionalPlantBlocksDataProvider;
        public NonConventionalPlantBlocksCollectionViewModel NonConventionalPlantBlocksCollection { get; private set; }
        private PeriodicNonConventionalPlantsDataProvider PeriodicNonConventionalPlantsDataProvider;
        public PeriodicNonConventionalPlantsCollectionViewModel PeriodicNonConventionalPlantsCollection { get; private set; }
        private PeriodicThermalPlantsDataProvider PeriodicThermalPlantsDataProvider;
        public PeriodicConventionalPlantsCollectionViewModel PeriodicThermalPlantsCollection { get; private set; }
        private HydroSystemsDataProvider HydroSystemsDataProvider;
        public HydroSystemsCollectionViewModel HydroSystemsCollection { get; private set; }
        private PeriodicHydroSystemsDataProvider PeriodicHydroSystemsDataProvider;
        public PeriodicHydroSystemsCollectionViewModel PeriodicHydroSystemsCollection { get; private set; }
        private ReservoirsMappingDataProvider ReservoirsMappingDataProvider;
        public NamesMappingCollectionViewModel ReservoirsMapping { get; private set; }
        private HydroPlantsMappingDataProvider HydroPlantsMappingDataProvider;
        public NamesMappingCollectionViewModel HydroPlantsMapping { get; private set; }
        private ThermalPlantsMappingDataProvider ThermalPlantsMappingDataProvider;
        public NamesMappingCollectionViewModel ThermalPlantsMapping { get; private set; }
        private RiversMappingDataProvider RiversMappingDataProvider;
        public NamesMappingCollectionViewModel RiversMapping { get; private set; }
        public static List<string> RiversScenario1 { get; private set; }
        private VariableHydroPlantsDataProvider VariableHydroPlantsDataProvider;
        public VariableHydroPlantsCollectionViewModel VariableHydroPlantsCollection { get; private set; }
        private VariableThermalPlantsDataProvider VariableThermalPlantsDataProvider;
        public VariableConventionalPlantsCollectionViewModel VariableThermalPlantsCollection { get; private set; }
        private PFEquationsDataProvider PFEquationsDataProvider;
        public PFEquationsCollectionViewModel PFEquationsCollection { get; private set; }
        private ExcludingPlantsDataProvider ExcludingPlantsDataProvider;
        public ExcludingPlantsCollectionViewModel ExcludingPlantsCollection { get; private set; }
        public ScenariosActivosCollectionViewModel ScenariosActivosCollection { get; private set; }

        // tablas de despacho economico
        private PeriodicBarraDataProvider PeriodicBarraDataProvider;
        public PeriodicBarraCollectionViewModel PeriodicBarraCollection { get; private set; }

        public EntitiesCollections(DHOGDataBaseViewModel dhogCaseViewModel)
        {
            DHOGCaseViewModel = dhogCaseViewModel;
            CreateDataProviders();
            CreateCollections();
            SetPeriodsDate();
            SetPeriodsDateInPeriodicCollections();
        }

        private void CreateDataProviders()
        {
            CompaniesDataProvider = new CompaniesDataProvider();
            HydroPlantsDataProvider = new HydroPlantsDataProvider();
            ThermalPlantsDataProvider = new ThermalPlantsDataProvider();
            PeriodicInflowsDataProvider = new PeriodicInflowsDataProvider();
            ZonesDataProvider = new ZonesDataProvider();
            EspecialZonesDataProvider = new EspecialZonesDataProvider();
            AreasDataProvider = new AreasDataProvider();
            BlocksDataProvider = new BlocksDataProvider();
            PeriodsDataProvider = new PeriodsDataProvider();
            PeriodicAreasDataProvider = new PeriodicAreasDataProvider();
            PeriodicBlocksDataProvider = new PeriodicBlocksDataProvider();
            FuelsDataProvider = new FuelsDataProvider();
            PeriodicFuelsDataProvider = new PeriodicFuelsDataProvider();
            FuelContractsDataProvider = new FuelContractsDataProvider();

            RecursoFuelContractsDataProvider = new RecursoFuelContractsDataProvider();

            PeriodicFuelContractsDataProvider = new PeriodicFuelContractsDataProvider();
            PeriodicLoadBlocksDataProvider = new PeriodicLoadBlocksDataProvider();
            HydroElementsDataProvider = new HydroElementsDataProvider();
            PeriodicHydroElementsDataProvider = new PeriodicHydroElementsDataProvider();
            ReservoirsDataProvider = new ReservoirsDataProvider();
            PeriodicReservoirsDataProvider = new PeriodicReservoirsDataProvider();
            PeriodicCompaniesDataProvider = new PeriodicCompaniesDataProvider();
            ScenariosDataProvider = new ScenariosDataProvider();
            PeriodicHydroPlantsDataProvider = new PeriodicHydroPlantsDataProvider();
            NonConventionalPlantsDataProvider = new NonConventionalPlantsDataProvider();
            NonConventionalPlantBlocksDataProvider = new NonConventionalPlantBlocksDataProvider();
            PeriodicNonConventionalPlantsDataProvider = new PeriodicNonConventionalPlantsDataProvider();
            PeriodicThermalPlantsDataProvider = new PeriodicThermalPlantsDataProvider();
            HydroSystemsDataProvider = new HydroSystemsDataProvider();
            PeriodicHydroSystemsDataProvider = new PeriodicHydroSystemsDataProvider();
            ReservoirsMappingDataProvider = new ReservoirsMappingDataProvider();
            HydroPlantsMappingDataProvider = new HydroPlantsMappingDataProvider();
            ThermalPlantsMappingDataProvider = new ThermalPlantsMappingDataProvider();
            RiversMappingDataProvider = new RiversMappingDataProvider();
            VariableHydroPlantsDataProvider = new VariableHydroPlantsDataProvider();
            VariableThermalPlantsDataProvider = new VariableThermalPlantsDataProvider();
            PFEquationsDataProvider = new PFEquationsDataProvider();
            ExcludingPlantsDataProvider = new ExcludingPlantsDataProvider();
            PeriodicZonesDataProvider = new PeriodicZonesDataProvider();

            //Tablas de Despacho
            PeriodicBarraDataProvider = new PeriodicBarraDataProvider();
        }

        private void CreateHydroElementsTypes()
        {
            HydroElementsTypes = new List<string>()
            {
                "Rio",
                "Embalse",
                "RecursoHidro",
                "ElementoHidro"
            };
        }

        private void SetPeriodsDate()
        {
            DHOGDataBaseViewModel.PeriodsDate = PeriodsDataAccess.GetPeriodsDate();
            DHOGCaseViewModel.SetInitialDate();
        }

        private void CreateCollections()
        {
            CreateCompaniesCollection();
            CreateHydroPlantsCollection();
            CreateThermalPlantsCollection();
            CreateNonConventionalPlantsTypes();
            CreateNonConventionalPlantsCollection();
            CreatePlantsCollectionScenario1();
            CreateZonesCollection();
            CreateEspecialZonesCollection();
            CreateZoneTypes();
            CreateHydroElementsTypes();
            CreateAreasCollection();
            CreateBlocksCollection();
            CreatePeriodsCollection();
            CreatePeriodicInflowsCollection();
            CreatePeriodicAreasCollection();
            CreatePeriodicBlocksCollection();
            CreateFuelsCollection();
            CreatePeriodicFuelsCollection();
            CreateFuelContractsCollection();

            CreateRecursoFuelContractsCollection();

            CreatePeriodicFuelContractsCollection();
            CreatePeriodicLoadBlocksCollection();
            CreateHydroElementsCollection();
            CreatePeriodicHydroElementsCollection();
            CreateReservoirsCollection();
            CreatePeriodicReservoirsCollection();
            CreatePeriodicCompaniesCollection();
            CreateActiveScenarios();
            CreateScenariosCollection();
            CreatePeriodicHydroPlantsCollection();
            CreateNonConventionalPlantBlocksCollection();
            CreatePeriodicNonConventionalPlantsCollection();
            CreatePeriodicThermalPlantsCollection();
            CreateHydroSystemsCollection();
            CreatePeriodicHydroSystemsCollection();
            CreateRiversMapping();
            CreateVariableHydroPlantsCollection();
            CreateVariableThermalPlantsCollection();
            CreatePFEquationsCollection();
            CreateExcludingPlantsCollection();
            CreatePeriodicZonesCollection();

            // tablas del despacho economico
            CreatePeriodicBarraCollection();
        }

        private void CreateCompaniesCollection()
        {
            CompaniesCollection = CompaniesDataProvider.GetObjects();
            CompaniesCollection.ItemEndEdit += CompaniesCollection_ItemEndEdit;
            CompaniesCollection.CollectionChanged += CompaniesCollection_CollectionChanged;
            RaisePropertyChanged("CompaniesCollection");

            CreateCompaniesScenario1();
        }

        private void CreateHydroPlantsCollection()
        {
            HydroPlantsCollection = HydroPlantsDataProvider.GetObjects();
            HydroPlantsCollection.CollectionChanged += HydroPlantsCollection_CollectionChanged;
            HydroPlantsCollection.ItemEndEdit += HydroPlantsCollection_ItemEndEdit;
            RaisePropertyChanged("HydroPlantsCollection");
        }

        private void CreateThermalPlantsCollection()
        {
            ThermalPlantsCollection = ThermalPlantsDataProvider.GetObjects();
            ThermalPlantsCollection.CollectionChanged += ThermalPlantsCollection_CollectionChanged;
            ThermalPlantsCollection.ItemEndEdit += ThermalPlantsCollection_ItemEndEdit;
            RaisePropertyChanged("ThermalPlantsCollection");
        }

        private void CreateNonConventionalPlantsCollection()
        {
            NonConventionalPlantsCollection = NonConventionalPlantsDataProvider.GetObjects();
            NonConventionalPlantsCollection.CollectionChanged += NonConventionalPlantsCollection_CollectionChanged;
            NonConventionalPlantsCollection.ItemEndEdit += NonConventionalPlantsCollection_ItemEndEdit;
            RaisePropertyChanged("NonConventionalPlantsCollection");

            CreateNonConventionalPlantsScenario1();
        }

        private void CreateNonConventionalPlantBlocksCollection()
        {
            NonConventionalPlantBlocksCollection = NonConventionalPlantBlocksDataProvider.GetObjects();
            RaisePropertyChanged("NonConventionalPlantBlocksCollection");
        }

        private void CreateFuelsCollection()
        {
            FuelsCollection = FuelsDataProvider.GetObjects();
            RaisePropertyChanged("FuelsCollection");
        }

        private void CreateFuelContractsCollection()
        {
            FuelContractsCollection = FuelContractsDataProvider.GetObjects();
            RaisePropertyChanged("FuelContractsCollection");
        }

        private void CreateRecursoFuelContractsCollection()
        {
            RecursoFuelContractsCollection = RecursoFuelContractsDataProvider.GetObjects();
            RaisePropertyChanged("RecursoFuelContractsCollection");
        }

        private void CreateHydroElementsCollection()
        {
            HydroElementsCollection = HydroElementsDataProvider.GetObjects();
            HydroElementsCollection.CollectionChanged += HydroElementsCollection_CollectionChanged;
            HydroElementsCollection.ItemEndEdit += HydroElementsCollection_ItemEndEdit;
            RaisePropertyChanged("HydroElementsCollection");

            CreateHydroElementsScenario1();
        }

        private void CreateHydroSystemsCollection()
        {
            HydroSystemsCollection = HydroSystemsDataProvider.GetObjects();
            RaisePropertyChanged("HydroSystemsCollection");
        }

        private void CreateReservoirsCollection()
        {
            ReservoirsCollection = ReservoirsDataProvider.GetObjects();
            ReservoirsCollection.ItemEndEdit += ReservoirsCollection_ItemEndEdit;
            ReservoirsCollection.CollectionChanged += ReservoirsCollection_CollectionChanged;
            RaisePropertyChanged("ReservoirsCollection");

            CreateReservoirsScenario1();
        }

        private void ReservoirsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    ReservoirViewModel removedObject = item as ReservoirViewModel;
                    if (removedObject.Case == 1)
                    {
                        ReservoirsScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("ReservoirsScenario1");

                        ReservoirsMapping.RemoveItem(removedObject.Name);
                        RaisePropertyChanged("ReservoirsMapping");

                        reservoirsScenario1 = ReservoirsScenario1.ToList();
                    }
                }
            }
        }

        private void ReservoirsCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            ReservoirViewModel editedObject = sender as ReservoirViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals("") && editedObject.Case == 1)
            {
                foreach (string reservoirName in ReservoirsScenario1)
                    if (reservoirName.Equals(editedObject.Name))
                        return;

                ReservoirsScenario1.Add(editedObject.Name);
                RaisePropertyChanged("ReservoirsScenario1");

                ReservoirsMapping.Add(new NameMappingViewModel(new NameMapping(editedObject.Name, "")));
                RaisePropertyChanged("ReservoirsMapping");

                reservoirsScenario1 = ReservoirsScenario1.ToList();
            }
        }

        private void CreateActiveScenarios()
        {
            ActiveScenarios = new ObservableCollection<int>();
        }

        private void CreateScenariosCollection()
        {
           ScenariosCollection = ScenariosDataProvider.GetObjects();
            RaisePropertyChanged("ScenariosCollection");
            
           // UpdateActiveScenarios();
        }


        private void CreateScenariosActivosCollection()
        {
            ScenariosActivosCollection = ScenariosDataProvider.GetObjectsScenariosActivos(); 
            RaisePropertyChanged("ScenariosActivosCollection");

            UpdateActiveScenarios();
        }

        private void UpdateActiveScenarios()
        {
            ActiveScenarios.Clear();
       
            int activeScenarios = ScenariosActivosCollection.GetActiveScenariosQuantity1();
            for (int i = 1; i <= activeScenarios; i++)
                ActiveScenarios.Add(i);

            RaisePropertyChanged("ActiveScenarios");
        }

        private void CreateZonesCollection()
        {
            ZonesCollection = ZonesDataProvider.GetObjects();
            RaisePropertyChanged("ZonesCollection");
        }

        private void CreateEspecialZonesCollection()
        {
            EspecialZonesCollection = EspecialZonesDataProvider.GetObjects();
            RaisePropertyChanged("EspecialZonesCollection");
        }


        private void CreateAreasCollection()
        {
            AreasCollection = AreasDataProvider.GetObjects();
            RaisePropertyChanged("AreasCollection");
        }

        private void CreateBlocksCollection()
        {
            BlocksCollection = BlocksDataProvider.GetObjects();
            RaisePropertyChanged("BlocksCollection");
        }

        private void CreatePeriodsCollection()
        {
            PeriodsCollection = PeriodsDataProvider.GetObjects();

            PeriodsCollection.CollectionChanged += PeriodsCollection_CollectionChanged;

            ThermalPlantsCollection.CollectionChanged += ThermalPlantsCollection_CollectionChanged;
            ThermalPlantsCollection.ItemEndEdit += ThermalPlantsCollection_ItemEndEdit;

            RaisePropertyChanged("PeriodsCollection");
        }

        private void CreatePeriodicInflowsCollection()
        {
            PeriodicInflowsCollection = PeriodicInflowsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicInflowsCollection");
        }

        private void CreatePeriodicAreasCollection()
        {
            PeriodicAreasCollection = PeriodicAreasDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicAreasCollection");
        }

        private void CreatePeriodicBlocksCollection()
        {
            PeriodicBlocksCollection = PeriodicBlocksDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicBlocksCollection");
        }

        private void CreatePeriodicBarraCollection()
        {
            PeriodicBarraCollection = PeriodicBarraDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicBarraCollection");
        }

        private void CreatePeriodicLoadBlocksCollection()
        {
            PeriodicLoadBlocksCollection = PeriodicLoadBlocksDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicLoadBlocksCollection");
        }

        private void CreatePeriodicHydroElementsCollection()
        {
            PeriodicHydroElementsCollection = PeriodicHydroElementsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicHydroElementsCollection");
        }

        private void CreatePeriodicHydroSystemsCollection()
        {
            PeriodicHydroSystemsCollection = PeriodicHydroSystemsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicHydroSystemsCollection");
        }

        private void CreatePeriodicZonesCollection()
        {
            PeriodicZonesCollection = PeriodicZonesDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicZonesCollection");
        }

        private void CreatePeriodicHydroPlantsCollection()
        {
            PeriodicHydroPlantsCollection = PeriodicHydroPlantsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicHydroPlantsCollection");
        }

        private void CreatePeriodicThermalPlantsCollection()
        {
            PeriodicThermalPlantsCollection = PeriodicThermalPlantsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicThermalPlantsCollection");
        }

        private void CreatePeriodicNonConventionalPlantsCollection()
        {
            PeriodicNonConventionalPlantsCollection = PeriodicNonConventionalPlantsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicNonConventionalPlantsCollection");
        }

        private void CreatePeriodicFuelsCollection()
        {
            PeriodicFuelsCollection = PeriodicFuelsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicFuelsCollection");
        }

        private void CreatePeriodicFuelContractsCollection()
        {
            PeriodicFuelContractsCollection = PeriodicFuelContractsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicFuelContractsCollection");
        }

        private void CreatePeriodicReservoirsCollection()
        {
            PeriodicReservoirsCollection = PeriodicReservoirsDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicReservoirsCollection");
        }

        private void CreatePeriodicCompaniesCollection()
        {
            PeriodicCompaniesCollection = PeriodicCompaniesDataProvider.GetObjects();
            RaisePropertyChanged("PeriodicCompaniesCollection");
        }

        private void CreateZoneTypes()
        {
            ZoneTypes = new List<string>
            {
                "MX",
                "MN"
            };
        }

        private void CreateNonConventionalPlantsTypes()
        {
            NonConventionalPlantsTypes = new List<string>
            {
                "B",
                "E", 
                "M", 
                "S"
            };
        }

        private void CreateCompaniesScenario1()
        {
            CompaniesScenario1 = new ObservableCollection<string>();

            foreach (CompanyViewModel UIObject in CompaniesCollection)
                if (UIObject.Name != null && UIObject.Case == 1)
                    CompaniesScenario1.Add(UIObject.Name);
            
            RaisePropertyChanged("CompaniesScenario1");
        }

        private void CreateNonConventionalPlantsScenario1()
        {
            NonConventionalPlantsScenario1 = new ObservableCollection<string>();

            foreach (NonConventionalPlantViewModel UIObject in NonConventionalPlantsCollection)
                if (UIObject.Name != null && UIObject.Case == 1)
                    NonConventionalPlantsScenario1.Add(UIObject.Name);

            RaisePropertyChanged("NonConventionalPlantsScenario1");
        }

        private void CompaniesCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    CompanyViewModel removedObject= item as CompanyViewModel;
                    if (removedObject.Case == 1)
                        CompaniesScenario1.Remove(removedObject.Name);
                }
            }
        }

        private void CompaniesCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            CompanyViewModel editedObject = sender as CompanyViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals("") && editedObject.Case == 1)
            {
                foreach (string companyName in CompaniesScenario1)
                    if (companyName.Equals(editedObject.Name))
                        return;

                CompaniesScenario1.Add(editedObject.Name);
            }
        }

        private void CreateHydroElementsScenario1()
        {
            HydroElementsScenario1 = new List<string>();

            foreach (HydroElementViewModel UIObject in HydroElementsCollection)
                HydroElementsScenario1.Add(UIObject.Name);
        }

        private void CreateRiversScenario1()
        {
            RiversScenario1 = new List<string>();

            foreach (NameMappingViewModel UIObject in RiversMapping)
                RiversScenario1.Add(UIObject.DHOGName);
        }

        private void CreatePlantsCollectionScenario1()
        {
            PlantsCollectionScenario1 = new ObservableCollection<string>();

            HydroPlantsScenario1 = new ObservableCollection<string>();
            foreach (HydroPlantViewModel UIObject in HydroPlantsCollection)
            { 
                if (UIObject.Name != null && UIObject.Case == 1)
                {
                    PlantsCollectionScenario1.Add(UIObject.Name);
                    HydroPlantsScenario1.Add(UIObject.Name);
                }
            }
            hydroPlantsScenario1 = HydroPlantsScenario1.ToList();
            RaisePropertyChanged("HydroPlantsScenario1");
            CreateHydroPlantsMapping();

            ThermalPlantsScenario1 = new ObservableCollection<string>();
            foreach (ThermalPlantViewModel UIObject in ThermalPlantsCollection)
            {
                if (UIObject.Name != null && UIObject.Case == 1)
                {
                    PlantsCollectionScenario1.Add(UIObject.Name);
                    ThermalPlantsScenario1.Add(UIObject.Name);
                }
            }
            RaisePropertyChanged("ThermalPlantsScenario1");
            CreateThermalPlantsMapping();

            foreach (NonConventionalPlantViewModel UIObject in NonConventionalPlantsCollection) 
                if (UIObject.Case == 1)
                    if (UIObject.Name != null)
                        PlantsCollectionScenario1.Add(UIObject.Name);
            RaisePropertyChanged("PlantsCollectionScenario1");
        }


        

        private void CreateReservoirsScenario1()
        {
            ReservoirsScenario1 = new ObservableCollection<string>();

            foreach (ReservoirViewModel UIObject in ReservoirsCollection)
                if (UIObject.Name != null && UIObject.Case == 1)
                        ReservoirsScenario1.Add(UIObject.Name);

            reservoirsScenario1 = ReservoirsScenario1.ToList();

            CreateReservoirsMapping();

            RaisePropertyChanged("ReservoirsScenario1");
        }

        private void CreateReservoirsMapping()
        {
            ReservoirsMapping = ReservoirsMappingDataProvider.GetObjects(ReservoirsScenario1.ToList());
            RaisePropertyChanged("ReservoirsMapping");
        }

        private void CreateHydroPlantsMapping()
        {
            HydroPlantsMapping = HydroPlantsMappingDataProvider.GetObjects(HydroPlantsScenario1.ToList());
            RaisePropertyChanged("HydroPlantsMapping");
        }

        private void CreateThermalPlantsMapping()
        {
            ThermalPlantsMapping = ThermalPlantsMappingDataProvider.GetObjects(ThermalPlantsScenario1.ToList());
            RaisePropertyChanged("ThermalPlantsMapping");
        }

        private void CreateRiversMapping()
        {
            RiversMapping = RiversMappingDataProvider.GetObjects();
            RaisePropertyChanged("RiversMapping");

            CreateRiversScenario1();
        }

        private void CreateVariableHydroPlantsCollection()
        {
            VariableHydroPlantsCollection = VariableHydroPlantsDataProvider.GetObjects();
            RaisePropertyChanged("VariableHydroPlantsCollection");
        }

        private void CreateVariableThermalPlantsCollection()
        {
            VariableThermalPlantsCollection = VariableThermalPlantsDataProvider.GetObjects();
            RaisePropertyChanged("VariableThermalPlantsCollection");
        }

        private void CreatePFEquationsCollection()
        {
            PFEquationsCollection = PFEquationsDataProvider.GetObjects();
            RaisePropertyChanged("PFEquationsCollection");
        }

        private void CreateExcludingPlantsCollection()
        {
            ExcludingPlantsCollection = ExcludingPlantsDataProvider.GetObjects();
            RaisePropertyChanged("ExcludingPlantsCollection");
        }


        private void PeriodsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (object item in e.OldItems)
                    {
                        PeriodsCollectionViewModel removedObject = item as PeriodsCollectionViewModel;
                        //PeriodsCollection.Remove();
                    }
                }



               
            }

        }

        private void ThermalPlantsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    ThermalPlantViewModel removedObject = item as ThermalPlantViewModel;
                    if (removedObject.Case == 1)
                    {
                        ThermalPlantsScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("ThermalPlantsScenario1");

                        ThermalPlantsMapping.RemoveItem(removedObject.Name);
                        RaisePropertyChanged("ThermalPlantsMapping");

                        PlantsCollectionScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("PlantsCollectionScenario1");
                    }
                }
            }
        }

        private void ThermalPlantsCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            ThermalPlantViewModel editedObject = sender as ThermalPlantViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals("") && editedObject.Case == 1) {
                foreach (string plantName in ThermalPlantsScenario1)
                    if (plantName.Equals(editedObject.Name))
                        return;

                ThermalPlantsScenario1.Add(editedObject.Name);
                RaisePropertyChanged("ThermalPlantsScenario1");

                ThermalPlantsMapping.Add(new NameMappingViewModel(new NameMapping(editedObject.Name, "")));
                RaisePropertyChanged("ThermalPlantsMapping");

                PlantsCollectionScenario1.Add(editedObject.Name);
                RaisePropertyChanged("PlantsCollectionScenario1");
            }
        }

        private void HydroPlantsCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            HydroPlantViewModel editedObject = sender as HydroPlantViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals("") && editedObject.Case == 1)
            {
                foreach (string plantName in HydroPlantsScenario1)
                    if (plantName.Equals(editedObject.Name))
                        return;

                HydroPlantsScenario1.Add(editedObject.Name);
                hydroPlantsScenario1 = HydroPlantsScenario1.ToList();
                RaisePropertyChanged("HydroPlantsScenario1");

                HydroPlantsMapping.Add(new NameMappingViewModel(new NameMapping(editedObject.Name, "")));
                RaisePropertyChanged("HydroPlantsMapping");

                PlantsCollectionScenario1.Add(editedObject.Name);
                RaisePropertyChanged("PlantsCollectionScenario1");
            }
        }

        private void HydroPlantsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    HydroPlantViewModel removedObject = item as HydroPlantViewModel;
                    if (removedObject.Case == 1)
                    {
                        HydroPlantsScenario1.Remove(removedObject.Name);
                        hydroPlantsScenario1 = HydroPlantsScenario1.ToList();
                        RaisePropertyChanged("HydroPlantsScenario1");

                        HydroPlantsMapping.RemoveItem(removedObject.Name);
                        RaisePropertyChanged("HydroPlantsMapping");

                        PlantsCollectionScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("PlantsCollectionScenario1");
                    }
                }
            }
        }

        private void HydroElementsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    HydroElementViewModel removedObject = item as HydroElementViewModel;
                    HydroElementsScenario1.Remove(removedObject.Name);
                }
            }
        }

        private void HydroElementsCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            HydroElementViewModel editedObject = sender as HydroElementViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals(""))
            {
                foreach (string elementName in HydroElementsScenario1)
                    if (elementName.Equals(editedObject.Name))
                        return;

                HydroElementsScenario1.Add(editedObject.Name);
            }
        }

        private void NonConventionalPlantsCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            NonConventionalPlantViewModel editedObject = sender as NonConventionalPlantViewModel;
            if (editedObject.Name != null && !editedObject.Name.Equals("") && editedObject.Case == 1)
            {
                foreach (string plantName in NonConventionalPlantsScenario1)
                    if (plantName.Equals(editedObject.Name))
                        return;

                NonConventionalPlantsScenario1.Add(editedObject.Name);
                RaisePropertyChanged("NonConventionalPlantsScenario1");
                
                PlantsCollectionScenario1.Add(editedObject.Name);
                RaisePropertyChanged("PlantsCollectionScenario1");
            }
        }

        private void NonConventionalPlantsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NonConventionalPlantViewModel removedObject = item as NonConventionalPlantViewModel;
                    if (removedObject.Case == 1)
                    {
                        PlantsCollectionScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("PlantsCollectionScenario1");

                        NonConventionalPlantsScenario1.Remove(removedObject.Name);
                        RaisePropertyChanged("NonConventionalPlantsScenario1");
                    }

                }
            }
        }

        private void SetPeriodsDateInPeriodicCollections()
        {
            PeriodicInflowsCollection.SetPeriodsDate();
            PeriodicAreasCollection.SetPeriodsDate();
            PeriodicBlocksCollection.SetPeriodsDate();
            PeriodicFuelsCollection.SetPeriodsDate();
            PeriodicFuelContractsCollection.SetPeriodsDate();
            PeriodicLoadBlocksCollection.SetPeriodsDate();
            PeriodicHydroElementsCollection.SetPeriodsDate();
            PeriodicReservoirsCollection.SetPeriodsDate();
            PeriodicCompaniesCollection.SetPeriodsDate();
            PeriodicHydroPlantsCollection.SetPeriodsDate();
            PeriodicNonConventionalPlantsCollection.SetPeriodsDate();
            PeriodicThermalPlantsCollection.SetPeriodsDate();
            PeriodicHydroSystemsCollection.SetPeriodsDate();
            PeriodicZonesCollection.SetPeriodsDate();
        }

        public void UpdateCollections(List<EntityType> entityTypes)
        {
            foreach(EntityType entityType in entityTypes)
            {
                switch (entityType)
                {
                    case EntityType.Company:
                        CreateCompaniesCollection();                        
                        break;
                    case EntityType.HydroPlant:
                        CreateHydroPlantsCollection();
                        CreatePlantsCollectionScenario1();
                        break;
                    case EntityType.ThermalPlant:
                        CreateThermalPlantsCollection();
                        CreatePlantsCollectionScenario1();
                        break;
                    case EntityType.PeriodicInflow:
                        CreatePeriodicInflowsCollection();
                        PeriodicInflowsCollection.SetPeriodsDate();
                        break;
                    case EntityType.Zone:
                        CreateZonesCollection();
                        ZonesCollectionImported.Invoke();
                        break;
                    case EntityType.Area:
                        CreateAreasCollection();
                        break;
                    case EntityType.Block:
                        CreateBlocksCollection();
                        break;
                    case EntityType.Period:
                        CreatePeriodsCollection();
                        SetPeriodsDate();
                        SetPeriodsDateInPeriodicCollections();
                        BasicPeriodsCollectionImported.Invoke();
                        break;
                    case EntityType.PeriodicArea:
                        CreatePeriodicAreasCollection();
                        PeriodicAreasCollection.SetPeriodsDate();
                        break;
                    case EntityType.PeriodicBlock:
                        CreatePeriodicBlocksCollection();
                        PeriodicBlocksCollection.SetPeriodsDate();
                        break;
                    case EntityType.Fuel:
                        CreateFuelsCollection();
                        break;
                    case EntityType.PeriodicFuel:
                        CreatePeriodicFuelsCollection();
                        PeriodicFuelsCollection.SetPeriodsDate();
                        break;
                    case EntityType.FuelContract:
                        CreateFuelContractsCollection();
                        break;
                    case EntityType.RecursoFuelContract:
                        CreateRecursoFuelContractsCollection();
                        break;

                    case EntityType.PeriodicFuelContract:
                        CreatePeriodicFuelContractsCollection();
                        PeriodicFuelContractsCollection.SetPeriodsDate();
                        break;
                    case EntityType.PeriodicLoadBlock:
                        CreatePeriodicLoadBlocksCollection();
                        PeriodicLoadBlocksCollection.SetPeriodsDate();
                        break;
                    case EntityType.HydroElement:
                        CreateHydroElementsCollection();
                        break;
                    case EntityType.PeriodicHydroElement:
                        CreatePeriodicHydroElementsCollection();
                        PeriodicHydroElementsCollection.SetPeriodsDate();
                        break;
                    case EntityType.Reservoir:
                        CreateReservoirsCollection();
                        break;
                    case EntityType.PeriodicReservoir:
                        CreatePeriodicReservoirsCollection();
                        PeriodicReservoirsCollection.SetPeriodsDate();
                        break;
                    case EntityType.PeriodicCompany:
                        CreatePeriodicCompaniesCollection();
                        PeriodicCompaniesCollection.SetPeriodsDate();
                        break;
                    case EntityType.Scenario:
                        //ScenariosActivosCollection
                        CreateScenariosCollection();
                        CreateScenariosActivosCollection();
                        break;
                    case EntityType.PeriodicHydroPlant:
                        CreatePeriodicHydroPlantsCollection();
                        PeriodicHydroPlantsCollection.SetPeriodsDate();
                        break;
                    case EntityType.NonConventionalPlant:
                        CreateNonConventionalPlantsCollection();
                        CreatePlantsCollectionScenario1();
                        CreateNonConventionalPlantsScenario1();
                        break;
                    case EntityType.NonConventionalPlantBlock:
                        CreateNonConventionalPlantBlocksCollection();
                        break;
                    case EntityType.PeriodicNonConventionalPlant:
                        CreatePeriodicNonConventionalPlantsCollection();
                        PeriodicNonConventionalPlantsCollection.SetPeriodsDate();
                        break;
                    case EntityType.PeriodicThermalPlant:
                        CreatePeriodicThermalPlantsCollection();
                        PeriodicThermalPlantsCollection.SetPeriodsDate();
                        break;
                    case EntityType.HydroSystem:
                        CreateHydroSystemsCollection();
                        break;
                    case EntityType.PeriodicHydroSystem:
                        CreatePeriodicHydroSystemsCollection();
                        PeriodicHydroSystemsCollection.SetPeriodsDate();
                        break;
                    case EntityType.VariableHydroPlant:
                        CreateVariableHydroPlantsCollection();
                        break;
                    case EntityType.VariableThermalPlant:
                        CreateVariableThermalPlantsCollection();
                        break;
                    case EntityType.PFEquation:
                        CreatePFEquationsCollection();
                        break;
                    case EntityType.ExcludingPlants:
                        CreateExcludingPlantsCollection();
                        break;
                    case EntityType.PeriodicZone:
                        CreatePeriodicZonesCollection();
                        PeriodicZonesCollection.SetPeriodsDate();
                        break;
                }
            }
        }

        public static List<string> GetReservoirsScenario1()
        {
            return reservoirsScenario1;
        }

        public static List<string> GetHydroPlantsScenario1()
        {
            return hydroPlantsScenario1;
        }

        public static List<string> GetHydroElementsScenario1()
        {
            return HydroElementsScenario1;
        }

        public static List<string> GetRiversScenario1()
        {
            return RiversScenario1;
        }

        public event ZonesCollectionImportedHandler ZonesCollectionImported;
        public event BasicPeriodsCollectionImportedHandler BasicPeriodsCollectionImported;
    }
}
