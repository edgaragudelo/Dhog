using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class ScenariosActivos
    {
        public ScenariosActivos(string variable, int scenariosQuantity, int isActive, int treePeriod)
        {
            Variable = variable;
            CasesQuantity = scenariosQuantity;
            IsActive = isActive;
            TreePeriod = treePeriod;
        }

        public string Variable { get; set; }
        public int CasesQuantity { get; set; }
        public int IsActive { get; set; }
        public int TreePeriod { get; set; }
    }
}
