using Ql_BANSACH;
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
    public partial class QLSanPham : Form
    {
        String stringConnection = @"Data Source=LAPTOP-2J7ABSNE\SQLEXPRESS;Initial Catalog=QL_BANSACH;Integrated Security=True";
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        int chonTim;
        function fn = new function();
        public static String Masp = "";
        public QLSanPham()
        {
            InitializeComponent();
        }

        private void QLSanPham_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519, 199);
            loadData();
            khoaTxt();
            comboTimSp.SelectedIndex = 0;
            txtTimSp.PlaceholderText = "Nhập mã sản phẩm cần tìm";
        }
        public void loadData()
        {
            /*String query = "select * from SANPHAM";
            dataSanPham.DataSource = fn.GetDataTable(query);
            khoaTxt();*/
            String query = "select * from SANPHAM";
            conn = new SqlConnection(stringConnection);
            conn.Open();
            cmd = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            dataSanPham.DataSource = dt;
        }

        private void dataSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataSanPham.CurrentRow.Index;
            txtMaSp.Text = dataSanPham.Rows[i].Cells[0].Value.ToString();
            txtTenSp.Text = dataSanPham.Rows[i].Cells[1].Value.ToString();
            Masp = dataSanPham.Rows[i].Cells[0].Value.ToString();
        }
        public void khoaTxt()
        {
            txtMaSp.Enabled = false;
            txtTenSp.Enabled = false;
        }

        private void btnTimSp_Click(object sender, EventArgs e)
        {
            String query = "";
            switch (chonTim)
            {
                case 0:
                    query = "select * from SANPHAM where MASP = '" + txtTimSp.Text + "'";
                    break;
                case 1:
                    query = "select * from SANPHAM where TENSP like '" + txtTimSp.Text + "'";
                    break;
                case 2:
                    query = "select * from SANPHAM where LOAISP = '" + txtTimSp.Text + "'";
                    break;

            }

            conn.Open();

            cmd = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            dataSanPham.DataSource = dt;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())//đã tìm thấy
            {
                DialogResult dialog = MessageBox.Show("Đã tìm thấy sản phẩm: ", "canh bao");
                dataSanPham.DataSource = fn.GetDataTable(query);
                int i = dataSanPham.CurrentRow.Index;
                txtMaSp.Text = dataSanPham.Rows[i].Cells[0].Value.ToString();
                txtTenSp.Text = dataSanPham.Rows[i].Cells[1].Value.ToString();
                Masp = dataSanPham.Rows[i].Cells[0].Value.ToString();
            }
            else
            {
                DialogResult dialog = MessageBox.Show("Không tìm thấy sản phẩm nào: ", "canh bao");
                dataSanPham.DataSource = null;
            }
        }

        private void comboTimSp_SelectedIndexChanged(object sender, EventArgs e)
        {
            chonTim = comboTimSp.SelectedIndex;
            switch (chonTim)
            {
                case 0:
                    txtTimSp.PlaceholderText = "Nhập mã sản phẩm cần tìm";
                    break;
                case 1:
                    txtTimSp.PlaceholderText = "Nhập tên sản phẩm cần tìm";
                    break;
                case 2:
                    txtTimSp.PlaceholderText = "Nhập loại cần tìm";
                    break;

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemSp formThemSp = new FormThemSp();
            formThemSp.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1.dashboard.Enabled = true;
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            showMessageBox();

        }
        public void showMessageBox()
        {
            DialogResult dialog = MessageBox.Show("Ban co muon xoa: ", "canh bao", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                String query = "delete from SANPHAM  where MASP = '" + Masp + "' ";
                fn.SetDataTable(query);
                loadData();
            }
            else { }

        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            FormChiTietSp form = new FormChiTietSp();
            form.Show();
            this.Close();
        }
    }
}