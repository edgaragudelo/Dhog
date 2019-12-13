using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class CortePeriodoCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class CortePeriodoViewModel: BaseViewModel
    {
        CortePeriodo CortePeriodo;

        public CortePeriodoViewModel(CortePeriodo CortePeriodo) : base()
        {
            this.CortePeriodo = CortePeriodo;
        }

        public CortePeriodo GetDataObject()
        {
            return CortePeriodo;
        }

        

        public double Importacion
        {
            get
            {
                return CortePeriodo.Importacion;
            }
            set
            {
                CortePeriodo.Importacion = value;
                RaisePropertyChanged("Importacion");
            }
        }

        public double Exportacion
        {
            get
            {
                return CortePeriodo.Exportacion;
            }
            set
            {
                CortePeriodo.Exportacion = value;
                RaisePropertyChanged("Exportacion");
            }
        }

        public string Nombre
        {
            get
            {
                return CortePeriodo.Nombre;
            }
            set
            {
                CortePeriodo.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return CortePeriodo.Periodo;
            }
            set
            {
                CortePeriodo.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
