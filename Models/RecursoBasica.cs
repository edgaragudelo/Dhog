
namespace DHOG_WPF.Models
{
    public class RecursoBasica : BasicEntity
    {
        public RecursoBasica(string nombre, int tML, double tMFL, double genIni, double tlIni, double tflIni, double configuracion, double maximoarranques, double modelaRampas, double modelaintervalos, double costoArranque)
        {
            Nombre = nombre;
            TML = tML;
            TMFL = tMFL;
            GenIni = genIni;
            TlIni = tlIni;
            TflIni = tflIni;
            Configuracion = configuracion;
            Maximoarranques =maximoarranques;
            ModelaRampas = modelaRampas;
            Modelaintervalos = modelaintervalos;
            CostoArranque = costoArranque;
        }

        public string Nombre { get; set; }
        public int TML { get; set; }      
        public double TMFL { get; set; }
        public double GenIni { get; set; }
        public double TlIni { get; set; }
        public double TflIni { get; set; }
        public double Configuracion { get; set; }
        public double Maximoarranques { get; set; }
        public double ModelaRampas { get; set; }
        public double Modelaintervalos { get; set; }
        public double CostoArranque { get; set; }

    }
}
