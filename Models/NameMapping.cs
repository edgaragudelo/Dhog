using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class NameMapping
    {
        public NameMapping() { }

        public NameMapping(string dhogName, string sddpName)
        {
            DHOGName = dhogName;
            SDDPName = sddpName;
        }

        public NameMapping(string dhogName, int sddpNumber)
        {
            DHOGName = dhogName;
            SDDPNumber = sddpNumber;
        }

        public string DHOGName { get; set; }
        public string SDDPName { get; set; }
        public int SDDPNumber { get; set; }
    }
}
