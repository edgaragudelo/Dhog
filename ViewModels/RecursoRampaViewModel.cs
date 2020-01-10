using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoRampaCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoRampaViewModel: BaseViewModel
    {
        RecursoRampa RecursoRampa;

        public RecursoRampaViewModel(RecursoRampa RecursoRampa) : base()
        {
            this.RecursoRampa = RecursoRampa;
        }

        public RecursoRampa GetDataObject()
        {
            return RecursoRampa;
        }

        

        public int Configuracion
        {
            get
            {
                return RecursoRampa.Configuracion;
            }
            set
            {
                RecursoRampa.Configuracion = value;
                RaisePropertyChanged("Configuracion");
            }
        }

        public double Tipo
        {
            get
            {
                return RecursoRampa.Tipo;
            }
            set
            {
                RecursoRampa.Tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }


        public double Indice
        {
            get
            {
                return RecursoRampa.Indice;
            }
            set
            {
                RecursoRampa.Indice = value;
                RaisePropertyChanged("Indice");
            }
        }


        public string Modelo
        {
            get
            {
                return RecursoRampa.Modelo;
            }
            set
            {
                RecursoRampa.Modelo = value;
                RaisePropertyChanged("Modelo");
            }
        }


        public double Valor
        {
            get
            {
                return RecursoRampa.Valor;
            }
            set
            {
                RecursoRampa.Valor = value;
                RaisePropertyChanged("Valor");
            }
        }

        public string Nombre
        {
            get
            {
                return RecursoRampa.Nombre;
            }
            set
            {
                RecursoRampa.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


       

    }
}
