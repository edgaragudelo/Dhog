
namespace DHOG_WPF.Models
{
    public class RecursoRampa : BasicEntity
    {
        public RecursoRampa(string nombre, int configuracion, double tipo, double indice, string modelo, double valor)
        {
            Nombre = nombre;
            Configuracion = configuracion;
            Tipo = tipo;
            Indice = indice;           
            Modelo =modelo;
            Valor = valor;


        }

        public string Nombre { get; set; }
        public int Configuracion { get; set; }      
        public double Tipo { get; set; }
        public double Indice { get; set; }        
        public string Modelo { get; set; }
        public double Valor { get; set; }



    }
}
