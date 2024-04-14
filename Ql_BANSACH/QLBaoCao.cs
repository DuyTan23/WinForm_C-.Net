using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ql_BANSACH
{
    public partial class QLBaoCao : Form
    {
        function fn = new function();
        int chonTim = -1;
        public QLBaoCao()
        {
            InitializeComponent();
        }

        private void QLBaoCao_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519, 199);
            loadData();
        }
        public void loadData()
        {
            String query = "SELECT CTHOADON.MAHD, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) AS TIENBAN, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS TIENVON ,SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE MONTH(HOADON.NGAYLAPHD) = 3 GROUP BY CTHOADON.MAHD;";
            dataLoiNhuan.DataSource = fn.GetDataTable(query);
            String query1 = "SELECT SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE MONTH(HOADON.NGAYLAPHD) = 3";
            DataRow row = fn.GetDataTable(query1).Rows[0];
            labelLoiNhuan.Text = row["LOINHUAN"].ToString();
            labelSoDon.Text = dataLoiNhuan.RowCount.ToString();
        }

        private void comboBaoCao_SelectedIndexChanged(object sender, EventArgs e)
        {
            chonTim = comboBaoCao.SelectedIndex;
            switch (chonTim)
            {
                case 0:
                    txtBaoCao.PlaceholderText = "Chọn vào mục ngày bên cạnh";
                    txtBaoCao.Enabled = false;
                    break;
                case 1:
                    txtBaoCao.Enabled = true;
                    txtBaoCao.PlaceholderText = "Nhập tháng cần báo cáo";
                    break;
                case 2:
                    txtBaoCao.Enabled = true;
                    txtBaoCao.PlaceholderText = "Nhập năm cần báo cáo";
                    break;
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            String query = "";
            String query1 = "";
            switch (chonTim)
            {
                case 0:
                    query = "SELECT CTHOADON.MAHD, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) AS TIENBAN, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS TIENVON ,SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE HOADON.NGAYLAPHD = '" + guna2DateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "' GROUP BY CTHOADON.MAHD;";
                    query1 = "SELECT SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE HOADON.NGAYLAPHD = '" + guna2DateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'";
                    break;
                case 1:
                    query = "SELECT CTHOADON.MAHD, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) AS TIENBAN, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS TIENVON ,SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE MONTH(HOADON.NGAYLAPHD) = "+int.Parse(txtBaoCao.Text) +" GROUP BY CTHOADON.MAHD;";
                    query1 = "SELECT SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE MONTH(HOADON.NGAYLAPHD) = "+ int.Parse(txtBaoCao.Text) + "";
                    break;
                case 2:
                    query = "SELECT CTHOADON.MAHD, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) AS TIENBAN, SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS TIENVON ,SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE YEAR(HOADON.NGAYLAPHD) = " + int.Parse(txtBaoCao.Text) + " GROUP BY CTHOADON.MAHD;";
                    query1 = "SELECT SUM(CTHOADON.SOLUONG * SANPHAM.DONGIAXUAT) - SUM(CTHOADON.SOLUONG * SANPHAM.DONGIANHAP) AS LOINHUAN FROM HOADON INNER JOIN CTHOADON ON HOADON.MAHD = CTHOADON.MAHD INNER JOIN SANPHAM ON SANPHAM.MASP = CTHOADON.MASP WHERE YEAR(HOADON.NGAYLAPHD) = " + int.Parse(txtBaoCao.Text) + "";
                    break;
            }
            SqlCommand cmd = fn.GetCommand(query);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())//đã tìm thấy
            {
                dataLoiNhuan.DataSource = fn.GetDataTable(query);
                DataRow row = fn.GetDataTable(query1).Rows[0];
                labelLoiNhuan.Text = row["LOINHUAN"].ToString();
                labelSoDon.Text = (dataLoiNhuan.RowCount-1).ToString();
            }    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1.dashboard.Enabled = true;
            this.Close();
        }
    }
}
