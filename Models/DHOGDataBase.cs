
using System.IO;

namespace DHOG_WPF.Models
{
    public class DHOGDataBase
    {
        private string inputDBFile;

        /*
        public DHOGDataBase(string description, double dHOGVersion, string inputDBFile)
        {
            Description = description;
            Version = dHOGVersion;
            InputDBFile = inputDBFile;
            //DBFolder = Path.GetDirectoryName(dbFile);
        }
        */
        public string Description { get; set; }
        public double Version { get; set; }

        public string InputDBFile
        {
            get
            {
                return inputDBFile;
            }
            set
            {
                inputDBFile = value;
                if(File.Exists(inputDBFile))
                    OutputDBFile = Path.GetDirectoryName(value) + "\\DHOG_OUT.accdb";
            }
        }

        public string OutputDBFile { get; set; }
        //public string DBFolder { get; set; }
    }
}
