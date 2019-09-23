using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class NonConventionalPlantBlock : BasicEntity
    {
        public int Block { get; set; }
        public double ReductionFactor { get; set; }
    }
}
