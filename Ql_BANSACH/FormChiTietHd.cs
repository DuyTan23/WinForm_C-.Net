using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ql_BANSACH
{
    public partial class FormChiTietHd : Form
    {
        function fn = new function();
        public FormChiTietHd()
        {
            InitializeComponent();
        }

        private void FormCapNhatHoaDon_Load(object sender, EventArgs e)
        {
            this.Location = new Point(659,245);
            loadTxt();
            loadData();
        }
        public void loadTxt()
        {
            String query = "SELECT HOADON.MAHD, NHANVIEN.TENNV, HOADON.NGAYLAPHD FROM HOADON, NHANVIEN WHERE HOADON.MANV = NHANVIEN.MANV AND HOADON.MAHD = '" + QlHoaDon.mahd+"'";
            if (fn.GetDataTable(query).Rows.Count > 0)
            {
                DataRow row = fn.GetDataTable(query).Rows[0];
                labelMahd.Text = row["MAHD"].ToString();
                labelTennv.Text = row["TENNV"].ToString();
                labelNgayLap.Text = row["NGAYLAPHD"].ToString();
            }
        }
        public void loadData()
        {
            String query = "SELECT SANPHAM.MASP, SANPHAM.TENSP, CTHOADON.SOLUONG, CTHOADON.SOLUONG*SANPHAM.DONGIAXUAT AS THANHTIEN FROM SANPHAM, CTHOADON WHERE SANPHAM.MASP = CTHOADON.MASP AND CTHOADON.MAHD = '"+QlHoaDon.mahd+"'";
            dataCTHD.DataSource = fn.GetDataTable(query);
            String query1 = "SELECT sum(CTHOADON.SOLUONG*SANPHAM.DONGIAXUAT) AS TONGTIEN FROM SANPHAM, CTHOADON WHERE SANPHAM.MASP = CTHOADON.MASP AND CTHOADON.MAHD = '"+QlHoaDon.mahd+"'";
            DataRow row = fn.GetDataTable(query1).Rows[0];
            labelTongTien.Text = row["TONGTIEN"].ToString();
        }

        private void btnInHd_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Bạn có muốn in hóa đơn: ", "canh bao", MessageBoxButtons.YesNo);
            if(dialog == DialogResult.Yes)
            {
                MessageBox.Show("Chưa Kết Nối Máy in!");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
