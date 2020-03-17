using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class BaseController : Controller
    {
       
        public BaseController()
        {

        }

        public void SetSession(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            System.Web.HttpContext.Current.Session[name] = value;
        }

        public T GetSession<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                return default(T);

            var session = System.Web.HttpContext.Current.Session[name];
            if(session==null)
                return default(T);

            return (T)System.Web.HttpContext.Current.Session[name];
        }
       
    }
}