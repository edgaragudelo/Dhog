
namespace DHOG_WPF.Models
{
    public class UnidadPeriodo : BasicEntity
    {
        public UnidadPeriodo(string nombre, int period, double minimo, double maximo)
        {
            Nombre = nombre;
            Periodo = period;
            Minimo = minimo;
            Maximo = maximo;
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }      
        public double Minimo { get; set; }
        public double Maximo { get; set; }

    }
}
