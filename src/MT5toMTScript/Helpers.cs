using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    public static class Helpers
    {
        public static DateTime DTCombine(string date, string time) => DateTime.Parse(date + ' ' + time);
    }
}
