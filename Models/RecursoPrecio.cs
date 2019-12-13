
namespace DHOG_WPF.Models
{
    public class RecursoPrecio : BasicEntity
    {
        public RecursoPrecio(string nombre, double precio)
        {
            Nombre = nombre;          
            Precio = precio;
        }

        public string Nombre { get; set; }
       
        public double Precio { get; set; }

    }
}
