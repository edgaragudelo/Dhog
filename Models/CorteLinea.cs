
namespace DHOG_WPF.Models
{
    public class CorteLinea : BasicEntity
    {
        public CorteLinea(string nombre, string linea, int sentido)
        {
            Nombre = nombre;
            Linea = linea;
            Sentido = sentido;
           
        }

        public string Nombre { get; set; }
        public string Linea { get; set; }      
        public int Sentido { get; set; }
       
    }
}
