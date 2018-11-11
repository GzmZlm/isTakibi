using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isTakibiWeb.Function;
using System.Data;

namespace isTakibiWeb.Controllers
{
    [CheckLogin]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];
            string toplamKullanicilar = "SELECT COUNT(*) AS tp FROM accounts;";
            string yapilanlar = "SELECT COUNT(*) AS tp FROM tasks WHERE kId = '" + udate["Id"] + "';";
            string toplamYapilanlar = "SELECT COUNT(*) AS tp FROM tasks;";

            ViewBag.gzmzlm = vt.GetDataSet(toplamKullanicilar + toplamYapilanlar + yapilanlar);

            return View();
        }

        public ActionResult LogOut()
        {
            DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];

            utils.logYaz(udate["accountsName"].ToString(), "sistemden çıkış yaptı.");
            Session["adminLogin"] = "";
            Session["admin"] = "";
            Session.Abandon();
            return Redirect("/");
        }
    }
}