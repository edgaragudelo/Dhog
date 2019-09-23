
namespace DHOG_WPF.Models
{
    public class RutasDhogParameter : BasicEntity
    {
        public RutasDhogParameter(int IDd, string Rutamodelo, string Rutaejecutable, string RutabD, string Rutasalida, string rutaSolver)
        {
            Id = IDd;
            RutaModelo = Rutamodelo;
            RutaEjecutable = Rutaejecutable;
            RutaBD = RutabD;
            RutaSalida = Rutasalida;
            RutaSolver = rutaSolver;

        }

        public int ID { get; set; }
        public string RutaModelo { get; set; }
        public string RutaEjecutable { get; set; }
        public string RutaBD { get; set; }

        public string RutaSalida { get; set; }
        public string RutaSolver { get; set; }


    }
}
