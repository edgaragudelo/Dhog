using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class LineaPeriodoCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class LineaPeriodoViewModel: BaseViewModel
    {
        LineaPeriodo LineaPeriodo;

        public LineaPeriodoViewModel(LineaPeriodo LineaPeriodo) : base()
        {
            this.LineaPeriodo = LineaPeriodo;
        }

        public LineaPeriodo GetDataObject()
        {
            return LineaPeriodo;
        }

        

        public double Flujomaximo
        {
            get
            {
                return LineaPeriodo.Flujomaximo;
            }
            set
            {
                LineaPeriodo.Flujomaximo = value;
                RaisePropertyChanged("Flujomaximo");
            }
        }

        

        public string Nombre
        {
            get
            {
                return LineaPeriodo.Nombre;
            }
            set
            {
                LineaPeriodo.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return LineaPeriodo.Periodo;
            }
            set
            {
                LineaPeriodo.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
