using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DanhSachThi
    {       
        public string hoTen { get; set; }      
        public string maSo { get; set; }
        public string ngaySinh { get; set; }
        public string phai { set; get; }
        public string mail { set; get; }
        public int trangThai { get; set; }
    }
}