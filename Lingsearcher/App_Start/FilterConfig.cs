using System.Web.Mvc;
using Lingsearcher.Filters;

namespace Lingsearcher
{
    public class FilterConfig
    {
        public static object AuthorizationFilter { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
