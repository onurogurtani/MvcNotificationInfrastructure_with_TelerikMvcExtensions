using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Infrastructure.Extentions;

namespace Infrastructure
{
    public abstract class BaseController : Controller
    {
        protected virtual string Layout
        {
            get { return "~/Views/Shared/_Layout.cshtml"; }
        }

        public UserManager UserManager { get; set; }

        protected Uri Referer
        {
            get
            {
                if (Request.UrlReferrer != null)
                {
                    return Request.UrlReferrer;
                }
                return !string.IsNullOrEmpty(Request.Headers["Referer"]) ? new Uri(Request.Headers["Referer"]) : null;
            }
        }

        protected override void HandleUnknownAction(string actionName)
        {
            if (!String.IsNullOrEmpty(actionName))
            {
                ControllerContext.RouteData.Values["UnknownValue"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                if (ActionInvoker.InvokeAction(ControllerContext, "UnknownAction"))
                {
                    return;
                }
            }

            base.HandleUnknownAction(actionName);
        }

        public ActionResult UnknownAction([DefaultValue("")] String unknownValue)
        {
            return Content(String.Format("UnknownAction: {0} : {1}", RouteData.Values["action"], unknownValue));
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}