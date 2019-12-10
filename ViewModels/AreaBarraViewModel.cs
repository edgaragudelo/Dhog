using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class AreaBarraCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class AreaBarraViewModel: BaseViewModel
    {
        AreaBarra AreaBarra;

        public AreaBarraViewModel(AreaBarra AreaBarra) : base()
        {
            this.AreaBarra = AreaBarra;
        }

        public AreaBarra GetDataObject()
        {
            return AreaBarra;
        }

        

        public double Demanda
        {
            get
            {
                return AreaBarra.Demanda;
            }
            set
            {
                AreaBarra.Demanda = value;
                RaisePropertyChanged("Demanda");
            }
        }

        public double LimiteImportacion
        {
            get
            {
                return AreaBarra.LimiteImportacion;
            }
            set
            {
                AreaBarra.LimiteImportacion = value;
                RaisePropertyChanged("LimiteImportacion");
            }
        }

        public double LimiteExportacion
        {
            get
            {
                return AreaBarra.LimiteExportacion;
            }
            set
            {
                AreaBarra.LimiteExportacion = value;
                RaisePropertyChanged("LimiteExportacion");
            }
        }

        public string Nombre
        {
            get
            {
                return AreaBarra.Nombre;
            }
            set
            {
                AreaBarra.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return AreaBarra.Periodo;
            }
            set
            {
                AreaBarra.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
