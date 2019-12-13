using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class CorteLineaCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class CorteLineaViewModel: BaseViewModel
    {
        CorteLinea CorteLinea;

        public CorteLineaViewModel(CorteLinea CorteLinea) : base()
        {
            this.CorteLinea = CorteLinea;
        }

        public CorteLinea GetDataObject()
        {
            return CorteLinea;
        }

        

        public string Linea
        {
            get
            {
                return CorteLinea.Linea;
            }
            set
            {
                CorteLinea.Linea = value;
                RaisePropertyChanged("Linea");
            }
        }

        

        public string Nombre
        {
            get
            {
                return CorteLinea.Nombre;
            }
            set
            {
                CorteLinea.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Sentido
        {
            get
            {
                return CorteLinea.Sentido;
            }
            set
            {
                CorteLinea.Sentido = value;
                RaisePropertyChanged("Sentido");
            }
        }

    }
}
