using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class RecursoFuelContract : RecursoBasicEntity
    {
        /* Constructor needed by the FilesReader module */
        public RecursoFuelContract(string name1, string name,string recurso)
        {
            Name1 = name1;
            Name = name;
            Recurso = recurso;
        }

        public RecursoFuelContract() { }

        public string Recurso { get; set; }
       

        /* Property needed by the FilesReader module */
     
    }
}
