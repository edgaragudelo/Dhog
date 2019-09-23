using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public abstract class Plant : BasicEntity
    {
        public double ProductionFactor { get; set; }
        public double Max { get; set; }
        public string Company { get; set; }
        public int StartPeriod { get; set; }
        public string Subarea { get; set; }
    }
}
