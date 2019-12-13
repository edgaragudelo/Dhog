using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class RecursoPrecioCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class RecursoPrecioViewModel: BaseViewModel
    {
        RecursoPrecio RecursoPrecio;

        public RecursoPrecioViewModel(RecursoPrecio RecursoPrecio) : base()
        {
            this.RecursoPrecio = RecursoPrecio;
        }

        public RecursoPrecio GetDataObject()
        {
            return RecursoPrecio;
        }

               
        public double Precio
        {
            get
            {
                return RecursoPrecio.Precio;
            }
            set
            {
                RecursoPrecio.Precio = value;
                RaisePropertyChanged("Precio");
            }
        }

               
        public string Nombre
        {
            get
            {
                return RecursoPrecio.Nombre;
            }
            set
            {
                RecursoPrecio.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


        
    

    }
}
