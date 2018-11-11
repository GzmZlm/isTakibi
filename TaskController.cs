using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isTakibiWeb.Function;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace isTakibiWeb.Controllers
{
    [CheckLogin]
    public class TaskController : Controller
    {
        public static int SayfaNo;
        DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];

        public ActionResult ListTask(string id, string islem)
        {
            try
            {
                if (Request.QueryString["islem"] == "sil" && utils.sayimi(Request.QueryString["id"]))
                {
                    int veriId = Convert.ToInt32(utils.noinject(Request.QueryString["id"]));
                    string kId = vt.GetDataCell("SELECT kId FROM tasks WHERE Id=" + veriId);
                    if (kId == udate["Id"].ToString())
                    {
                        vt.cmd("DELETE FROM tasks WHERE id=" + veriId);
                        utils.logYaz(udate["accountName"].ToString(), "yapılan işi sildi.");
                        return Redirect("/Task/ListTask/" + id + "?islem=silindi");
                        //return RedirectToAction("ListTask", "Task", new { id = id, islem = "silindi" });
                    }
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
                        return Redirect("/Task/ListTask");
                    }

                    if (id == "1")
                    {
                        return Redirect("/Task/ListTask");
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

                ViewBag.Kullanicilar = vt.GetDataTable("SELECT Id, name, surname FROM accounts ORDER BY name, surname ASC");

                int SSS = 10;
                int Offset = 0;
                Offset = (SayfaNo - 1) * SSS;

                //Filtrele
                string filterQuery = "WHERE";

                if (Request.QueryString["f_kullanici"] != null && utils.noinjecttr(Request.QueryString["f_kullanici"]) != "")
                {
                    filterQuery += " t.kId = " + utils.noinjecttr(Request.QueryString["f_kullanici"]) + " AND";
                }

                if (Request.QueryString["f_tarih"] != null && Request.QueryString["f_tarih"] != "")
                {
                    DateTime tarih = DateTime.Parse(utils.noinjecttr(Request.QueryString["f_tarih"]));
                    filterQuery += " t.tDate BETWEEN '" + tarih.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + tarih.ToString("yyyy-MM-dd") + " 23:59:59' AND";
                }

                if (Request.QueryString["f_tariharalik"] != null && Request.QueryString["f_tariharalik"] != "")
                {
                    string tarAra = utils.noinjecttr(Request.QueryString["f_tariharalik"]);

                    string[] newtar = Regex.Split(tarAra, " - ");

                    string bastar = newtar[0];
                    string bittar = newtar[1];

                    string[] terscevirbas = bastar.Split('-');
                    string gunbas = terscevirbas[0];
                    string aybas = terscevirbas[1];
                    string yilbas = terscevirbas[2];

                    string[] terscevirbit = bittar.Split('-');
                    string gunbit = terscevirbit[0];
                    string aybit = terscevirbit[1];
                    string yilbit = terscevirbit[2];

                    string newbastar = yilbas + "-" + aybas + "-" + gunbas;
                    string newbittar = yilbit + "-" + aybit + "-" + gunbit;
                    filterQuery += " t.tDate BETWEEN '" + newbastar + " 00:00:00' AND '" + newbittar + " 23:59:59' AND";

                }

                if (filterQuery == "WHERE")
                {
                    filterQuery = "";
                }
                else
                {
                    filterQuery = utils.sondankirp(filterQuery);
                    filterQuery = utils.sondankirp(filterQuery);
                    filterQuery = utils.sondankirp(filterQuery);
                    filterQuery = utils.sondankirp(filterQuery);
                }
                //Filtrele


                ViewBag.List = vt.GetDataTable("SELECT t.*, a.name, a.surname FROM tasks t INNER JOIN accounts a ON t.kId=a.Id " + filterQuery + " ORDER BY tDate DESC LIMIT " + Offset + "," + SSS + "");

                ViewBag.ToplamSayfa = vt.GetDataCell("SELECT ceil(count(*)/" + SSS + ") as toplamsayfa FROM tasks t " + filterQuery);

                if (ViewBag.ToplamSayfa != null && Convert.ToInt32(ViewBag.ToplamSayfa) != 0)
                {
                    if (SayfaNo > Convert.ToInt32(ViewBag.ToplamSayfa))
                    {
                        return Redirect("/Task/ListTask");
                    }
                }
            }

            catch
            {
                return Redirect("/Home");
            }

            return View();
        }


        public ActionResult MyList(string id)
        {
            try
            {
                if (Request.QueryString["islem"] == "sil" && utils.sayimi(Request.QueryString["id"]))
                {
                    int veriId = Convert.ToInt32(utils.noinject(Request.QueryString["id"]));
                    string kId = vt.GetDataCell("SELECT kId FROM tasks WHERE Id=" + veriId);
                    if (kId == udate["Id"].ToString())
                    {
                        vt.cmd("DELETE FROM tasks WHERE id=" + veriId);
                        utils.logYaz(udate["accountName"].ToString(), "yapılan işi sildi.");
                        return RedirectToAction("MyList", "Task", new { id = id, islem = "silindi" });
                    }
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
                        return Redirect("/Task/MyList");
                    }

                    if (id == "1")
                    {
                        return Redirect("/Task/MyList");
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


                ViewBag.List = vt.GetDataTable("SELECT t.*, a.name, a.surname FROM tasks t INNER JOIN accounts a ON t.kId=a.Id WHERE kId=" + udate["Id"].ToString() + " ORDER BY tDate DESC LIMIT " + Offset + "," + SSS + "");

                ViewBag.ToplamSayfa = vt.GetDataCell("SELECT ceil(count(*)/" + SSS + ") as toplamsayfa FROM tasks WHERE kId = " + udate["Id"].ToString());

                if (ViewBag.ToplamSayfa != null && Convert.ToInt32(ViewBag.ToplamSayfa) != 0)
                {
                    if (SayfaNo > Convert.ToInt32(ViewBag.ToplamSayfa))
                    {
                        return Redirect("/Task/MyList");
                    }
                }

            }

            catch (Exception e)
            {
                return Redirect("/Home");
            }

            return View();
        }


        public ActionResult AddTask()
        {
            if (udate["a_k"].ToString() == "1")
                return View();
            else
                return Redirect("/Home");
        }

        [HttpPost]
        public ActionResult AddTask(FormCollection frm)
        {
            try
            {
                string yapilanIs = utils.noinjecttr(frm["turnover"]);

                if (yapilanIs != "" && udate["a_k"].ToString() == "1")
                {
                    List<vt.parameter> degerler = new List<vt.parameter>();
                    int LastId = 0;

                    degerler.Add(new vt.parameter("kId", udate["Id"].ToString()));
                    degerler.Add(new vt.parameter("tDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    degerler.Add(new vt.parameter("doing", yapilanIs));
                    LastId = vt.cmd(vt.parameter.command.insert, "tasks", degerler);
                    utils.logYaz(udate["accountName"].ToString(), "yapılan iş ekledi.");
                    //return RedirectToAction("UpdateTask", "Task", new { id = LastId, islem = "eklendi"});
                    return Redirect("/Task/ListTask?islem=eklendi");
                }

                else if (udate["a_k"].ToString() != "1")
                    return RedirectToAction("ListTask", "Task");
            }

            catch (Exception ex)
            {
                return RedirectToAction("ListTask", "Task");
            }

            return View();
        }


        public ActionResult UpdateTask(string id, string islem)
        {
            try
            {
                int VeriId = Convert.ToInt32(utils.noinject(id));
                ViewBag.Veri = vt.GetDataRow("SELECT * FROM tasks WHERE Id=" + VeriId);

                if (ViewBag.Veri == null && ViewBag.Veri == "" && ViewBag.Veri["kId"] != udate["Id"].ToString() && udate["a_k"].ToString() == "0")
                {
                    return RedirectToAction("ListTask", "Task");
                }
            }
            catch
            {
                return RedirectToAction("ListTask", "Task");
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateTask(FormCollection frm, string id)
        {
            try
            {
                int VeriId = Convert.ToInt32(utils.noinject(id));
                string kId = vt.GetDataCell("SELECT kId FROM tasks WHERE Id=" + VeriId);
                string yapilanIs = utils.noinjecttr(frm["turnover"]);

                if (udate["a_k"].ToString() == "1" && udate["Id"].ToString() == kId && yapilanIs != "")
                {
                    List<vt.parameter> degerler = new List<vt.parameter>();
                    degerler.Add(new vt.parameter("tDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    degerler.Add(new vt.parameter("doing", yapilanIs));
                    vt.cmd(vt.parameter.command.update, "tasks", degerler, new vt.parameter("Id", VeriId));
                    utils.logYaz(udate["accountName"].ToString(), "id si " + VeriId.ToString() + " olan görevi güncelledi.");
                    return Redirect("/Task/ListTask/?islem=guncellendi");
                    //return RedirectToAction("ListTask", "Task", new { id = id, islem = "guncellendi" });
                }

                else if (udate["a_k"].ToString() != "1" || udate["Id"].ToString() != kId)
                    return RedirectToAction("ListTask", "Task");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListTask", "Task");
            }

            return View();
        }


        public ActionResult DetailTask(string id)
        {
            try
            {
                int VeriId = Convert.ToInt32(utils.noinject(id));
                ViewBag.Veri = vt.GetDataRow("SELECT * FROM tasks WHERE Id=" + VeriId);

                if (ViewBag.Veri == null && ViewBag.Veri == "" && udate["a_k"].ToString() != "0")
                    return RedirectToAction("ListTask", "Task");
            }

            catch
            {
                return RedirectToAction("ListTask", "Task");
            }

            return View();
        }


        public ActionResult ExportToExcel()
        {
            string filterQuery = "WHERE";

            //Filtrele
            if (Request.QueryString["f_kullanici"] != null && utils.noinjecttr(Request.QueryString["f_kullanici"]) != "")
            {
                filterQuery += " a.Id = '" + utils.noinjecttr(Request.QueryString["f_kullanici"]) + "' AND";
            }

            if (Request.QueryString["f_tarih"] != null && Request.QueryString["f_tarih"] != "")
            {
                DateTime tarih = DateTime.Parse(utils.noinjecttr(Request.QueryString["f_tarih"]));
                filterQuery += " t.tDate BETWEEN '" + tarih.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + tarih.ToString("yyyy-MM-dd") + " 23:59:59' AND";
            }

            if (Request.QueryString["f_tariharalik"] != null && Request.QueryString["f_tariharalik"] != "")
            {
                string tarAra = utils.noinjecttr(Request.QueryString["f_tariharalik"]);

                string[] newtar = Regex.Split(tarAra, " - ");

                string bastar = newtar[0];
                string bittar = newtar[1];

                string[] terscevirbas = bastar.Split('-');
                string gunbas = terscevirbas[0];
                string aybas = terscevirbas[1];
                string yilbas = terscevirbas[2];

                string[] terscevirbit = bittar.Split('-');
                string gunbit = terscevirbit[0];
                string aybit = terscevirbit[1];
                string yilbit = terscevirbit[2];

                string newbastar = yilbas + "-" + aybas + "-" + gunbas;
                string newbittar = yilbit + "-" + aybit + "-" + gunbit;
                filterQuery += " t.tDate BETWEEN '" + newbastar + " 00:00:00' AND '" + newbittar + " 23:59:59' AND";

            }

            if (filterQuery == "WHERE")
            {
                filterQuery = "";
            }
            else
            {
                filterQuery = utils.sondankirp(filterQuery);
                filterQuery = utils.sondankirp(filterQuery);
                filterQuery = utils.sondankirp(filterQuery);
                filterQuery = utils.sondankirp(filterQuery);
            }
            //Filtrele

            var products = vt.GetDataTable("SELECT a.name AS Ad, a.surname AS Soyad, t.tDate AS Tarih, t.doing AS YapılanIs FROM tasks y INNER JOIN accounts a ON t.kId=a.Id " + filterQuery + " ORDER BY tDate DESC");

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            //Response.ClearContent();
            //Response.Buffer = true;
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("dd-MM-yyyy") + " Is_Listesi.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            //Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Redirect("/Task/ListTask?" + string.Join(string.Empty, Request.Url.ToString().Split('?').Skip(1)));
        }

    }
}