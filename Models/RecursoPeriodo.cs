
namespace DHOG_WPF.Models
{
    public class RecursoPeriodo : BasicEntity
    {
        public RecursoPeriodo(string nombre, int period, double minimo, double maximo, double precio, double obligatorio, string pruebas, double aGC)
        {
            Nombre = nombre;
            Periodo = period;
            Minimo = minimo;
            Maximo = maximo;
            Precio = precio;
            Obligatorio = obligatorio;
            Pruebas =pruebas;
            AGC = aGC;


        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }      
        public double Minimo { get; set; }
        public double Maximo { get; set; }
        public double Precio { get; set; }
        public double Obligatorio { get; set; }
        public string Pruebas { get; set; }
        public double AGC { get; set; }



    }
}
