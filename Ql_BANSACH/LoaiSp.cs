using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ql_BANSACH
{
    internal class LoaiSp
    {
        public string maLoai { get; set; }
        public string tenLoai { get; set; }
        public override string ToString()
        {
            return tenLoai + "---" + maLoai; // Hiển thị tên chat lieu trong ComboBox
        }
    }
}
