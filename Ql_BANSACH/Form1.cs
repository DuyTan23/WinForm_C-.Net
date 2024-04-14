using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;
using System.Collections;

namespace Ql_BANSACH
{    
    public partial class Form1 : Form
    {
        function fn = new function();
        public static String manv = "";
        public static Dashboard dashboard = new Dashboard();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUsername.Text == null ||  txtPassword.Text == null)
            {
                MessageBox.Show("Bạn cần nhập đầy đủ thông tin");
            }
            else
            {
                String name = txtUsername.Text;
                String password = txtPassword.Text;
                String query = "SELECT * FROM NHANVIEN WHERE NHANVIEN.USERNAME LIKE '"+name+"' AND NHANVIEN.PASSWORD LIKE '"+password+"'";
                SqlCommand cmd = fn.GetCommand(query);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())//đã tìm thấy
                {
                    DataRow row = fn.GetDataTable(query).Rows[0];
                    manv = row["MANV"].ToString();
                    dashboard.Show();
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("Đăng nhập thất bại: ", "canh bao");
                }
            }
        }

        private void btnExitLogin_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
