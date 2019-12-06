
namespace DHOG_WPF.Models
{
    public class PeriodicBarra : BasicEntity
    {
        public PeriodicBarra(string nombre, int period, double demanda, double maximoAngulo, double costoracionamiento)
        {
            Nombre = nombre;
            Periodo = period;
            Demanda = demanda;
            MaximoAngulo = maximoAngulo;
            Costoracionamiento = costoracionamiento;
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }

        public double MaximoAngulo { get; set; }
        public double Demanda { get; set; }
        public double Costoracionamiento { get; set; }
    }
}
