using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure;
using Infrastructure.Extentions;
using Telerik.Web.Mvc;

namespace Web.Controllers
{
    public class TestController : BaseController
    {
       


        public ActionResult Index()
        {

            base.UserManager = new UserManager
                                   {
                                       Id = 1,
                                       Name = "testUser",
                                       IsAuthenticated = true
                                   };
            return View();

        }


        public ActionResult Error()
        {
            this.ShowMessage(MessageType.Error, "Error");
            return View("Index");
        }

        public ActionResult Success()
        {
            this.ShowMessage(MessageType.Success, "Success");
            return View("Index");
        }

        public ActionResult Warning()
        {
            this.ShowMessage(MessageType.Warning, "Warning");
            return View("Index");
        }



        public ActionResult RedirectError()
        {
            this.ShowMessage(MessageType.Error, "RedirectError", true);
            return View();
        }

        public ActionResult RedirectSuccess()
        {
            this.ShowMessage(MessageType.Success, "RedirectSuccess", true);
            return View();
        }

        public ActionResult RedirectWarning()
        {
            this.ShowMessage(MessageType.Warning, "RedirectWarning", true);
            return View();
        }


        [HttpPost]
        public ActionResult ErrorAjax()
        {
            this.ShowMessage(MessageType.Error, "Error");
            return new JsonResult
            {
                Data = new { Success = true }
            };
        }

        public ActionResult SuccessAjax()
        {
            this.ShowMessage(MessageType.Success, "Success");
            return new JsonResult
            {
                Data = new { Success = true }
            };
        }

        public ActionResult WarningAjax()
        {
            this.ShowMessage(MessageType.Warning, "Warning");
            return new JsonResult
            {
                Data = new { Success = true }
            };
        }



        public ActionResult UnAuthorizedException()
        {
            throw new UnauthorizedAccessException("UnauthorizedAccessException");

        }

        [HttpPost]
        public JsonResult AjaxUnAuthorizedException()
        {
            throw new UnauthorizedAccessException("UnauthorizedAccessException");
        }



        public ActionResult UnHandledException()
        {
            throw new HttpUnhandledException("HttpUnhandledException");
        }

        [HttpPost]
        public JsonResult AjaxUnHandledException()
        {
            throw new HttpUnhandledException("HttpUnhandledException");
        }



        [GridAction(EnableCustomBinding = true)]
        public ActionResult LoadGrid(GridCommand command, int? id, string text, int? errorTypeId)
        {
            switch (errorTypeId)
            {
                case 0:
                    throw new HttpUnhandledException("HttpUnhandledException");
                case 1:
                    throw new UnauthorizedAccessException("UnauthorizedAccessException");
                case 2:
                    this.ShowMessage(MessageType.Warning, "Warning");
                    return new JsonResult { Data = new GridModel<User> { Data = null, Total = 0 } };
                case 3:
                    this.ShowMessage(MessageType.Success, "Success");
                    return new JsonResult { Data = new GridModel<User> { Data = null, Total = 0 } };
                case 4:
                    this.ShowMessage(MessageType.Error, "Error");
                    return new JsonResult { Data = new GridModel<User> { Data = null, Total = 0 } };
            }
            var list = new List<User>
            {
                new User {Id = 0, Name = "Noname1"},
                new User {Id = 1, Name = "Noname2"},
                new User {Id = 2, Name = "Noname3"},
                new User {Id = 3, Name = "Noname4"}
            };
            list = list.Where(u => u.Name.Contains(text) || u.Id == id).ToList();
            return new JsonResult
            {
                Data = new GridModel<User> { Data = list, Total = list.Count }
            };
        }

    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ErrorTypeId { get; set; }

    }
}
