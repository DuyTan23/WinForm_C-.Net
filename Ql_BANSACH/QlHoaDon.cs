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
    public partial class QlHoaDon : Form
    {
        function fn = new function();
         public static String mahd;
        public QlHoaDon()
        {
            InitializeComponent();
        }

        private void QlHoaDon_Load(object sender, EventArgs e)
        {
            this.Location = new Point(519, 199);
            loadData();
        }
        public void loadData()
        {
            String query = "select * from HOADON";
            DataHoaDon.DataSource = fn.GetDataTable(query);
        }

        private void DataHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = DataHoaDon.CurrentRow.Index;
            txtMahd.Text = DataHoaDon.Rows[i].Cells[0].Value.ToString();
            txtNgayLap.Text = DataHoaDon.Rows[i].Cells[3].Value.ToString();
            mahd = txtMahd.Text;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            FormChiTietHd formCapNhatHoaDon = new FormChiTietHd();
            formCapNhatHoaDon.ShowDialog();
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
                String query = "delete from HOADON  where MAHD = '" + mahd + "' ";
                String query1 = "delete from CTHOADON  where MAHD = '" + mahd + "' ";
                fn.SetDataTable(query);
                loadData();
            }
            else { }

        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            FormChiTietHd formChiTietHd = new FormChiTietHd();
            formChiTietHd.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1.dashboard.Enabled = true;
            this.Close();
        }
    }
}
