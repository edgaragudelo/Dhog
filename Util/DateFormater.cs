using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Util
{
    public class DateFormater
    {
        public static string FormatDate(string value)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-CO");
            DateTimeStyles styles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal;
            if (!DateTime.TryParse(value, culture, styles, out DateTime date))
            {
                try
                {
                    date = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    try
                    {
                        date = DateTime.ParseExact(value, "d/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        try
                        {
                            date = DateTime.ParseExact(value, "dd/M/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            try
                            {
                                date = DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            return String.Format("{0:dd/MM/yyyy}", date);
        }
    }
}
