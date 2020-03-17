using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Utils
{
    public class LoginHelper
    {
        public static string loginCookieId = "userid";
        public static string loginCookieName = "username";

        public static long UserId
        {
            get { return CookieHelper.GetCookie(loginCookieId).ToLong(0); }
        }

        public static string UserName
        {
            get { return CookieHelper.GetCookie(loginCookieName); }
        }

    }
}
