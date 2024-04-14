using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace Ql_BANSACH
{
    internal class function
    {
        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=LAPTOP-2J7ABSNE\SQLEXPRESS;Initial Catalog=QL_BANSACH;Integrated Security=True";
            return conn;
        }
        public DataTable GetDataTable(String query)
        {   
            SqlConnection con = GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query,con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dt.Clear();
            adapter.Fill(dt);
            return dt;
        }
        public void SetDataTable(String  query) {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;   
            con.Open();
            cmd.CommandText = query ;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public SqlCommand GetCommand(String query)
        {
            SqlConnection con = GetConnection();
            con.Open ();
            SqlCommand cmd = new SqlCommand (query, con);
            return cmd;
        }
        
    }
}
