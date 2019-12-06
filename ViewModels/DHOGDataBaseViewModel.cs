using DHOG_WPF.Models;
using System.IO;
using DHOG_WPF.DataAccess;
using System.Windows.Media;
using System;
using System.Globalization;

namespace DHOG_WPF.ViewModels
{
    public delegate void CaseScenarioChangedHandler();
    public delegate void SelectedReservoirChangedHandler();
    public delegate void SelectedCompanyChangedHandler();

    public class DHOGDataBaseViewModel : BaseViewModel
    {
        public static string[] PeriodsDate { get; set; }
        DHOGDataBase dhogDataBase;
        private string initialDate;
        private string Demo;
    
        int scenario;
        string selectedReservoir;
        string selectedCompany;
        Brush initialDateColor;

        public DHOGDataBaseViewModel()
        {
            dhogDataBase = new DHOGDataBase();
            InitialDate = null;
            scenario = 1;
            Demo = "Licencia en DEMO";
        }

       



       

               

        public DHOGDataBase GetDataObject()
        {
            return dhogDataBase;
        }

        public string InputDBFile
        {
            get
            {
                return dhogDataBase.InputDBFile;
            }
            set
            {
                dhogDataBase.InputDBFile = value;
                if (File.Exists(dhogDataBase.InputDBFile))
                {
                    DBFolder = Path.GetDirectoryName(value);
                    DHOGDataBaseDataAccess dhogDataBaseAccess = new DHOGDataBaseDataAccess(dhogDataBase.InputDBFile, dhogDataBase.OutputDBFile);
                }
                RaisePropertyChanged("InputDBFile");
            }
        }

        public string OutputDBFile
        {
            get
            {
                return dhogDataBase.OutputDBFile;
            }
        }

        public string TipoDespacho
        {
            get
            {
                return dhogDataBase.TipoDespacho;
            }
            set
            {
                dhogDataBase.TipoDespacho = value;
               
                RaisePropertyChanged("TipoDespacho");
            }
        }

        public string DBFolder { get; set; }
        
        public string Description
        {
            get
            {
                return dhogDataBase.Description;
            }
            set
            {
                dhogDataBase.Description = value;
                if (value != null)
                    DHOGDataBaseDataAccess.UpdateCaseDescription(value);
                RaisePropertyChanged("Description");
            }
        }



        public void UpdateDBVersionAndDescription()
        {
            DHOGDataBaseDataAccess.GetDBInformation(dhogDataBase);
            RaisePropertyChanged("Description");
        }

        public string InitialDate
        {
            get
            {
                if (initialDate == null)
                    return "*** No existe fecha para el primer periodo en la tabla PeriodoBasica ***";
                else
                {
                    string.Format("{0:ddd, MMM d, yyyy}", initialDate);
                    return initialDate;
                }
            }
            set
            {
                initialDate = value;

                if (value == null)
                    InitialDateColor = Brushes.Red;
                else
                {
                    InitialDateColor = Brushes.Black;
                    string.Format("{0:ddd, MMM d, yyyy}", initialDate);
                }
                RaisePropertyChanged("InitialDate");
            }
        }

        public Brush InitialDateColor
        {
            get
            {
                return initialDateColor;
            }
            set
            {
                initialDateColor = value;
                RaisePropertyChanged("InitialDateColor");
            }
        }

        public void SetInitialDate()
        {
            if (PeriodsDate != null)
            {
                InitialDate = PeriodsDate[0];
             
                RaisePropertyChanged("InitialDate");
            }
            else
                InitialDate = null;
        }

        public double Version
        {
            get
            {
                return dhogDataBase.Version;
            }
            set
            {
                dhogDataBase.Version = value;
            }
        }

        public int Scenario
        {
            get
            {
                return scenario;
            }
            set
            {
                scenario = value;
                RaisePropertyChanged("Scenario");
                CaseScenarioChanged.Invoke();
            }
        }

        public string SelectedReservoir
        {
            get
            {
                return selectedReservoir;
            }
            set
            {
                selectedReservoir = value;
                RaisePropertyChanged("SelectedReservoir");
                SelectedReservoirChanged.Invoke();
            }
        }

        public string SelectedCompany
        {
            get
            {
                return selectedCompany;
            }
            set
            {
                selectedCompany = value;
                RaisePropertyChanged("SelectedCompany");
                SelectedCompanyChanged.Invoke();
            }
        }

       

        public event CaseScenarioChangedHandler CaseScenarioChanged;
        public event SelectedReservoirChangedHandler SelectedReservoirChanged;
        public event SelectedCompanyChangedHandler SelectedCompanyChanged;
    }
}
