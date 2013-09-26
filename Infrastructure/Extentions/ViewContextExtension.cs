using System.Web.Mvc;

namespace Infrastructure.Extentions
{
    public static class ViewContextExtension
    {
        public static BaseController BaseController(this ViewContext view)
        {
            BaseController baseController = (BaseController)view.Controller;
            return baseController;
        }
    }
}
