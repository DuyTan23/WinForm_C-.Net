using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Ql_BANSACH
{
    public partial class FormThemSp : Form
    {
        String stringConnection = @"Data Source=LAPTOP-2J7ABSNE\SQLEXPRESS;Initial Catalog=QL_BANSACH;Integrated Security=True";
        function fn = new function();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        public FormThemSp()
        {
            InitializeComponent();
        }

        

        private void FormThemSp_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519, 199);
            conn = new SqlConnection(stringConnection);
            txtMaSp.Enabled = false;
            txtMaSp.Text = GenerateUniqueID(fn);
            loadComBo();
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                btnPicture.Image = Image.FromFile(ofd.FileName);
                btnPicture.SizeMode = PictureBoxSizeMode.Zoom;
                this.Text = ofd.FileName;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            LoaiSp loaiSp = comBoLoai.SelectedItem as LoaiSp;
            byte[] bytes = ImageToByteArray(btnPicture.Image);
            float dongianhap = float.Parse(txtGiaNhap.Text);
            float dongiaxuat = float.Parse(txtGiaXuat.Text);
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "insert into SANPHAM Values(@masp, @tensp, @donvi, @dongianhap, @dongiaxuat, @loai, @hinh)";
            cmd.Parameters.Add("@masp", txtMaSp.Text);
            cmd.Parameters.Add("@tensp",txtTenSp.Text);
            cmd.Parameters.Add("@donvi", txtDonVi.Text);
            cmd.Parameters.Add("@dongianhap", dongianhap);
            cmd.Parameters.Add("@dongiaxuat", dongiaxuat);
            cmd.Parameters.Add("@loai", loaiSp.maLoai);
            cmd.Parameters.Add("@hinh", bytes);
            cmd.ExecuteNonQuery();
            this.Close();
            QLSanPham qL = new QLSanPham();
            qL.Show();
            
        }
        //hàm chuyển hình sang byte
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

        static string GenerateRandomID(int length)
        {
            // Chuỗi chứa tất cả các ký tự có thể xuất hiện trong mã sinh viên
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // Một đối tượng Random để sinh ra các số ngẫu nhiên
            Random random = new Random();
            // Tạo mã sinh viên ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi 'chars'
            string masp = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return masp;
        }

        static bool IsIDUnique(string masp, function fn)
        {
            // Truy vấn kiểm tra tính duy nhất của mã sinh viên trong CSDL
            string query = "SELECT COUNT(*) FROM SANPHAM WHERE MASP = @masp";
            using (SqlCommand command = fn.GetCommand(query))
            {
                command.Parameters.AddWithValue("@masp", masp);
                int count = (int)command.ExecuteScalar();
                return count == 0;
            }
        }

        static string GenerateUniqueID(function fn)
        {
            string masp;
            do
            {
                masp = "SP_" + GenerateRandomID(7);
            } while (!IsIDUnique(masp, fn));
            return masp;
        }
        public void loadComBo()
        {
            String query = "select * from LOAISP";
            DataTable dataTable = new DataTable();
            dataTable = fn.GetDataTable(query);
            if(dataTable.Rows.Count > 0)
            {
                foreach(DataRow row in dataTable.Rows) 
                { 
                    LoaiSp loaiSp = new LoaiSp();
                    loaiSp.maLoai = row["MALOAI"].ToString();
                    loaiSp.tenLoai = row["TENLOAI"].ToString();
                    comBoLoai.Items.Add(loaiSp);
                }
            }
        }
    }
}
