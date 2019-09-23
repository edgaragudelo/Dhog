
namespace DHOG_WPF.Models
{
    public class HydroTopology : BasicEntity
    {
        public HydroTopology() { }

        public HydroTopology(string system, string element, string type, string elementType, int id)
        {
            System = system;
            Element = element;
            Type = type;
            ElementType = elementType;
            Id = id;
        }

        public string System { get; set; }
        public string Element { get; set; }
        public string Type { get; set; }
        public string ElementType { get; set; }
    }
}
