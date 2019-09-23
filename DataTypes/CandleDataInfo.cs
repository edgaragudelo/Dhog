using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.DataTypes
{
    public class CandleDataInfo
    {
        //private DateTime _date;
        private double _open;
        private double _high;
        private double _low;
        private double _close;

        //public DateTime Date
        //{
        //    get
        //    {
        //        return this._date;
        //    }
        //    set
        //    {
        //        this._date = value;
        //    }
        //}

        public double Open
        {
            get
            {
                return this._open;
            }
            set
            {
                this._open = value;
            }
        }

        public double High
        {
            get
            {
                return this._high;
            }
            set
            {
                this._high = value;
            }
        }

        public double Low
        {
            get
            {
                return this._low;
            }
            set
            {
                this._low = value;
            }
        }

        public double Close
        {
            get
            {
                return this._close;
            }
            set
            {
                this._close = value;
            }
        }
    }
}
