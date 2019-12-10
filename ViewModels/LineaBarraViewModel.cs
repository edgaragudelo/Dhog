using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class LineaBarraCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class LineaBarraViewModel: BaseViewModel
    {
        LineaBarra LineaBarra;

        public LineaBarraViewModel(LineaBarra LineaBarra) : base()
        {
            this.LineaBarra = LineaBarra;
        }

        public LineaBarra GetDataObject()
        {
            return LineaBarra;
        }

        

        public double Flujomaximo
        {
            get
            {
                return LineaBarra.FlujoMaximo;
            }
            set
            {
                LineaBarra.FlujoMaximo = value;
                RaisePropertyChanged("FlujoMaximo");
            }
        }

        public double Reactancia
        {
            get
            {
                return LineaBarra.Reactancia;
            }
            set
            {
                LineaBarra.Reactancia = value;
                RaisePropertyChanged("Reactancia");
            }
        }

        public string Nombre
        {
            get
            {
                return LineaBarra.Nombre;
            }
            set
            {
                LineaBarra.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }

        public string BarraInicial
        {
            get
            {
                return LineaBarra.BarraInicial;
            }
            set
            {
                LineaBarra.BarraInicial = value;
                RaisePropertyChanged("BarraInicial");
            }
        }

        public string BarraFinal
        {
            get
            {
                return LineaBarra.BarraFinal;
            }
            set
            {
                LineaBarra.BarraFinal = value;
                RaisePropertyChanged("BarraFinal");
            }
        }

        public int NMenos1
        {
            get
            {
                return LineaBarra.NMenos1;
            }
            set
            {
                LineaBarra.NMenos1 = value;
                RaisePropertyChanged("NMenos1");
            }
        }


        public int Activa
        {
            get
            {
                return LineaBarra.Activa;
            }
            set
            {
                LineaBarra.Activa = value;
                RaisePropertyChanged("Activa");
            }
        }

        


    }
}
