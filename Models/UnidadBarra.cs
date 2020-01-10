
namespace DHOG_WPF.Models
{
    public class UnidadBarra : BasicEntity
    {
        public UnidadBarra(string nombre, string barra)
        {
            Nombre = nombre;          
            Barra = barra;
        }

        public string Nombre { get; set; }
       
        public string Barra { get; set; }

    }
}
