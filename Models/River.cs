/* Class needed by the FilesReader module */

namespace DHOG_WPF.Models
{
    public class River : BasicEntity
    {
        public River(string name, int periods)
        {
            Name = name;
            PeriodicInflows = new double[periods];
            for (int i = 0; i < periods; i++)
                PeriodicInflows[i] = -1;
        }

        public double[] PeriodicInflows { get; set; }
    }
}
