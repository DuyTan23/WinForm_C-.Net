using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ql_BANSACH
{
    public class ChiTietHd
    {
        public String MAHD { get; set; }
        public String MASP { get; set;}
        public String TENSP {  get; set;}
        public float GIASP { get; set;}
        public int SOLUONG { get; set; }

        public ChiTietHd()
        {
            MAHD = string.Empty;
            MASP = string.Empty;
            TENSP = string.Empty;
            GIASP = 0;
            SOLUONG = 0;
        }
        public ChiTietHd(String mahd, String masp, string tENSP, float gIASP ,int soluong)
        {
            MAHD = mahd;
            MASP = masp;
            SOLUONG = soluong;
            TENSP = tENSP;
            GIASP = gIASP;
        }
    }
}
