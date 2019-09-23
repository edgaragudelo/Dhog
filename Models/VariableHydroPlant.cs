using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class VariableHydroPlant : VariableConventionalPlant
    {
        public VariableHydroPlant() { }

        public string Reservoir { get; set; }
        public double Level { get; set; }
    }
}
