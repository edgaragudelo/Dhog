using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class ZonaUnidadCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class ZonaUnidadViewModel: BaseViewModel
    {
        ZonaUnidad ZonaUnidad;

        public ZonaUnidadViewModel(ZonaUnidad ZonaUnidad) : base()
        {
            this.ZonaUnidad = ZonaUnidad;
        }

        public ZonaUnidad GetDataObject()
        {
            return ZonaUnidad;
        }

        

        public double Peso
        {
            get
            {
                return ZonaUnidad.Peso;
            }
            set
            {
                ZonaUnidad.Peso = value;
                RaisePropertyChanged("Peso");
            }
        }

        public string Unidad
        {
            get
            {
                return ZonaUnidad.Unidad;
            }
            set
            {
                ZonaUnidad.Unidad = value;
                RaisePropertyChanged("Unidad");
            }
        }

        public string Nombre
        {
            get
            {
                return ZonaUnidad.Nombre;
            }
            set
            {
                ZonaUnidad.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return ZonaUnidad.Periodo;
            }
            set
            {
                ZonaUnidad.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
