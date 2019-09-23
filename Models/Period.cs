using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class Period
    {
        public Period() { }

        /* Constructor needed by the FilesReader module */
        public Period(DateTime dateFR, int name, double load, double hourlyDuration, double rationingCost, double car)
        {
            DateFR = dateFR;
            Name = name;
            Load = load;
            HourlyDuration = hourlyDuration;
            RationingCost = rationingCost;
            CAR = car;
        }

        public int Id { get; set; }
        public string Date { get; set; }
        public DateTime DateFR { get; } // Date needed by the DHOGFilesReader
        public int Name { get; set; }
        public double Load { get; set; }
        public double HourlyDuration { get; set; }
        public double AGCReservoir { get; set; }
        public double RationingCost { get; set; }
        public double CAR { get; set; }
        public double InternationalLoad { get; set; }
        public double DiscountRate { get; set; }
        public int Case { get; set; }
    }
}
