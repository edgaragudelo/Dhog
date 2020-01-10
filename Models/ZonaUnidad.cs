
namespace DHOG_WPF.Models
{
    public class ZonaUnidad : BasicEntity
    {
        public ZonaUnidad(string nombre, string unidad,int period, double peso)
        {
            Nombre = nombre;
            Periodo = period;
            Peso = peso;
            Unidad = unidad;
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }      
        public double Peso { get; set; }
        public string Unidad { get; set; }

    }
}
