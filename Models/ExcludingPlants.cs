using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class ExcludingPlants
    {
        public ExcludingPlants() { }

        public ExcludingPlants(string plant1, string plant2)
        {
            Plant1 = plant1;
            Plant2 = plant2;
        }

        public string Plant1 { get; set; }
        public string Plant2 { get; set; }
    }
}
