using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace isTakibiWeb.Function
{
    public class utils
    {
        public static string ReplaceBreaks(string value)
        {
            return Regex.Replace(value, @"(<br */>)|(\[br */\])", "\n");
        }

        public static string Sayfalama(int SayfaNo, int ToplamSayfa, string url)
        {
            if (ToplamSayfa > 1)
            {
                string html = "";
                int sayfalamabasla = 1;
                int butonsayisi = 4;
                int sayfabitis = 0;
                if (SayfaNo > 2)
                {
                    sayfalamabasla = SayfaNo - 2;
                }
                if (ToplamSayfa > 6)
                {
                    sayfabitis = sayfalamabasla + butonsayisi;

                    if (sayfabitis > ToplamSayfa - 2) { sayfabitis = ToplamSayfa; sayfalamabasla = sayfabitis - 7; }
                }
                else
                {
                    sayfabitis = ToplamSayfa;
                }

                string qString = "?";
                foreach (String key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    qString += key + "=" + HttpContext.Current.Request.QueryString[key] + "&";
                }
                if (qString == "?")
                {
                    qString = "";
                }
                else
                {
                    qString = utils.sondankirp(qString);
                }

                if (SayfaNo > 1)
                {
                    html += "<li><a href='" + url + qString + "' class='fa fa-angle-double-left' aria-label='Prev'></a></li>";
                    html += "<li><a href='" + url + "/" + (SayfaNo - 1) + qString + "' class='fa fa-angle-left' aria-label='Prev'></a></li>";
                }

                for (int i = sayfalamabasla; i <= sayfabitis; i++)
                {
                    if (i == SayfaNo)
                    {
                        html += "<li class='active'><a href='javascript:void(0);'>" + i + "</a></li>";
                    }
                    else
                    {
                        html += "<li><a href='" + url + "/" + i + qString + "'>" + i + "</a></li>";
                    }

                }

                if (SayfaNo < ToplamSayfa)
                {
                    html += "<li><a href='" + url + "/" + (SayfaNo + 1) + qString + "' class='fa fa-angle-right' aria-label='Next'></a></li>";
                    html += "<li><a href='" + url + "/" + ToplamSayfa + qString + "' class='fa fa-angle-double-right' aria-label='Next'></a></li>";
                }

                return html;
            }
            else
            {
                return "";
            }
        }

        public static string QueryFilter(string str)
        {
            str = str.Replace("*", "[INJ]");
            str = str.Replace("<", "[INJ]");
            str = str.Replace(">", "[INJ]");
            str = str.Replace(";", "[INJ]");
            str = str.Replace("(", "[INJ]");
            str = str.Replace(")", "[INJ]");
            str = str.Replace("UNION", "[INJ]");
            str = str.Replace("SELECT", "[INJ]");
            str = str.Replace("WHERE", "[INJ]");
            str = str.Replace("UPDATE", "[INJ]");
            str = str.Replace("INSERT", "[INJ]");
            str = str.Replace("ORDER", "[INJ]");
            str = str.Replace("MODIFY", "[INJ]");
            str = str.Replace("RENAME", "[INJ]");
            str = str.Replace("DECLARE", "[INJ]");
            str = str.Replace("TABLE_NAME", "[INJ]");
            str = str.Replace("COLUMN_NAME", "[INJ]");
            str = str.Replace("COLUMNS", "[INJ]");
            str = str.Replace("DATA_TYPE", "[INJ]");
            str = str.Replace("CHARACTER", "[INJ]");
            str = str.Replace("LENGTH", "[INJ]");
            str = str.Replace("FETCH", "[INJ]");
            str = str.Replace("STATUS", "[INJ]");
            str = str.Replace("union", "[INJ]");
            str = str.Replace("select", "[INJ]");
            str = str.Replace("update", "[INJ]");
            str = str.Replace("inster", "[INJ]");
            str = str.Replace("order", "[INJ]");
            str = str.Replace("modify", "[INJ]");
            str = str.Replace("rename", "[INJ]");
            str = str.Replace("declare", "[INJ]");
            str = str.Replace("table_name", "[INJ]");
            str = str.Replace("column_table", "[INJ]");
            str = str.Replace("columns", "[INJ]");
            str = str.Replace("data_type", "[INJ]");
            str = str.Replace("character", "[INJ]");
            str = str.Replace("length", "[INJ]");
            str = str.Replace("fetch", "[INJ]");
            str = str.Replace("status", "[INJ]");
            str = str.Replace("adf.ly", "[INJ]");
            return str;
        }

        public static string noinject(string text)
        {
            text = text.Replace("'", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("!", "");
            text = text.Replace("^", "");
            text = text.Replace("%", "");
            text = text.Replace("*", "");
            text = text.Replace("''", "");
            text = text.Replace("#", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace("iframe", "");
            text = text.Replace("<script>", "");
            text = text.Replace("script", "");
            text = text.Replace("<cookie>", "");
            text = text.Replace("cookie", "");
            text = text.Replace("thread", "");
            text = text.Replace("session", "");
            text = text.Replace("$", "");
            text = text.Replace("function", "");
            text = text.Replace("$function()", "");
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("alert", "");
            text = text.Replace("select", "");
            text = text.Replace("union", "");
            text = text.Replace("update", "");
            text = text.Replace("delete", "");
            text = text.Replace("char", "");
            return Regex.Replace(text, "[^A-Za-z0-9$-_]", "");
        }

        public static string noinjecttr(string text)
        {
            if (text == null) return text;
            text = text.Replace("'", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("!", "");
            text = text.Replace("^", "");
            text = text.Replace("%", "");
            text = text.Replace("*", "");
            text = text.Replace("''", "");
            text = text.Replace("#", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace("iframe", "");
            text = text.Replace("<script>", "");
            text = text.Replace("script", "");
            text = text.Replace("<cookie>", "");
            text = text.Replace("cookie", "");
            text = text.Replace("thread", "");
            text = text.Replace("session", "");
            text = text.Replace("$", "");
            text = text.Replace("function", "");
            text = text.Replace("$function()", "");
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("alert", "");
            text = text.Replace("select", "");
            text = text.Replace("union", "");
            text = text.Replace("update", "");
            text = text.Replace("delete", "");
            text = text.Replace("char", "");
            return text;
        }

        public static string sondankirp(string text, int lenght = 1)
        {
            if (text.Length > 1)
            {
                return text.Substring(0, (text.Length - lenght));
            }
            else
            {
                return text;
            }
        }

        public static bool sayimi(string str)
        {
            long i;
            bool ret = true;
            //if (int.TryParse(str, out i)) { return true; } else { return false; }
            try
            {
                i = Int64.Parse(str);

            }
            catch
            {
                ret = false;
            }
            return ret;

        }

        public static Boolean sendMail(string to, string subject, string message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                if (to != "")
                {
                    msg.To.Add(new MailAddress(to.Trim()));
                    msg.To.Add(new MailAddress("email@****.***"));
                }
                else
                {
                    msg.To.Add(new MailAddress("email@****.***"));
                }
                msg.From = new MailAddress("email@****.***", "İş takip Sistemi");
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = message;
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                SmtpClient server = new SmtpClient("mail.****.***", 587);
                server.Credentials = new System.Net.NetworkCredential("email@****.***", "********");
                server.EnableSsl = false; server.Send(msg); msg.Dispose();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public static void logYaz(string baslik, string aciklama)
        {
            DataRow udate = (DataRow)System.Web.HttpContext.Current.Session["admin"];
            vt.cmd("INSERT INTO operations (oDate,person,type,explanation) VALUES ('" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'," + udate["Id"] + ",'" + baslik + "','" + udate["accountName"] + " " + aciklama + "')");
        }
    }
}