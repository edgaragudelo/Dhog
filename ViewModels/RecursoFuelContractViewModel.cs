using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class RecursoFuelContractsCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class RecursoFuelContractViewModel : BaseViewModel
    {
        RecursoFuelContract recursofuelContract;

        /* Constructor needed to add rows in the RadGridView control */
        public RecursoFuelContractViewModel()
        {
            recursofuelContract = new RecursoFuelContract();
            //Recurso = "";
        }

        public RecursoFuelContractViewModel(RecursoFuelContract recursofuelContract)
        {
            this.recursofuelContract = recursofuelContract;
        }

        public RecursoFuelContract GetDataObject()
        {
            return recursofuelContract;
        }

        public int Id
        {
            get
            {
                return recursofuelContract.Id;
            }
            set
            {
                recursofuelContract.Id = value;
            }
        }

        public string Name1
        {
            get
            {
                return recursofuelContract.Name1;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    recursofuelContract.Name1 = value;
                    RaisePropertyChanged("Name1");
                }
            }
        }

        public string Name
        {
            get
            {
                return recursofuelContract.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    recursofuelContract.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }


        public string Recurso
        {
            get
            {
                return recursofuelContract.Recurso;
            }
            set
            {
                if (value == null)
                    recursofuelContract.Recurso = "";
                else
                {
                    if (recursofuelContract.Name != null)
                        recursofuelContract.Recurso = recursofuelContract.Name;
                    else
                        recursofuelContract.Recurso = value;
                }
                    
                
            }
        }

        
    }
}
