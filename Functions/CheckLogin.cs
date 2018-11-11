using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace isTakibiWeb.Function
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool ulogin = false;

            if (HttpContext.Current.Session["adminLogin"] != null && HttpContext.Current.Session["adminLogin"].ToString() != "")
            {
                ulogin = true;
            }


            if (!ulogin)
            {
                filterContext.Result = new EmptyResult();
                filterContext.Result = new RedirectResult("/login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}