using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class VariableConventionalPlant : BasicEntity
    {
        public VariableConventionalPlant() { }

        public int Segment { get; set; }
        public double Max { get; set; }
        public double ProductionFactor { get; set; }
    }
}
