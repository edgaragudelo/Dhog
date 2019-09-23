using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class Company : BasicEntity
    {
        public Company() { }

        public double StockPrice { get; set; }
        public double Contract { get; set; }
        public int IsContractModeled { get; set; }
        public double ContractFactor { get; set; }
        public double ContractPenalizationFactor { get; set; }
    }
}
