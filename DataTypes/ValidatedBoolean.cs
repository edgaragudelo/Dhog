using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOGInputFilesReader.Models
{
    class ValidatedBoolean
    {
        public ValidatedBoolean(bool value, bool valid)
        {
            Value = value;
            Valid = valid;
        }

        public bool Value { get; }
        public bool Valid { get; }
    }
}
