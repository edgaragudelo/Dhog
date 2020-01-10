
namespace DHOG_WPF.Models
{
    public class RecursoUnidad : BasicEntity
    {
        public RecursoUnidad(string nombre, string unidad)
        {
            Nombre = nombre;          
            Unidad = unidad;
        }

        public string Nombre { get; set; }
       
        public string Unidad { get; set; }

    }
}
