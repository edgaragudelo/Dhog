using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoPeriodoCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoPeriodoViewModel: BaseViewModel
    {
        RecursoPeriodo RecursoPeriodo;

        public RecursoPeriodoViewModel(RecursoPeriodo RecursoPeriodo) : base()
        {
            this.RecursoPeriodo = RecursoPeriodo;
        }

        public RecursoPeriodo GetDataObject()
        {
            return RecursoPeriodo;
        }

        

        public double Minimo
        {
            get
            {
                return RecursoPeriodo.Minimo;
            }
            set
            {
                RecursoPeriodo.Minimo = value;
                RaisePropertyChanged("Minimo");
            }
        }

        public double Maximo
        {
            get
            {
                return RecursoPeriodo.Maximo;
            }
            set
            {
                RecursoPeriodo.Maximo = value;
                RaisePropertyChanged("Maximo");
            }
        }


        public double Precio
        {
            get
            {
                return RecursoPeriodo.Precio;
            }
            set
            {
                RecursoPeriodo.Precio = value;
                RaisePropertyChanged("Precio");
            }
        }


        public double Obligatorio
        {
            get
            {
                return RecursoPeriodo.Obligatorio;
            }
            set
            {
                RecursoPeriodo.Obligatorio = value;
                RaisePropertyChanged("Obligatorio");
            }
        }


        public double AGC
        {
            get
            {
                return RecursoPeriodo.AGC;
            }
            set
            {
                RecursoPeriodo.AGC = value;
                RaisePropertyChanged("AGC");
            }
        }

        public string Nombre
        {
            get
            {
                return RecursoPeriodo.Nombre;
            }
            set
            {
                RecursoPeriodo.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


        public string Pruebas
        {
            get
            {
                return RecursoPeriodo.Pruebas;
            }
            set
            {
                RecursoPeriodo.Pruebas = value;
                RaisePropertyChanged("Pruebas");
            }
        }


        public int Periodo
        {
            get
            {
                return RecursoPeriodo.Periodo;
            }
            set
            {
                RecursoPeriodo.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
