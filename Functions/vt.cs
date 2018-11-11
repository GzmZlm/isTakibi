using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace isTakibiWeb.Function
{
    public class vt
    {
        public static MySqlConnection Conn()
        {
            MySqlConnection Conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["cstr"].ToString());
            Conn.Open();
            return Conn;
        }


        public static int cmd(string sqlcumle)
        {
            MySqlConnection Conn = vt.Conn();
            MySqlCommand Cmd = new MySqlCommand(sqlcumle, Conn);
            int sonuc = 0;
            try
            {
                sonuc = Cmd.ExecuteNonQuery();
                sonuc = Convert.ToInt16(Cmd.LastInsertedId);
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message + " (" + sqlcumle + ")");
            }
            Cmd.Dispose();
            Conn.Close();
            Conn.Dispose();
            return (sonuc);
        }


        public class parameter
        {
            public enum command
            {
                insert = 0,
                update = 1
            }
            private string _field;
            private object _value;
            public string field
            {
                get { return _field; }
                set { _field = value; }
            }
            public object value
            {
                get { return _value; }
                set { _value = value; }
            }
            public parameter(string field, object value)
            {
                _field = field;
                _value = value;
            }
        }


        public static int cmd(vt.parameter.command komut, string tablo, List<vt.parameter> parameters, vt.parameter where = null)
        {
            // kullanim örneği
            //List<vt.parameter> degerler = new List<vt.parameter>();
            //degerler.Add(new vt.parameter("label", "mesut"));
            //degerler.Add(new vt.parameter("value", "test"));
            //degerler.Add(new vt.parameter("metin", "buraya'da  yazalım."));
            //degerler.Add(new vt.parameter("tarih", DateTime.Now));
            //vt.cmd(vt.parameter.command.insert, "test", degerler);

            int sonuc = 0;
            switch (komut)
            {
                case vt.parameter.command.insert:
                    string sql = "INSERT INTO " + tablo + "({__GZM_KOLONLAR__}) VALUES ({__GZM_DEGERLER__});";
                    string kolonlar = "";
                    string degerler = "";
                    foreach (vt.parameter p in parameters)
                    {
                        kolonlar += p.field + ",";
                        degerler += "@" + p.field + ",";
                    }
                    kolonlar = utils.sondankirp(kolonlar);
                    degerler = utils.sondankirp(degerler);
                    sql = sql.Replace("{__GZM_KOLONLAR__}", kolonlar).Replace("{__GZM_DEGERLER__}", degerler);
                    MySqlConnection Conn = vt.Conn();
                    MySqlCommand Cmd = new MySqlCommand(sql, Conn);
                    Cmd.Parameters.Clear();
                    foreach (vt.parameter p in parameters)
                    {
                        Cmd.Parameters.AddWithValue("@" + p.field, p.value);
                    }
                    try
                    {
                        sonuc = Cmd.ExecuteNonQuery();
                        sonuc = Convert.ToInt16(Cmd.LastInsertedId);
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message + " (" + sql + ")");
                    }
                    Cmd.Dispose();
                    Conn.Close();
                    Conn.Dispose();
                    break;
                case vt.parameter.command.update:
                    string usql = "UPDATE " + tablo + " SET {__GZM_UPDATEPARAM__}";
                    if (where != null)
                    {
                        usql += " WHERE " + where.field + "=@W__" + where.field;
                    }
                    string uparam = "";
                    foreach (vt.parameter p in parameters)
                    {
                        uparam += p.field + "=" + "@" + p.field + ",";
                    }
                    uparam = utils.sondankirp(uparam);
                    usql = usql.Replace("{__GZM_UPDATEPARAM__}", uparam);
                    MySqlConnection uConn = vt.Conn();
                    MySqlCommand uCmd = new MySqlCommand(usql, uConn);
                    uCmd.Parameters.Clear();
                    foreach (vt.parameter p in parameters)
                    {
                        uCmd.Parameters.AddWithValue("@" + p.field, p.value);
                    }
                    if (where != null)
                    {
                        uCmd.Parameters.AddWithValue("@W__" + where.field, where.value);
                    }
                    try
                    {
                        sonuc = uCmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message + " (" + usql + ")");
                    }
                    uCmd.Dispose();
                    uConn.Close();
                    uConn.Dispose();
                    break;
            }
            return sonuc;
        }


        public static DataTable GetDataTable(string sql)
        {
            MySqlConnection Conn = vt.Conn();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, Conn);
            DataTable dt = new DataTable();
            try
            {
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message + " (" + sql + ")");
            }
            adapter.Dispose();
            Conn.Close();
            Conn.Dispose();
            return dt;
        }


        public static DataSet GetDataSet(string sql)
        {
            MySqlConnection Conn = vt.Conn();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, Conn);
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message + " (" + sql + ")");
            }
            adapter.Dispose();
            Conn.Close();
            Conn.Dispose();
            return ds;
        }


        public static DataRow GetDataRow(string sql)
        {
            DataTable table = GetDataTable(sql);
            if (table.Rows.Count == 0) return null;
            return table.Rows[0];
        }


        public static string GetDataCell(string sql)
        {
            DataTable table = GetDataTable(sql);
            if (table.Rows.Count == 0) return null;
            return table.Rows[0][0].ToString();
        }


        public static int GetCount(string strSQL)
        {
            DataSet ds = GetDataSet(strSQL);
            return ds.Tables[0].Rows.Count;
        }

        internal static void GetDataCell()
        {
            throw new NotImplementedException();
        }
    }
}