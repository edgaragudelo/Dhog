
namespace DHOG_WPF.Models
{
    public class LineaPeriodo : BasicEntity
    {
        public LineaPeriodo(string nombre, int period, double flujomaximo)
        {
            Nombre = nombre;
            Periodo = period;
            Flujomaximo = flujomaximo;
           
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }      
        public double Flujomaximo { get; set; }
       
    }
}
