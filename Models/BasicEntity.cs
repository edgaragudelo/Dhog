using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public abstract class BasicEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Case { get; set; }

    }
}
