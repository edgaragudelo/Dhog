
namespace DHOG_WPF.Models
{
    public class AreaBarra : BasicEntity
    {
        public AreaBarra(string nombre, int period, double demanda, double limiteImportacion, double limiteExportacion)
        {
            Nombre = nombre;
            Periodo = period;
            Demanda = demanda;
            LimiteImportacion = limiteImportacion;
            LimiteExportacion = limiteExportacion;
        }

        public string Nombre { get; set; }
        public int Periodo { get; set; }

        public double LimiteImportacion { get; set; }
        public double Demanda { get; set; }
        public double LimiteExportacion { get; set; }
    }
}
