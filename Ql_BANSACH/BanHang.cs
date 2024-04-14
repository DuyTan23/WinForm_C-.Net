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

namespace Ql_BANSACH
{
    public partial class BanHang : Form
    {
        public List<ChiTietHd> chiTietHds = new List<ChiTietHd>();
        int chonTimKh = -1;
        int chonTimSp = -1;
        function fn = new function();
        public List<SanPham> sanPhamS = new List<SanPham>();

        float thanhTien;
        
        public BanHang()
        {
            InitializeComponent();
        }

        private void BanHang_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519, 199);
            loadtable();
            txtMahd.Enabled = false;
            txtNgayLap.Text = DateTime.Today.ToString();
        }
        public void loadtable()
        {
            khoatxtKh();
            comboTimKH.SelectedIndex = 0;
            txtTimKh.PlaceholderText = "Nhập mã san pham cần tìm";
            String query = "select * from SANPHAM";
            DataTable dt = new DataTable();
            dt = fn.GetDataTable(query);

            if(dt.Rows.Count > 1) 
            {
                foreach(DataRow row in dt.Rows)
                {
                    SanPham pham = new SanPham();
                    pham.maSp = row["MASP"].ToString();
                    pham.tenSp = row["TENSP"].ToString();
                    pham.image = (byte[])(row["HINHANH"]);
                    pham.giaSp =float.Parse(row["DONGIAXUAT"].ToString());
                    sanPhamS.Add(pham);
                    itemSanPham(pham);
                }
            }
        }

        public void itemSanPham(SanPham sanpham)
        {
            //Create panel
            Panel panel;
            panel = new Panel();
            panel.Name = String.Format("PnlSanPham{0}", sanpham.maSp);
            panel.BackColor = Color.White;
            panel.Size = new Size(125, 185);
            panel.Margin = new Padding(10);
            panel.Tag = sanpham.maSp;
            //panel.Click += Panel_Click;

            //create pictureBox
            PictureBox picBox;
            picBox = new PictureBox();
            picBox.Name = String.Format("PbSanPham{0}", sanpham.maSp);
            picBox.Size = new Size(100, 148);
            picBox.Location = new Point(12, 10);
            picBox.SizeMode = PictureBoxSizeMode.Zoom;
            picBox.Tag = sanpham.maSp;
            byte[] imageData = (byte[])sanpham.image;
            picBox.Click += Picbox_click;
            ShowImage(imageData, picBox);

            //Create title label
            Label labelTitle;
            labelTitle = new Label();
            labelTitle.Name = String.Format("LblSanPham{0}", sanpham.maSp);
            labelTitle.Text = sanpham.tenSp;
            labelTitle.Location = new Point(12, 165);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelTitle.AutoSize = true;
            labelTitle.Tag = sanpham.maSp;

            //Create title label
            Label labelGia;
            labelGia = new Label();
            labelGia.Name = String.Format("LblSanPham{0}", sanpham.maSp);
            labelGia.Text = sanpham.giaSp.ToString();
            labelGia.Location = new Point(12, 185);
            labelGia.ForeColor = Color.Black;
            labelGia.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelGia.AutoSize = true;
            labelGia.Tag = sanpham.maSp;

            //add item
            panel.Controls.Add(labelGia);
            panel.Controls.Add(labelTitle);
            panel.Controls.Add(picBox);

            //add panel
            flpTable.Controls.Add(panel);
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            if (clickedPanel != null)
            {
                MessageBox.Show("Bạn đã click vào panel!");
            }
        }


        private void Picbox_click(object sender, EventArgs e)
        {
            List<ChiTietHd> chiTietHds1 = new List<ChiTietHd>();
            ChiTietHd chiTietHd = new ChiTietHd();
            PictureBox clickedPicbox = sender as PictureBox;
            if (clickedPicbox != null)
            {
                MessageBox.Show("Bạn đã click vào PictureBox có tên: " + clickedPicbox.Tag);
                String query = "select * from SANPHAM where MASP = '" + clickedPicbox.Tag + "'";
                DataTable dt = new DataTable();
                dt = fn.GetDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SanPham pham = new SanPham();
                        String masp = row["MASP"].ToString();
                        String tensp = row["TENSP"].ToString();
                        float giasp = float.Parse(row["DONGIAXUAT"].ToString());
                        int i = kt(clickedPicbox.Tag.ToString());
                        if (i != -1) // click vào item đã có sẵn
                        {
                            this.chiTietHds[i].SOLUONG = this.chiTietHds[i].SOLUONG + 1;
                            thanhTien = thanhTien + chiTietHds[i].GIASP;
                            txtThanhTien.Text = thanhTien.ToString();
                            ClearFlowLayoutTable(flpChiTiet);
                            for (int j = 0; j < chiTietHds.Count; j++)
                            {
                                loadItem_chiTietHd(chiTietHds[j]);
                                
                            }
                        }
                        else
                        {
                            chiTietHd.MAHD = txtMahd.Text;
                            chiTietHd.MASP = masp;
                            chiTietHd.TENSP = tensp;
                            chiTietHd.GIASP = giasp;
                            chiTietHd.SOLUONG = 1;
                            thanhTien = thanhTien + (giasp * chiTietHd.SOLUONG);
                            txtThanhTien.Text = thanhTien.ToString();
                            chiTietHds.Add(chiTietHd);
                            chiTietHds1.Add(chiTietHd);
                            loadItem_chiTietHd(chiTietHd);
                        }
                    }
                }  
            }
        }
        public int kt(String masp)
        {
            int kt = -1;
            for (int i = 0; i < chiTietHds.Count; i++)
            {
                if (masp.Equals(chiTietHds[i].MASP)) // click vào item đã có sẵn
                {
                    kt = i;
                }
            }
            return kt;
        }
        //hàm  xóa tất cả các panel trong flpChiTiet
        private void ClearFlowLayoutTable(FlowLayoutPanel flowLayoutPanel)
        {
            flowLayoutPanel.Controls.Clear();
        }

    //hàm xóa dòng sản phẩm khi ấn delete trong floLayout
    private List<ChiTietHd> deleteDongHd(String masp)
        {
            bool kt = false;
            int dem = -1;
            // Tìm sp có masp tương ứng
            ChiTietHd chiTietHdDelete = chiTietHds.Find(s => s.MASP == masp);
            for(int i = 0; i < this.chiTietHds.Count; i++)
            {
                if (this.chiTietHds[i].MASP.Equals(masp))
                {
                    kt = true;
                   dem = i;
                }
            }
            if (kt)
            {
                thanhTien = thanhTien - (this.chiTietHds[dem].GIASP * this.chiTietHds[dem].SOLUONG);
                txtThanhTien.Text = thanhTien.ToString();
                this.chiTietHds.Remove(this.chiTietHds[dem]);
            }
            
            return this.chiTietHds;
        }

        public void loadItem_chiTietHd(ChiTietHd chiTietHd)
        {
                item_chitietHd(chiTietHd.MASP, chiTietHd.TENSP, chiTietHd.GIASP, chiTietHd.SOLUONG);
        }
            private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ShowImage(byte[] imageData, PictureBox picture)
        {
            if (imageData != null && imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);
                    picture.Image = image;
                    picture.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                // Nếu không có dữ liệu hình ảnh, xóa hình ảnh trên PictureBox
                picture.Image = null;
            }
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
            string mahd = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return mahd;
        }

        static bool IsIDUnique(string mahd, function fn)
        {
            // Truy vấn kiểm tra tính duy nhất của mã sinh viên trong CSDL
            string query = "SELECT COUNT(*) FROM HOADON WHERE MAHD = @mahd";
            using (SqlCommand command = fn.GetCommand(query))
            {
                command.Parameters.AddWithValue("@mahd", mahd);
                int count = (int)command.ExecuteScalar();
                return count == 0;
            }
        }

        static string GenerateUniqueID(function fn)
        {
            string mahd;
            do
            {
                mahd = "HD_"+GenerateRandomID(7);
            } while (!IsIDUnique(mahd, fn));
            return mahd;
        }

        private void comboTimKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            chonTimKh = comboTimKH.SelectedIndex;
            switch (chonTimKh)
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
            switch (chonTimKh)
            {
                case 0:
                    query = "select * from KHACHHANG where MAKH = '" + txtTimKh.Text + "'";
                    break;
                case 1:
                    query = "select * from KHACHHANG where TENKH like '" + txtTimKh.Text + "'";
                    break;
                case 2:
                    query = "select * from KHACHHANG where SODT = '" + txtTimKh.Text + "'";
                    break;
            }
            SqlCommand cmd = fn.GetCommand(query);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())//đã tìm thấy
            {
                DialogResult dialog = MessageBox.Show("Đã tìm thấy khách hàng: ", "canh bao");
                if (fn.GetDataTable(query).Rows.Count > 0)
                {
                    DataRow row = fn.GetDataTable(query).Rows[0];
                    txtMakh.Text = row["MAKH"].ToString();
                }
                else
                {
                    DialogResult dialog1 = MessageBox.Show("Không tìm thấy khách hàng: ", "canh bao");

                }
            }
        }

        private void comboTimKH_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            chonTimKh = comboTimKH.SelectedIndex;
            switch (chonTimKh)
            {
                case 0:
                    txtTimKh.PlaceholderText = "Nhập mã khách hàng cần tìm";
                    break;
                case 1:
                    txtTimKh.PlaceholderText = "Nhập tên khách hàng cần tìm";
                    break;
                case 2:
                    txtTimKh.PlaceholderText = "Nhập sđt khách hàng cần tìm";
                    break;
            }
        }

        private void btnClearKh_Click(object sender, EventArgs e)
        {
            txtMakh.Text = null;
            for(int i = 0; i< chiTietHds.Count; i++)
            {
                loadItem_chiTietHd(chiTietHds[i]);
            }
        }

       public void khoatxtKh()
        {
            txtMakh.Enabled = false;
            txtNgayLap.Enabled = false;
            txtTimKh.Enabled = false;
            comboTimKH.Enabled = false;
        }

        private void btnThemHd_Click(object sender, EventArgs e)
        {
            motxtKh();
            txtMahd.Text = GenerateUniqueID(fn);
        }
        public void motxtKh()
        {
            txtTimKh.Enabled = true;
            comboTimKH.Enabled = true;
        }

        public void item_chitietHd(String masp, String tensp, float giasp, int soluong)
        {
            //Create panel
            Panel panel;
            panel = new Panel();
            panel.Name = String.Format("PnlSanPham{0}", masp);
            panel.BackColor = Color.White;
            panel.Size = new Size(470, 41);
            panel.Margin = new Padding(10);
            panel.Tag = masp;

            //Create lable ma sp
            Label labelMasp;
            labelMasp = new Label();
            labelMasp.Name = String.Format("LblSanPham{0}", masp);
            labelMasp.Text = masp;
            labelMasp.Location = new Point(5, 13);
            labelMasp.BackColor = Color.FromArgb(250, 202, 109);
            labelMasp.ForeColor = Color.Black;
            labelMasp.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelMasp.AutoSize = false;
            labelMasp.Size = new Size(80, 19);
            labelMasp.Tag = masp;

            //Create lable name sp
            Label labelTensp;
            labelTensp = new Label();
            labelTensp.Name = String.Format("LblSanPham{0}", masp);
            labelTensp.Text = tensp;

            labelTensp.BackColor = Color.FromArgb(250, 202, 109);
            labelTensp.Location = new Point(90,13);
            labelTensp.ForeColor = Color.Black;
            labelTensp.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelTensp.AutoSize = false;
            labelTensp.Size = new Size(135, 19);
            labelTensp.Tag = masp;

            //Create title label
            Label labelGia;
            labelGia = new Label();
            labelGia.BackColor = Color.FromArgb(250, 202, 109);
            labelGia.Name = String.Format("LblSanPham{0}", masp);
            labelGia.Text = giasp.ToString();
            labelGia.Location = new Point(230 ,13);
            labelGia.ForeColor = Color.Black;
            labelGia.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelGia.AutoSize = false;
            labelGia.Size = new Size(100, 19);
            labelGia.Tag = masp;

            Guna2NumericUpDown numericUpDown = new Guna2NumericUpDown();
            numericUpDown.Value =   soluong;
            numericUpDown.Location = new Point(335, 13);
            numericUpDown.ForeColor = Color.Black;
            numericUpDown.Font = new Font(this.Font.FontFamily, 7.5f, FontStyle.Regular);
            numericUpDown.AutoSize = false;
            numericUpDown.Size = new Size(50,18);
            numericUpDown.Tag = masp;
            numericUpDown.ValueChanged += numericUpDown_changed;

            Guna2Button buttonDelete = new Guna2Button();
            buttonDelete.Text = "Xóa";
            buttonDelete.Location = new Point(390, 13);
            buttonDelete.ForeColor = Color.Black;
            buttonDelete.AutoSize = false;
            buttonDelete.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            buttonDelete.Size = new Size(70, 19);
            buttonDelete.Click += btnDelete_click;
            buttonDelete.Tag = masp;


            //add item
            panel.Controls.Add(labelGia);
            panel.Controls.Add(labelMasp);
            panel.Controls.Add(labelTensp);
            panel.Controls.Add(numericUpDown);
            panel.Controls.Add(buttonDelete);
            //add panel
            flpChiTiet.Controls.Add(panel);
        }

        private void btnDelete_click(object sender, EventArgs e)
        {
            List<ChiTietHd> chiTietHds1 = new List<ChiTietHd>();
            chiTietHds1 = chiTietHds;
            Guna2Button btnDelete = sender as Guna2Button;
            ClearFlowLayoutTable(flpChiTiet);
            for (int i = 0; i < deleteDongHd(btnDelete.Tag.ToString()).Count; i++)
            {
                loadItem_chiTietHd(deleteDongHd(btnDelete.Tag.ToString())[i]);
            }
        }

        private void numericUpDown_changed(object sender, EventArgs e)
        {
            Guna2NumericUpDown numericUpDown = sender as Guna2NumericUpDown;
            //ChiTietHd chiTietHdDelete = chiTietHds.Find(s => s.MASP == numericUpDown.Tag.ToString());
            for (int i = 0; i < this.chiTietHds.Count; i++)
            {
                if (this.chiTietHds[i].MASP.Equals(numericUpDown.Tag.ToString()))
                {
                    if (this.chiTietHds[i].SOLUONG <= (int)numericUpDown.Value)//tang so luong
                    {
                        this.chiTietHds[i].SOLUONG = (int)numericUpDown.Value;
                        thanhTien = thanhTien + (this.chiTietHds[i].GIASP );
                    }
                    else //giam so luong
                    {
                        this.chiTietHds[i].SOLUONG = (int)numericUpDown.Value;
                        thanhTien = thanhTien - (this.chiTietHds[i].GIASP);
                    }
                    txtThanhTien.Text = thanhTien.ToString();
                }
            }
        }

        private void guna2ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnTimSp_Click(object sender, EventArgs e)
        {

            String query = "";
            switch (chonTimSp)
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
            SqlCommand cmd = fn.GetCommand(query);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())//đã tìm thấy
            {
                DialogResult dialog = MessageBox.Show("Đã tìm thấy san pham: ", "canh bao");
                ClearFlowLayoutTable(flpTable);
                SqlCommand cmd1 = fn.GetCommand(query);
                DataTable dt = new DataTable();
                dt = fn.GetDataTable(query);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (fn.GetDataTable(query).Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SanPham pham = new SanPham();
                        pham.maSp = row["MASP"].ToString();
                        pham.tenSp = row["TENSP"].ToString();
                        pham.image = (byte[])(row["HINHANH"]);
                        pham.giaSp = float.Parse(row["DONGIAXUAT"].ToString());
                        sanPhamS.Add(pham);
                        itemSanPham(pham);
                    }
                }
            }
                else
                {
                    DialogResult dialog2 = MessageBox.Show("Không tìm thấy khách hàng: ", "canh bao");

                }
        }

        private void comboTimSp_SelectedIndexChanged(object sender, EventArgs e)
        {
            chonTimSp = comboTimSp.SelectedIndex;
            switch (chonTimSp)
            {
                case 0:
                    txtTimKh.PlaceholderText = "Nhập mã khách hàng cần tìm";
                    break;
                case 1:
                    txtTimKh.PlaceholderText = "Nhập tên khách hàng cần tìm";
                    break;
                case 2:
                    txtTimKh.PlaceholderText = "Nhập sđt khách hàng cần tìm";
                    break;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            String query = "select * from SANPHAM";
            DataTable dt = new DataTable();
            dt = fn.GetDataTable(query);

            if (dt.Rows.Count > 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    SanPham pham = new SanPham();
                    pham.maSp = row["MASP"].ToString();
                    pham.tenSp = row["TENSP"].ToString();
                    pham.image = (byte[])(row["HINHANH"]);
                    pham.giaSp = float.Parse(row["DONGIAXUAT"].ToString());
                    sanPhamS.Add(pham);
                    itemSanPham(pham);
                }
            }
        }

        private void btnXacNhanTaoHd_Click(object sender, EventArgs e)
        {
            string sqlQuery = "INSERT INTO HOADON (MAHD, MANV, MAKH, NGAYLAPHD) VALUES ('" + txtMahd.Text + "', '"+Form1.manv+"', '" + txtMakh.Text + "', '" + DateTime.Today.ToString("yyyy-MM-dd") + "')";
            fn.SetDataTable(sqlQuery);
            for (int i = 0; i< chiTietHds.Count; i++)
            {
                string sqlQuery1 = "INSERT INTO CTHOADON (MAHD, MASP, SOLUONG) VALUES ('" + txtMahd.Text + "', '" + chiTietHds[i].MASP +"', " + chiTietHds[i].SOLUONG + ")";
                fn.SetDataTable(sqlQuery1);
            }

            DialogResult dialog = MessageBox.Show("Hóa đơn đã tạo thành công. Bạn có muốn in háo đơn: ", "Thông báo", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                MessageBox.Show("Chưa kết nối với máy in");
            }
            else { 
                
            }
        }
    }
}
