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
    public partial class QLKhachHang : Form
    {
        
        Boolean luu = true;
        function fn = new function();
        String MAKH = "";
        int chonTim = -1;
        public QLKhachHang()
        {
            InitializeComponent();
        }

        private void QLKhachHang_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519,199);
            loadData();
            comboTimKH.SelectedIndex = 0;
            txtTimKh.PlaceholderText = "Nhập mã khách hàng cần tìm";

        }
        public void loadData()
        {
            String query = "select * from KHACHHANG";
            dataKhachHang.DataSource = fn.GetDataTable(query);
            khoaTxt();
            
        }

        private void dataKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            khoaTxt();
            int i = dataKhachHang.CurrentRow.Index;
            txtMakh.Text = dataKhachHang.Rows[i].Cells[0].Value.ToString();
            txtTenKh.Text = dataKhachHang.Rows[i].Cells[1].Value.ToString();
            txtDiaChi.Text = dataKhachHang.Rows[i].Cells[2].Value.ToString();
            txtSoDt.Text =  dataKhachHang.Rows[i].Cells[3].Value.ToString();
            MAKH = dataKhachHang.Rows[i].Cells[0].Value.ToString(); ;
        }
        public void khoaTxt()
        {
            txtMakh.Enabled = false;
            txtTenKh.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSoDt.Enabled = false;
        }
        public void moTxt()
        {
            txtTenKh.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSoDt.Enabled = true;
        }
        public void khoiTaoTxt()
        {
            txtTenKh.Text = null;
            txtDiaChi.Text = null;
            txtSoDt.Text = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            luu = true;
            moTxt();
            txtMakh.Text = GenerateUniqueID(fn);
            khoiTaoTxt();

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            luu = false;
            moTxt();
            txtMakh.Enabled=false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(luu == true)//nút thêm
            {
                String makh = txtMakh.Text;
                String query = "select * from KHACHHANG WHERE MAKH = '" + makh + "'";
                SqlCommand cmd = fn.GetCommand(query);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())//Mã khách hàng đã được dùng
                {
                    DialogResult dialog = MessageBox.Show("Mã khách hàng đã được dùng vui lòng nhập lại", "canh bao", MessageBoxButtons.OK);
                    txtMakh.Text = null;
                    txtMakh.Focus();
                }
                else
                {
                    String query1 = "insert into KHACHHANG Values('" + txtMakh.Text + "',N'" + txtTenKh.Text + "',N'" + txtDiaChi.Text + "',N'" + txtSoDt.Text + "')";
                    fn.SetDataTable(query1);
                    loadData();
                }
            }
            else
            {
                String query = "update KHACHHANG set MAKH = '"+txtMakh.Text+ "', TENKH = N'"+txtTenKh.Text+ "', DIACHI = N'"+ txtDiaChi.Text + "', SODT = '"+ txtSoDt.Text + "' where MAKH = '"+MAKH+"' ";
                fn.SetDataTable(query);
                loadData();
            }
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
                String query = "delete from KHACHHANG  where MAKH = '" + MAKH + "' ";
                fn.SetDataTable(query);
                loadData();
            }
            else { }
            
        }

        private void comboTimKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            chonTim = comboTimKH.SelectedIndex;
            switch (chonTim)
            {
                case 0:
                    txtTimKh.PlaceholderText = "Nhập mã khách hàng cần tìm";
                    break;
                case 1:
                    txtTimKh.PlaceholderText = "Nhập tên khách hàng cần tìm";
                    break;
                case 2:
                    txtTimKh.PlaceholderText = "Nhập địa chỉ khách hàng cần tìm";
                    break;
                case 3:
                    txtTimKh.PlaceholderText = "Nhập sđt khách hàng cần tìm";
                    break;
            }

        }

        private void btnTimKh_Click(object sender, EventArgs e)
        {
            String query = "";
            switch (chonTim)
            {
                case 0:
                    query = "select * from KHACHHANG where MAKH = '" + txtTimKh.Text + "'";
                    break;
                case 1:
                    query = "select * from KHACHHANG where TENKH like '"+txtTimKh.Text+"'";
                    break;
                case 2:
                    query = "select * from KHACHHANG where DIACHI like '" + txtTimKh.Text + "'";
                    break;
                case 3:
                    query = "select * from KHACHHANG where SODT = '" + txtTimKh.Text + "'";
                    break;
            }
            SqlCommand cmd = fn.GetCommand(query);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())//đã tìm thấy
            {
                DialogResult dialog = MessageBox.Show("Đã tìm thấy khách hàng: ", "canh bao");
                dataKhachHang.DataSource = fn.GetDataTable(query);
                int i = dataKhachHang.CurrentRow.Index;
                txtMakh.Text = dataKhachHang.Rows[i].Cells[0].Value.ToString();
                txtTenKh.Text = dataKhachHang.Rows[i].Cells[1].Value.ToString();
                txtDiaChi.Text = dataKhachHang.Rows[i].Cells[2].Value.ToString();
                txtSoDt.Text = dataKhachHang.Rows[i].Cells[3].Value.ToString();
                MAKH = dataKhachHang.Rows[i].Cells[0].Value.ToString(); ;
                khoaTxt();
            }
            else
            {
                DialogResult dialog = MessageBox.Show("Không tìm thấy khách hàng: ", "canh bao");
                dataKhachHang.DataSource = null;
                khoiTaoTxt();
                khoaTxt();
            }
           
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            loadData();
            khoiTaoTxt();
            khoaTxt();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1.dashboard.Enabled = true;
            this.Close();
        }

        static string GenerateRandomID(int length)
        {
            // Chuỗi chứa tất cả các ký tự có thể xuất hiện trong mã sinh viên
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // Một đối tượng Random để sinh ra các số ngẫu nhiên
            Random random = new Random();
            // Tạo mã sinh viên ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi 'chars'
            string makh = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return makh;
        }

        static bool IsIDUnique(string makh, function fn)
        {
            // Truy vấn kiểm tra tính duy nhất của mã sinh viên trong CSDL
            string query = "SELECT COUNT(*) FROM KHACHHANG WHERE MAKH = @makh";
            using (SqlCommand command = fn.GetCommand(query))
            {
                command.Parameters.AddWithValue("@makh", makh);
                int count = (int)command.ExecuteScalar();
                return count == 0;
            }
        }

        static string GenerateUniqueID(function fn)
        {
            string makh;
            do
            {
                makh = "KH_" + GenerateRandomID(7);
            } while (!IsIDUnique(makh, fn));
            return makh;
        }
    }
}
