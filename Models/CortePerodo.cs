
namespace DHOG_WPF.Models
{
    public class CortePeriodo : BasicEntity
    {
        public CortePeriodo(string nombre, int period, double importacion, double exportacion)
        {
            Nombre = nombre;
            Periodo = period;
            Importacion = importacion;
            Exportacion = exportacion;
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }      
        public double Importacion { get; set; }
        public double Exportacion { get; set; }

    }
}
