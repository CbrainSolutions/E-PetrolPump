using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Models
{
    

    public class MyAuthorizeAttribute : ActionFilterAttribute
    {

        public bool IsAdmin { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (SessionManager.Instance.ActiveUser!=null)
            {

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (SessionManager.Instance.ActiveUser.UserType.Equals("Admin", StringComparison.CurrentCultureIgnoreCase) == false && IsAdmin)
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Message = "you do not have sufficient permission to access."
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {//Redirect him to somewhere.
                        return;
                    }
                }
                else
                {
                    if (SessionManager.Instance.ActiveUser.UserType.Equals("Admin", StringComparison.CurrentCultureIgnoreCase) == false && IsAdmin)
                    {
                        var redirectTarget = new System.Web.Routing.RouteValueDictionary(new { action = "Index", controller = "Home", area = "" });
                        filterContext.Result = new RedirectToRouteResult(redirectTarget);
                    }
                    else
                    {//Redirect him to somewhere.
                        return;
                    }
                    
                }

            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Message = "your server session expired. you were logged out."
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {//Redirect him to somewhere.
                    var redirectTarget = new System.Web.Routing.RouteValueDictionary(new { action = "Index", controller = "Home", area = "" });
                    filterContext.Result = new RedirectToRouteResult(redirectTarget);
                }
            }

        }


    }
}