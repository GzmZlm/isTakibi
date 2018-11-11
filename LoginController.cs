using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isTakibiWeb.Function;

namespace isTakibiWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.showError = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            string kullaniciAdi = utils.noinjecttr(frm["account"]);
            string sifre = utils.noinjecttr(frm["password"]);

            DataRow dtVeri = vt.GetDataRow("SELECT * FROM accounts WHERE accountName= '" + kullaniciAdi + "' AND password='" + sifre + "'");

            if (dtVeri != null && dtVeri.ToString() != "")
            {
                Session["adminLogin"] = true;
                Session["admin"] = dtVeri;
                utils.logYaz(kullaniciAdi, "sisteme giriş yaptı.");
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return Redirect("/login/?login=false");
            }
        }
    }
}