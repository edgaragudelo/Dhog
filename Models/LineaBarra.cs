
namespace DHOG_WPF.Models
{
    public class LineaBarra : BasicEntity
    {
        public LineaBarra(string nombre, string barraInicial, string barraFinal, double reactancia, double flujoMaximo, int nMenos1, int activa)
        {
            Nombre = nombre;
            BarraInicial = barraInicial;
            BarraFinal = barraFinal;
            Reactancia = reactancia;
            FlujoMaximo = flujoMaximo;
            NMenos1 = nMenos1;
            Activa = activa;
        }

        public string Nombre { get; set; }
        public string BarraInicial { get; set; }

        public string BarraFinal { get; set; }

        public double FlujoMaximo { get; set; }
        public double Reactancia { get; set; }
        public int NMenos1 { get; set; }
        public int Activa { get; set; }
    }
}
