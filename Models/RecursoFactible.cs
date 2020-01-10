
namespace DHOG_WPF.Models
{
    public class RecursoFactible : BasicEntity
    {
        public RecursoFactible(string nombre, int indice, double minimo, double maximo)
        {
            Nombre = nombre;
            indice = indice;
            Minimo = minimo;
            Maximo = maximo;
           


        }

        public string Nombre { get; set; }
        public int indice { get; set; }      
        public double Minimo { get; set; }
        public double Maximo { get; set; }
      



    }
}
