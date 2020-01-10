using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoFactibleCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoFactibleViewModel: BaseViewModel
    {
        RecursoFactible RecursoFactible;

        public RecursoFactibleViewModel(RecursoFactible RecursoFactible) : base()
        {
            this.RecursoFactible = RecursoFactible;
        }

        public RecursoFactible GetDataObject()
        {
            return RecursoFactible;
        }

        

        public double Minimo
        {
            get
            {
                return RecursoFactible.Minimo;
            }
            set
            {
                RecursoFactible.Minimo = value;
                RaisePropertyChanged("Minimo");
            }
        }

        public double Maximo
        {
            get
            {
                return RecursoFactible.Maximo;
            }
            set
            {
                RecursoFactible.Maximo = value;
                RaisePropertyChanged("Maximo");
            }
        }


       

        public string Nombre
        {
            get
            {
                return RecursoFactible.Nombre;
            }
            set
            {
                RecursoFactible.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


       public int indice
        {
            get
            {
                return RecursoFactible.indice;
            }
            set
            {
                RecursoFactible.indice = value;
                RaisePropertyChanged("indice");
            }
        }

    }
}
