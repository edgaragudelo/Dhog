using DHOG_WPF.Models;
using System;
using System.Collections.Specialized;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class UnidadBarraCollectionViewModel : PeriodicEntityCollectionViewModel
    {
       
    }

    public class UnidadBarraViewModel: BaseViewModel
    {
        UnidadBarra UnidadBarra;

        public UnidadBarraViewModel(UnidadBarra UnidadBarra) : base()
        {
            this.UnidadBarra = UnidadBarra;
        }

        public UnidadBarra GetDataObject()
        {
            return UnidadBarra;
        }

               
        public string Barra
        {
            get
            {
                return UnidadBarra.Barra;
            }
            set
            {
                UnidadBarra.Barra = value;
                RaisePropertyChanged("Barra");
            }
        }

               
        public string Nombre
        {
            get
            {
                return UnidadBarra.Nombre;
            }
            set
            {
                UnidadBarra.Nombre = value;
                RaisePropertyChanged("Nombre");
            }
        }


        
    

    }
}
