﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class CplexParameter
    {
        public CplexParameter(string name, double value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }

        public string Name { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}
