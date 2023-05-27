using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
