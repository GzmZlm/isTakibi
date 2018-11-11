using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace isTakibiWeb.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult _Layout()
        {
            DataRow udate = (DataRow)Session["admin"];

            return View();
        }
    }
}