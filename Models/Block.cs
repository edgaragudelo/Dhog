using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class Block
    {
        /* Constructor needed by the FilesReader module */
        public Block(int name, double durationFactor, double loadFactor)
        {
            Name = name;
            DurationFactor = durationFactor;
            LoadFactor = loadFactor;
        }

        public Block(int name, double durationFactor, double loadFactor, int id)
        {
            Name = name;
            DurationFactor = durationFactor;
            LoadFactor = loadFactor;
            Id = id;
        }

        public Block() { }

        public int Id { get; set; }
        public int Name { get; set; }
        public double DurationFactor { get; set; }
        public double LoadFactor { get; set; }
    }
}
