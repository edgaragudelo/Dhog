
using DHOG_WPF.DataTypes;
using DHOG_WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DHOG_WPF.ViewModels
{
    public class FilesReadingParametersViewModel : BaseViewModel
    {
        FilesReadingParameters filesReadingParameters;
        private string dbFolderName;
        private string busyContent;
        private double progressValue;

        public FilesReadingParametersViewModel()
        {
            filesReadingParameters = new FilesReadingParameters();
            //InputFilesPath = "E:\\Alejandra\\Documents\\Proyectos\\DHOG\\Pruebas\\20170920";
            ScenarioFolderName = "BD-Caso_";
            InitialScenarioToCreate = 1;
            InitialPeriodDB = 1;
            InflowsReferenceYear = 1999;
            ReadEcuadorLoad = true;
            ControlScenariosCreation = false;
            RiverRangesToOmit = new ObservableCollection<RiversRangeViewModel>()
            {
                new RiversRangeViewModel(300, 500)
            };
        }

        public FilesReadingParameters GetDataObject()
        {
            return filesReadingParameters;
        }

        public string InputFilesPath
        {
            get
            {
                return filesReadingParameters.InputFilesPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) // Error Controlado que la seleccion no este vacia
                {
                    if (value[value.Length - 1] != '\\')
                        filesReadingParameters.InputFilesPath = value + "\\";
                    else
                        filesReadingParameters.InputFilesPath = value;

                    RaisePropertyChanged("InputFilesPath");
                }
                }


        }

        public int InitialPeriodDB
        {
            get
            {
                return filesReadingParameters.InitialPeriodDB;
            }
            set
            {
                filesReadingParameters.InitialPeriodDB = value;
                RaisePropertyChanged("InitialPeriodDB");
            }
        }

        public int InflowsReferenceYear
        {
            get
            {
                return filesReadingParameters.InflowsReferenceYear;
            }
            set
            {
                filesReadingParameters.InflowsReferenceYear = value;
                RaisePropertyChanged("InflowsReferenceYear");
            }
        }

        public bool CreateScenario
        {
            get
            {
                return filesReadingParameters.CreateScenario;
            }
            set
            {
                filesReadingParameters.CreateScenario = value;
                RaisePropertyChanged("CreateScenario");
            }
        }

        public bool ControlScenariosCreation
        {
            get
            {
                return !filesReadingParameters.AutomaticScenarioCreation;
            }
            set
            {
                filesReadingParameters.AutomaticScenarioCreation = !value;
                RaisePropertyChanged("ControlScenariosCreation");
            }
        }

        public string ScenarioFolderName
        {
            get
            {
                return dbFolderName;
            }
            set
            {
                dbFolderName = value;
                filesReadingParameters.ScenarioFolderName = InputFilesPath + value;
                RaisePropertyChanged("ScenarioFolderName");
            }
        }

        public int InitialScenarioToCreate
        {
            get
            {
                return filesReadingParameters.InitialScenarioToCreate;
            }
            set
            {
                filesReadingParameters.InitialScenarioToCreate = value;
                RaisePropertyChanged("InitialScenarioToCreate");
            }
        }

        public bool ReadEcuadorLoad
        {
            get
            {
                return filesReadingParameters.ReadEcuadorLoad;
            }
            set
            {
                filesReadingParameters.ReadEcuadorLoad = value;
                RaisePropertyChanged("ReadEcuadorLoad");
            }
        }

        public ObservableCollection<RiversRangeViewModel> RiverRangesToOmit
        {
            get; set;
        }

        public void SetRiversToOmit()
        {
            filesReadingParameters.RiverRangesToOmit = new List<OpenRange>();
            foreach (RiversRangeViewModel riversRange in RiverRangesToOmit)
            {
                if (riversRange.Min < riversRange.Max)
                    filesReadingParameters.RiverRangesToOmit.Add(new OpenRange(riversRange.Min, riversRange.Max));
            }
        }

        public bool RepeatMaintenances
        {
            get
            {
                return filesReadingParameters.RepeatMaintenances;
            }
            set
            {
                filesReadingParameters.RepeatMaintenances = value;
                RaisePropertyChanged("RepeatMaintenances");
            }
        }

        public List<string> ModelTypes
        {
            get
            {
                List<string> modelTypes = new List<string>()
                {
                    "MP",
                    "LP"
                };

                return modelTypes;
            }
        }

        public string BusyContent
        {
            get
            {
                return busyContent;
            }
            set
            {
                if (busyContent != value)
                {
                    busyContent = value;
                    RaisePropertyChanged("BusyContent");
                }
            }
        }

        public double ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (progressValue != value)
                {
                    progressValue = value;
                    RaisePropertyChanged("ProgressValue");
                }
            }
        }
    }
}
