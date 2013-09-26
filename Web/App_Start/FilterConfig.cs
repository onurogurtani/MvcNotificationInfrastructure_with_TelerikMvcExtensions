using System.Web.Mvc;
using Infrastructure.Filter;

namespace Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandling());
            filters.Add(new AjaxMessagesFilter());
        }
    }
}