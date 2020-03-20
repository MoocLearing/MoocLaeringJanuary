using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Utils
{
    public static class ConvertHelper
    {
        public static int ToInt(this object value, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static long ToLong(this object value, long defaultValue)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static string GetConfigValue(string sKey)
        {
            string sValue = null;
            if ((sValue = System.Configuration.ConfigurationManager.AppSettings[sKey]) == null)
            {
                sValue = "";
            }
            return sValue;
        }

        public static string GetDatetime(this DateTime? dt)
        {
            if (dt == null || dt == DateTime.MinValue|dt==DateTime.MaxValue)
                return DateTime.Now.ToShortDateString();

            return Convert.ToDateTime(dt).ToShortDateString();
            
        }


    }
}
