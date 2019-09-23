using System.Collections.Generic;

namespace DHOG_WPF.Models
{
    public class Zone : BasicEntity
    {
        public Zone()
        {
            Plants = new List<string>();
        }

        public Zone(string name, string type, double value, int id)
        {
            Name = name;
            Type = type;
            Value = value;
            Id = id;
        }

        public string Type { get; set; }
        public double Value { get; set; }
        public List<string> Plants { get; set; }
    }
}
