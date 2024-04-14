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

namespace Ql_BANSACH
{
    public partial class Dashboard : Form
    {
        function fn = new function();
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            if (Form1.manv != "")
            {
                String query = "select TENNV from NHANVIEN where MANV = '" + Form1.manv + "'";
                if (fn.GetDataTable(query).Rows.Count > 0)
                {
                    DataRow row = fn.GetDataTable(query).Rows[0];
                    btnLogin.Text = row["TENNV"].ToString();
                }
            }
        }

        private void btnExitDashboard_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            QLKhachHang qLKhachHang = new QLKhachHang();
            Form1.dashboard.Enabled = false;
            qLKhachHang.Show();
            
            
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            QLSanPham qLSanPham = new QLSanPham();
            qLSanPham.Show();
            Form1.dashboard.Enabled = false;


        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            BanHang ban = new BanHang();
            Form1.dashboard.Enabled = false;
            ban.Show();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            QlHoaDon ql = new QlHoaDon();
            Form1.dashboard.Enabled = false;
            ql.Show();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {

            QLBaoCao ql  = new QLBaoCao();
            Form1.dashboard.Enabled = false;
            ql.Show();
        }
    }
}
