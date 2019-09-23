using System.Collections.Generic;

namespace DHOG_WPF.Models
{
    public class EspecialZone : BasicEntity
    {
        public EspecialZone()
        {
            Plants = new List<string>();
        }

        public EspecialZone(string name, double indiceini, double indicefin, int id)
        {
            Name = name;
            IndiceIni = indiceini;
            IndiceFin = indicefin;
            Id = id;
        }

        public double IndiceIni { get; set; }
        public double IndiceFin { get; set; }
        public List<string> Plants { get; set; }
    }
}
