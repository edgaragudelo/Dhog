
using DHOG_WPF.DataTypes;
using System.Collections.Generic;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Models
{
    public class FilesReadingParameters
    {
        public Model Model { get; set; }
        public int PeriodsToLoad { get; set; }
        public int InitialYear { get; set; }
        public int InitialPeriod { get; set; }
        public int InitialPeriodDB { get; set; }
        public int Blocks { get; set; }
        public int InflowsReferenceYear { get; set; }
        public bool CreateScenario { get; set; }
        public bool AutomaticScenarioCreation { get; set; }
        public string ScenarioFolderName { get; set; }
        public int InitialScenarioToCreate { get; set; }
        public int PeriodsQuantity { get; set; }
        public bool ReadEcuadorLoad { get; set; }
        public string DBFile { get; set; }
        public string InputFilesPath { get; set; }
        public List<OpenRange> RiverRangesToOmit { get; set; }
        public bool RepeatMaintenances { get; set; }

        public bool RiversToOmitContains(int riverNumber)
        {
            foreach (OpenRange range in RiverRangesToOmit)
                if (range.Contains(riverNumber))
                    return true;

            return false;
        }
    }
}
