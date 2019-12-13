using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class UnidadPeriodoCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class UnidadPeriodoViewModel: BaseViewModel
    {
        UnidadPeriodo UnidadPeriodo;

        public UnidadPeriodoViewModel(UnidadPeriodo UnidadPeriodo) : base()
        {
            this.UnidadPeriodo = UnidadPeriodo;
        }

        public UnidadPeriodo GetDataObject()
        {
            return UnidadPeriodo;
        }

        

        public double Minimo
        {
            get
            {
                return UnidadPeriodo.Minimo;
            }
            set
            {
                UnidadPeriodo.Minimo = value;
                RaisePropertyChanged("Minimo");
            }
        }

        public double Maximo
        {
            get
            {
                return UnidadPeriodo.Maximo;
            }
            set
            {
                UnidadPeriodo.Maximo = value;
                RaisePropertyChanged("Maximo");
            }
        }

        public string Nombre
        {
            get
            {
                return UnidadPeriodo.Nombre;
            }
            set
            {
                UnidadPeriodo.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return UnidadPeriodo.Periodo;
            }
            set
            {
                UnidadPeriodo.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
