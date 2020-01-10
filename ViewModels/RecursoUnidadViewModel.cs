using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoUnidadCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoUnidadViewModel: BaseViewModel
    {
        RecursoUnidad RecursoUnidad;

        public RecursoUnidadViewModel(RecursoUnidad RecursoUnidad) : base()
        {
            this.RecursoUnidad = RecursoUnidad;
        }

        public RecursoUnidad GetDataObject()
        {
            return RecursoUnidad;
        }

               
        public string Unidad
        {
            get
            {
                return RecursoUnidad.Unidad;
            }
            set
            {
                RecursoUnidad.Unidad = value;
                RaisePropertyChanged("Unidad");
            }
        }

               
        public string Nombre
        {
            get
            {
                return RecursoUnidad.Nombre;
            }
            set
            {
                RecursoUnidad.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


        
    

    }
}
