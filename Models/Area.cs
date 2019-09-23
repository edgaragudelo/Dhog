using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class Area : BasicEntity
    {
        public Area() { }

        public double BaseLoad { get; set; }
        public double ImportationLimit { get; set; }
        public double ExportationLimit { get; set; }
    }
}
