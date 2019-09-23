using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class PFEquation : BasicEntity
    {
        public PFEquation() { }

        public string Reservoir { get; set; }
        public double Intercept { get; set; }
        public double LinearCoefficient { get; set; }
        public double CuadraticCoefficient { get; set; }
    }
}
