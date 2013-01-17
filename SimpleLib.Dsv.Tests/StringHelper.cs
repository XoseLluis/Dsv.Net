using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLibTest
{
    public static class StringHelper
    {
        public static string Multiply(this string st, int times)
        {
            //all merit to this guy:
            //http://stackoverflow.com/a/8520023/169558
            return new StringBuilder().Insert(0, st, times).ToString();
        }
    }
}
