using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Infrastructure.Filter
{
    public class ExceptionHandling : FilterAttribute, IExceptionFilter
    {


        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                filterContext.Result = UnauthorizedAccessExceptionResult(filterContext);
            }
            else if (filterContext.Exception is BusinessException)
            {
                filterContext.Result = BusinessExceptionResult(filterContext);
            }
            else //Unhandled Exception
            {

                filterContext.Result = UnhandledExceptionResult(filterContext);
            }
        }

        private static ActionResult UnauthorizedAccessExceptionResult(ExceptionContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 403;
            var response = filterContext.HttpContext.Response;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            if (!request.IsAjaxRequest())
            {
                return new RedirectResult("/Login/Index?ReturnUrl=" + request.RawUrl + "");
            }
            if (request.UrlReferrer != null)
                response.AddHeader("X-RedirectUrl", "/Login/Index?ReturnUrl=" + request.UrlReferrer.AbsolutePath + "");

            return new ContentResult { Content = string.Empty };
        }

        private static ActionResult BusinessExceptionResult(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Controller.ViewData.ModelState.AddModelError("System Error", filterContext.Exception.Message);
                filterContext.HttpContext.Response.StatusCode = 500;
                var errors = String.Join("\n",
                                            (from state in filterContext.Controller.ViewData.ModelState select state)
                                                .SelectMany(s => s.Value.Errors)
                                                .Select(e => e.ErrorMessage)
                                                .ToArray()
                                                .Distinct());
                var response = filterContext.HttpContext.Response;
                response.AddHeader("X-Message-Type", MessageType.Error.ToString());
                response.AddHeader("X-Message", errors);
                return new ContentResult { Content = errors };
            }
            filterContext.ExceptionHandled = true;
            var lRoutes = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Error"
                });
            return new RedirectToRouteResult(lRoutes);
        }

        private static ActionResult UnhandledExceptionResult(ExceptionContext filterContext)
        {


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Controller.ViewData.ModelState.AddModelError("System Error", filterContext.Exception.Message);

                var errors = String.Join("\n",
                                            (from state in filterContext.Controller.ViewData.ModelState select state)
                                                .SelectMany(s => s.Value.Errors)
                                                .Select(e => e.ErrorMessage)
                                                .ToArray());
                var response = filterContext.HttpContext.Response;
                response.AddHeader("X-Message-Type", MessageType.Error.ToString());
                response.AddHeader("X-Message", errors);
                return new ContentResult { Content = errors };
            }
            filterContext.ExceptionHandled = true;
            var lRoutes = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Error"
                });

            return new RedirectToRouteResult(lRoutes);
        }
    }

    public class BusinessException : Exception
    {
    }
}