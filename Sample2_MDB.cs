using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Jeff.DB
{
    public class MDB
    {
        //public string Txt_path;  // file path
        private List<string> P_str_Name = new List<string>(); // table list 

        public List<string> GetTableList(string Txt_path)//對下拉列表進行資料繫結
        {
            //連接Access資料庫
            OleDbConnection olecon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Txt_path);
            olecon.Open();//打開資料庫連接

            //連接Access資料庫
            System.Data.DataTable DTable = olecon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });//實例化表對像

            DataTableReader DTReader = new DataTableReader(DTable);//實例化表讀取對像
            while (DTReader.Read())//循環讀取
            {
                P_str_Name.Add(DTReader["Table_Name"].ToString().Replace('$', ' ').Trim());//記錄工作表名稱
            }

            DTable = null;//清空表對像
            DTReader = null;//清空表讀取對像
            olecon.Close();//關閉資料庫連接
            return P_str_Name;
        }

        //連線Access MDB
        private static OleDbConnection OleDbOpenConn(string Database)
        {
            string cnstr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Database);
            OleDbConnection icn = new OleDbConnection();
            icn.ConnectionString = cnstr;
            if (icn.State == ConnectionState.Open) icn.Close();
            icn.Open();
            return icn;
        }

        //取得 MDB table raw data
        public DataTable GetOleDbDataTable(string Database, string SQL)
        {
            DataTable myDataTable = new DataTable();
            OleDbConnection icn = OleDbOpenConn(Database);
            OleDbDataAdapter da = new OleDbDataAdapter(SQL, icn);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            myDataTable = ds.Tables[0];
            if (icn.State == ConnectionState.Open) icn.Close();
            return myDataTable;
        }

    }
}
