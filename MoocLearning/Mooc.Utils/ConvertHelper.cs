using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Utils
{
    public static class ConvertHelper
    {
        public static int ToInt(this object value,int defaultValue)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }
    }
}
