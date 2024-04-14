using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ql_BANSACH
{
    public class SanPham
    {
        public String maSp {  get; set; }
        public String tenSp { get; set; }
        public  float giaSp { get; set; }
        public  byte[] image { get; set; }

        public SanPham()
        {
            maSp = string.Empty;
            tenSp = string.Empty;
            image = null ;
            giaSp = 0 ;
        }
        public SanPham(String masp, String tensp , float giasp, byte[] imagesp)
        {
            maSp = masp;
            tenSp = tensp;
            giaSp = giasp;
            image = imagesp;
        }
    }

}
