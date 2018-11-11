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
    public class AccountController : Controller
    {
        public static int SayfaNo;
        DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];

        // GET: Account
        public ActionResult ListAccount(string id, string islem)
        {
            if (udate["a_k"].ToString() == "0")
            {
                try
                {
                    if (Request.QueryString["islem"] == "sil" && utils.sayimi(Request.QueryString["id"]) && udate["a_k"].ToString() == "0")
                    {
                        int veriId = Convert.ToInt32(utils.noinject(Request.QueryString["id"]));
                        vt.cmd("DELETE FROM accounts WHERE id=" + veriId);
                        utils.logYaz(udate["accountName"].ToString(), "kullanıcı sildi.");
                        return RedirectToAction("ListAccount", "Account", new { id = id, islem = "silindi" });
                    }

                    SayfaNo = 1;
                    if (id != null && id != "")
                    {
                        try
                        {
                            int i = Convert.ToInt32(id);
                        }
                        catch
                        {
                            return Redirect("/Account/ListAccount");
                        }

                        if (id == "1")
                        {
                            return Redirect("/Account/ListAccount");
                        }

                        if (id != null)
                        {
                            id = utils.noinject(id);
                            try
                            {
                                SayfaNo = Convert.ToInt32(id);
                                if (SayfaNo < 1)
                                {
                                    SayfaNo = 1;
                                }
                            }
                            catch { }
                        }
                    }

                    int SSS = 10;
                    int Offset = 0;
                    Offset = (SayfaNo - 1) * SSS;
                    ViewBag.List = vt.GetDataTable("SELECT * FROM accounts ORDER BY name,surname ASC LIMIT " + Offset + "," + SSS + "");

                    ViewBag.ToplamSayfa = vt.GetDataCell("SELECT ceil(count(*)/" + SSS + ") as toplamsayfa FROM accounts");

                    if (ViewBag.ToplamSayfa != null && Convert.ToInt32(ViewBag.ToplamSayfa) != 0)
                    {
                        if (SayfaNo > Convert.ToInt32(ViewBag.ToplamSayfa))
                        {
                            return Redirect("/Account/ListAccount");
                        }
                    }
                }

                catch
                {
                    return Redirect("/Account/ListAccount");
                }

                return View();
            }

            else
                return Redirect("/Home");

        }


        public ActionResult AddAccount()
        {
            if (udate["a_k"].ToString() == "0")
                return View();
            else
                return Redirect("/Home");
        }


        [HttpPost]
        public ActionResult AddAccount(FormCollection frm)
        {
            try
            {
                string kontrol = udate["a_k"].ToString();

                string a_k = utils.noinjecttr(frm["elemantursec"]);
                string name = utils.noinjecttr(frm["name"]);
                string surname = utils.noinjecttr(frm["surname"]);
                string accountName = utils.noinjecttr(frm["accountName"]);
                string password = utils.noinjecttr(frm["password"]);
                string passwordAgain = utils.noinjecttr(frm["passwordAgain"]);

                if (kontrol == "0" && a_k != "" && name != "" && surname != "" && accountName != "" && password != "" && passwordAgain != "")
                {
                    if (password == passwordAgain)
                    {
                        List<vt.parameter> degerler = new List<vt.parameter>();
                        degerler.Add(new vt.parameter("name", name));
                        degerler.Add(new vt.parameter("surname", surname));
                        degerler.Add(new vt.parameter("accountName", accountName));
                        degerler.Add(new vt.parameter("password", password));
                        degerler.Add(new vt.parameter("a_k", a_k));
                        int LastId = vt.cmd(vt.parameter.command.insert, "accounts", degerler);
                        utils.logYaz(udate["accountName"].ToString(), name + " " + surname + " kullanıcı olarak eklendi.");
                        return RedirectToAction("UpdateAccount", "Account", new { id = LastId, islem = "eklendi" });
                    }

                    else
                        return Redirect("/Account/AddAccount?islem=sifreleruyusmadi");
                }

            }

            catch
            {
                return RedirectToAction("ListAccount", "Account");
            }

            return View();
        }


        public ActionResult UpdateAccount(string id, string islem)
        {
            try
            {
                int veriId = Convert.ToInt32(utils.noinject(id));
                ViewBag.Veri = vt.GetDataRow("SELECT * FROM accounts WHERE id=" + veriId);

                if (ViewBag.Veri == null && ViewBag.Veri == "" && udate["admin"].ToString() != "0")
                    return Redirect("/Home");
            }

            catch
            {
                return RedirectToAction("ListAccount", "Account");
            }

            return View();
        }


        [HttpPost]
        public ActionResult UpdateAccount(FormCollection frm, string id)
        {
            try
            {
                int VeriId = Convert.ToInt32(utils.noinject(id));

                string a_k = utils.noinjecttr(frm["elemantursec"]);
                string name = utils.noinjecttr(frm["name"]);
                string surName = utils.noinjecttr(frm["surName"]);
                string accountName = utils.noinjecttr(frm["accountName"]);
                string password = utils.noinjecttr(frm["password"]);
                string passwordAgain = utils.noinjecttr(frm["passwordAgain"]);

                if (udate["a_k"].ToString() == "0" && a_k != "" && name != "" && surName != "" && password != "" && passwordAgain != "")
                {
                    if (password == passwordAgain)
                    {
                        List<vt.parameter> degerler = new List<vt.parameter>();
                        degerler.Add(new vt.parameter("name", name));
                        degerler.Add(new vt.parameter("surname", surName));
                        degerler.Add(new vt.parameter("accountName", accountName));
                        degerler.Add(new vt.parameter("password", password));
                        degerler.Add(new vt.parameter("a_k", a_k));
                        vt.cmd(vt.parameter.command.update, "accounts", degerler, new vt.parameter("Id", VeriId));
                        utils.logYaz(udate["accountName"].ToString(), "Id si " + VeriId + " olan kullanıcıyı güncelledi.");
                        return Redirect("/Account/ListAccount/" + VeriId + "?islem=guncellendi");
                        //return RedirectToAction("UpdateAccount", "Account", new { id = VeriId , islem = "guncellendi" });

                    }

                    else
                        return Redirect("/Account/UpdateAccount?islem=sifreleruyusmadi");
                }
            }

            catch (Exception e)
            {
                return RedirectToAction("ListAccount", "Account");
            }

            return View();
        }
    }
}