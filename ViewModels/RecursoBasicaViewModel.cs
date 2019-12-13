using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoBasicaCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoBasicaViewModel: BaseViewModel
    {
        RecursoBasica RecursoBasica;

        public RecursoBasicaViewModel(RecursoBasica RecursoBasica) : base()
        {
            this.RecursoBasica = RecursoBasica;
        }

        public RecursoBasica GetDataObject()
        {
            return RecursoBasica;
        }

        

        public int TML
        {
            get
            {
                return RecursoBasica.TML;
            }
            set
            {
                RecursoBasica.TML = value;
                RaisePropertyChanged("TML");
            }
        }

        public double TMFL
        {
            get
            {
                return RecursoBasica.TMFL;
            }
            set
            {
                RecursoBasica.TMFL = value;
                RaisePropertyChanged("TMFL");
            }
        }


        public double GenIni
        {
            get
            {
                return RecursoBasica.GenIni;
            }
            set
            {
                RecursoBasica.GenIni = value;
                RaisePropertyChanged("GenIni");
            }
        }


        public double TlIni
        {
            get
            {
                return RecursoBasica.TlIni;
            }
            set
            {
                RecursoBasica.TlIni = value;
                RaisePropertyChanged("TlIni");
            }
        }


        public double TflIni
        {
            get
            {
                return RecursoBasica.TflIni;
            }
            set
            {
                RecursoBasica.TflIni = value;
                RaisePropertyChanged("TflIni");
            }
        }

        public string Nombre
        {
            get
            {
                return RecursoBasica.Nombre;
            }
            set
            {
                RecursoBasica.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


        public double Configuracion
        {
            get
            {
                return RecursoBasica.Configuracion;
            }
            set
            {
                RecursoBasica.Configuracion = value;
                RaisePropertyChanged("Configuracion");
            }
        }


        public double Maximoarranques
        {
            get
            {
                return RecursoBasica.Maximoarranques;
            }
            set
            {
                RecursoBasica.Maximoarranques = value;
                RaisePropertyChanged("Maximoarranques");
            }
        }

        public double Modelaintervalos
        {
            get
            {
                return RecursoBasica.Modelaintervalos;
            }
            set
            {
                RecursoBasica.Modelaintervalos = value;
                RaisePropertyChanged("Modelaintervalos");
            }
        }

        public double CostoArranque
        {
            get
            {
                return RecursoBasica.CostoArranque;
            }
            set
            {
                RecursoBasica.CostoArranque = value;
                RaisePropertyChanged("CostoArranque");
            }
        }



    }
}
