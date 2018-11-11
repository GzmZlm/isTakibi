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
    public class ReportController : Controller
    {
        public static int SayfaNo;
        DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];

        // GET: Report
        public ActionResult List(string id)
        {
            if (udate["a_k"].ToString() == "0")
            {
                try
                {
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

                    //Filter
                    string filterQuery = "AND";

                    if (Request.QueryString["f_kullanici"] != null && utils.noinjecttr(Request.QueryString["f_kullanici"]) != "")
                    {
                        filterQuery += " t.kId = " + utils.noinjecttr(Request.QueryString["f_kullanici"]) + " AND";
                    }


                    if (filterQuery == "AND")
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
                    //Filter

                    ViewBag.Kullanicilar = vt.GetDataTable("SELECT Id, name, surname FROM accounts ORDER BY name, surname ASC");

                    int SSS = 10;
                    int Offset = 0;
                    Offset = (SayfaNo - 1) * SSS;

                    DateTime haftalik = DateTime.Now.AddDays(-7);

                    ViewBag.List = vt.GetDataTable("SELECT t.*, a.name, a.surname FROM tasks t INNER JOIN accounts a ON t.kId=a.Id WHERE tDate BETWEEN '" + haftalik.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59'" + filterQuery + " ORDER BY tDate DESC LIMIT " + Offset + "," + SSS + "");

                    ViewBag.ToplamSayfa = vt.GetDataCell("SELECT ceil(count(*)/" + SSS + ") as toplamsayfa FROM tasks WHERE tarih BETWEEN '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + haftalik.ToString("yyyy-MM-dd") + " 23:59:59'" + filterQuery);

                    if (ViewBag.ToplamSayfa != null && Convert.ToInt32(ViewBag.ToplamSayfa) != 0)
                    {
                        if (SayfaNo > Convert.ToInt32(ViewBag.ToplamSayfa))
                        {
                            return Redirect("/Report/List");
                        }
                    }

                }
                catch
                {
                    return Redirect("/Home");
                }

                return View();
            }

            else
                return Redirect("/Home");
        }


        public ActionResult SendToMail()
        {
            //Filtrele
            string filterQuery = "AND";

            if (Request.QueryString["f_kullanici"] != null && utils.noinjecttr(Request.QueryString["f_kullanici"]) != "")
            {
                filterQuery += " t.kId = " + utils.noinjecttr(Request.QueryString["f_kullanici"]) + " AND";
            }

            if (filterQuery == "AND")
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

            DateTime haftalik = DateTime.Now.AddDays(-7);

            DataTable List = vt.GetDataTable("SELECT t.*, a.name, a.surname FROM tasks t INNER JOIN accounts a ON t.kId=a.Id WHERE tDate BETWEEN '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + haftalik.ToString("yyyy-MM-dd") + " 23:59:59'" + filterQuery + " ORDER BY tDate DESC");


            string mailInformation = "";
            // mailInformation += "<html><head> <style> .table { border:1px solid #d2d2d2; padding:4px; } .table td { border:1px solid #d2d2d2; padding:4px; } </style></head>";
            mailInformation = "<table style='border:1px solid #d2d2d2;'>";
            mailInformation += "<tr><td style='border:1px solid #d2d2d2;'>#</td>";
            mailInformation += "<td style='border:1px solid #d2d2d2;'>Ad</td>";
            mailInformation += "<td style='border:1px solid #d2d2d2;'>Soyad</td>";
            mailInformation += "<td style='border:1px solid #d2d2d2;'>Tarih</td>";
            mailInformation += "<td style='border:1px solid #d2d2d2;'>Yapılan İş</td></tr>";

            int j = 0;
            for (int i = 0; i < List.Rows.Count; i++)
            {
                j = i + 1;
                DateTime dTarih = Convert.ToDateTime(List.Rows[i]["tarih"]);
                mailInformation += "<tr><td style='border:1px solid #d2d2d2;'>" + j + "</td>";
                mailInformation += "<td style='border:1px solid #d2d2d2;'>" + List.Rows[i]["ad"].ToString() + "</td>";
                mailInformation += "<td style='border:1px solid #d2d2d2;'>" + List.Rows[i]["soyad"].ToString() + "</td>";
                mailInformation += "<td style='border:1px solid #d2d2d2;'>" + dTarih.ToString("dd-MM-yyy") + "</td>";
                mailInformation += "<td style='border:1px solid #d2d2d2;'>" + List.Rows[i]["yapilan_is"].ToString() + "</td></tr>";
            }

            mailInformation += "</table>";

            string mailmsg = "";
            mailmsg = mailmsg.Replace(Environment.NewLine, "<br/>");
            mailmsg = mailmsg.Replace("{NAMESURNAME}", "Yetkili");
            mailmsg = mailmsg.Replace("{SITETITLE}", "İş Takip Sistemi");

            mailmsg = mailmsg.Replace("{MAILINFORMATION}", mailInformation);

            if (utils.sendMail("gizem.ozlem.oztekesin@gmail.com", "Haftalık Rapor Bilgileri", mailInformation))
                return Redirect("/Report/List?sendmail=true");

            else
                return Redirect("/Report/List?sendmail=error");
        }
    }
}