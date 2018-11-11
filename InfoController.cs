using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isTakibiWeb.Function;

namespace isTakibiWeb.Controllers
{
    [CheckLogin]
    public class InfoController : Controller
    {
        DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];
        // GET: Info
        public ActionResult Information()
        {
            ViewBag.Veri = vt.GetDataRow("SELECT * FROM accounts WHERE Id=" + udate["Id"].ToString());

            return View();
        }

        [HttpPost]
        public ActionResult Information(FormCollection frm)
        {
            try
            {
                string sifre = utils.noinjecttr(frm["oldPassword"]);
                string yenisifre = utils.noinjecttr(frm["password"]);
                string tekrarsifre = utils.noinjecttr(frm["passwordAgain"]);

                if (udate["password"].ToString() == sifre)
                {
                    if (yenisifre == tekrarsifre)
                    {
                        List<vt.parameter> deger = new List<vt.parameter>();
                        deger.Add(new vt.parameter("password", yenisifre));
                        vt.cmd(vt.parameter.command.update, "accounts", deger, new vt.parameter("Id", udate["Id"]));
                        utils.logYaz(udate["accountName"].ToString(), "şifresini güncelledi.");
                        return Redirect("/Info/Information?islem=guncellendi");
                    }

                    else
                        return Redirect("/Info/Information?islem=ytsifreuyusmadi");
                }

                else
                    return Redirect("/Info/Information?islem=sifreleruyusmadi");
            }
            catch
            {
                return Redirect("/Home");
            }
        }
    }
}