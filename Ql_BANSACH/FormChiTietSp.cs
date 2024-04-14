using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDevHtmlRenderer.Adapters;

namespace Ql_BANSACH
{
    public partial class FormChiTietSp : Form
    {
        String stringConnection = @"Data Source=LAPTOP-2J7ABSNE\SQLEXPRESS;Initial Catalog=QL_BANSACH;Integrated Security=True";
        SqlConnection conn = null;
        SqlCommand cmd = null;
        function fn = new function();
        bool KT = false;
        public FormChiTietSp()
        {
            InitializeComponent();
        }

        private void FormChiTietSp_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(stringConnection);
            this.Location = new Point(519, 199);
            
            loadComBo();
            loadTxt();
            Khoatxt();
        }
        public void loadTxt()
        {
            String query = "select * from SANPHAM where MASP = '" + QLSanPham.Masp + "'";
            if(fn.GetDataTable(query).Rows.Count > 0)
            {
                DataRow row = fn.GetDataTable(query).Rows[0];
                txtMaSp.Text = row["MASP"].ToString();
                txtTenSp.Text = row["TENSP"].ToString();
                txtDonVi.Text = row["DONVITINH"].ToString() ;
                txtDonGiaNhap.Text = row["DONGIANHAP"].ToString();
                txtDonGiaXuat.Text = row["DONGIAXUAT"].ToString();
                
                byte[] imageData = (byte[])row["HINHANH"];
                ShowImage(imageData);
                List<LoaiSp> list = new List<LoaiSp>();
                list = DanhSachSp();
                int dem = 0;
                for(int i = 0; i < list.Count; i++)
                {
                    if (list[i].maLoai.Equals(row["MALOAI"].ToString()))
                    {
                        dem = i;
                    }
                }

               comBoLoai.SelectedIndex = dem;

            }
        }
        public void loadComBo()
        {
            String query = "select * from LOAISP";
            DataTable dataTable = new DataTable();
            dataTable = fn.GetDataTable(query);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    LoaiSp loaiSp = new LoaiSp();
                    loaiSp.maLoai = row["MALOAI"].ToString();
                    loaiSp.tenLoai = row["TENLOAI"].ToString();
                    comBoLoai.Items.Add(loaiSp);
                }
            }
        }

        //Tạo 1 danh sách lưu các giá trị trong comboBox
        private List<LoaiSp> DanhSachSp()
        {
            List<LoaiSp> loaiSps = new List<LoaiSp>();
            String query = "select * from LOAISP";
            DataTable dataTable = new DataTable();
            dataTable = fn.GetDataTable(query);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    LoaiSp loaiSp = new LoaiSp();
                    loaiSp.maLoai = row["MALOAI"].ToString();
                    loaiSp.tenLoai = row["TENLOAI"].ToString();
                    loaiSps.Add(loaiSp);
                }
            }
            return loaiSps;
        }
        private void ShowImage(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);
                   guna2PictureBox1.Image = image;
                    guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                // Nếu không có dữ liệu hình ảnh, xóa hình ảnh trên PictureBox
                guna2PictureBox1.Image = null;
            }
        }

        public void Khoatxt()
        {
            txtMaSp.Enabled = false;
            txtTenSp.Enabled = false;
            txtDonVi.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            txtDonGiaXuat.Enabled = false;
            comBoLoai.Enabled = false;
            guna2PictureBox1.Enabled = false;
        }

        public void khoiTao()
        {
            txtMaSp.Text = null;
            txtTenSp.Text = null;
            txtDonVi.Text = null;
            txtDonGiaNhap.Text = null;
            txtDonGiaXuat.Text = null;
            comBoLoai.Text = null;
            guna2PictureBox1.Image = null;
        }

        public void motxt()
        {
            txtMaSp.Enabled = true;
            txtTenSp.Enabled = true;
            txtDonVi.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtDonGiaXuat.Enabled = true;
            comBoLoai.Enabled = true;
            guna2PictureBox1.Enabled = true;
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                guna2PictureBox1.Image = Image.FromFile(ofd.FileName);
                guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                this.Text = ofd.FileName;
                KT = true;
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            motxt();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            byte[] bytes = ImageToByteArray(guna2PictureBox1.Image);
            float dongianhap = float.Parse(txtDonGiaNhap.Text);
            float dongiaxuat = float.Parse(txtDonGiaXuat.Text);
            LoaiSp loaiSp = comBoLoai.SelectedItem as LoaiSp;
            cmd = conn.CreateCommand();
            //cmd.CommandText = "update  SANPHAM set MASP = @masp   TENSP = @tensp, DONVITINH = @donvi, DONGIANHAP = @dongianhap, DONGIAXUAT = @dongiaxuat, LOAISP = @loai, HINHANH = @hinh where MASP = '"+QLSanPham.Masp+"'";
            cmd.CommandText = "update  SANPHAM set TENSP = '"+ txtTenSp.Text + "' , DONVITINH = '"+ txtDonVi.Text + "', DONGIANHAP = "+dongianhap+ ", DONGIAXUAT = "+dongiaxuat+",MALOAI = '"+loaiSp.maLoai+"' where MASP =  '" + QLSanPham.Masp + "'";
            if (KT)
            {
                cmd.CommandText = "update  SANPHAM set HINHANH = @hinh where MASP =  '" + QLSanPham.Masp + "'";
            }
            /*cmd.Parameters.Add("@tensp", txtTenSp.Text);
            cmd.Parameters.Add("@donvi", txtDonVi.Text);
            cmd.Parameters.Add("@dongianhap", dongianhap);
            cmd.Parameters.Add("@dongiaxuat", dongiaxuat);*/
            cmd.Parameters.Add("@hinh", bytes);
            cmd.ExecuteNonQuery();
            DialogResult dialog = MessageBox.Show("Cập nhật thành công: ", "canh bao");
            loadTxt();
        }
        byte[] ImageToByteArray(Image img)
        {
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return memoryStream.ToArray();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            QLSanPham qL = new QLSanPham();
            qL.Show();
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
                String query = "delete from SANPHAM  where MASP = '" + QLSanPham.Masp + "' ";
                fn.SetDataTable(query);
                this.Close();
            }
            else { }

        }
    }
}
