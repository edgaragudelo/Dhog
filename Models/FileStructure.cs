using System.Collections.Generic;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Models
{
    public class FileStructure
    {
        public FileStructure(string name, FileType type, int headerLines, bool dividedByPlants, bool sumNeeded, bool repeatPeriodicalValues, int periodicalColumnsWidth, int headerColumns, List<int> headerColumnsWidth)
        {
            Name = name;
            Type = type;
            HeaderLines = headerLines;
            DividedByPlants = dividedByPlants;
            SumNeeded = sumNeeded;
            RepeatPeriodicalValues = repeatPeriodicalValues;
            PeriodicalColumnsWidth = periodicalColumnsWidth;
            HeaderColumns = headerColumns;
            HeaderColumnsWidth = headerColumnsWidth;
            SetStartLine();
        }

        public string Name { get; }
        public FileType Type { get; }
        public int HeaderLines { get; }
        public bool DividedByPlants { get; }
        public bool SumNeeded { get; }
        public bool RepeatPeriodicalValues { get; }
        public int PeriodicalColumnsWidth { get; }
        public int HeaderColumns { get; }
        public List<int> HeaderColumnsWidth { get; }
        public int StartLine { get; private set; }

        private void SetStartLine()
        {
            if (HeaderLines == 0)
                StartLine = 0;
            else 
                StartLine = HeaderLines;
        }
    }
}
