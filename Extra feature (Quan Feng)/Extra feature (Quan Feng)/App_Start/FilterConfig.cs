using System.Web;
using System.Web.Mvc;

namespace Extra_feature__Quan_Feng_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
