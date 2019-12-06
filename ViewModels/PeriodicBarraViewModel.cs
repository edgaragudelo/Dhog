using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicBarraCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class PeriodicBarraViewModel: BaseViewModel
    {
        PeriodicBarra periodicBarra;

        public PeriodicBarraViewModel(PeriodicBarra periodicBarra) : base()
        {
            this.periodicBarra = periodicBarra;
        }

        public PeriodicBarra GetDataObject()
        {
            return periodicBarra;
        }

        

        public double Demanda
        {
            get
            {
                return periodicBarra.Demanda;
            }
            set
            {
                periodicBarra.Demanda = value;
                RaisePropertyChanged("Demanda");
            }
        }

        public double MaximoAngulo
        {
            get
            {
                return periodicBarra.MaximoAngulo;
            }
            set
            {
                periodicBarra.MaximoAngulo = value;
                RaisePropertyChanged("MaximoAngulo");
            }
        }

        public double Costoracionamiento
        {
            get
            {
                return periodicBarra.Costoracionamiento;
            }
            set
            {
                periodicBarra.Costoracionamiento = value;
                RaisePropertyChanged("Costoracionamiento");
            }
        }

        public string Nombre
        {
            get
            {
                return periodicBarra.Nombre;
            }
            set
            {
                periodicBarra.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public int Periodo
        {
            get
            {
                return periodicBarra.Periodo;
            }
            set
            {
                periodicBarra.Periodo = value;
                RaisePropertyChanged("Periodo");
            }
        }

    }
}
