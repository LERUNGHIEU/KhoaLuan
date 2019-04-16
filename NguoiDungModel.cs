using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class NguoiDungModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public int SoLuong()
        {
            return db.tbl_thanhvien.Count();
        }
        // get danh sách 
        public List<tbl_thanhvien> DanhSach_ThanhVien()
        {
            return db.tbl_thanhvien.ToList();
        }
        // tạo mã thành viên
        public string TaoMaThanhVien()
        {
            var linq = db.tbl_thanhvien.OrderByDescending(ch => ch.MaGiangVien).FirstOrDefault(ch => ch.Quyen == 0);
            if (linq != null)
                return (Convert.ToInt32(linq.MaGiangVien) + 1).ToString();

            return 1.ToString();
        }        
        // thêm thành viên
        public bool DangKy(string maSo, string hoTen, string ngaySinh, string hinhAnh, string email, string matKhau, int quyen)
        {
            var tv = new tbl_thanhvien();
            tv.MaGiangVien = maSo;
            tv.HoTen = hoTen;
            tv.NgaySinh = ngaySinh;
            tv.HinhAnh = hinhAnh;
            tv.Email = email;
            tv.MatKhau = matKhau;
            tv.Quyen = quyen;
            try
            {
                db.tbl_thanhvien.Add(tv);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public bool KiemTra_Mail(string mail)
        {
            var linq = db.tbl_thanhvien.FirstOrDefault(x => x.Email == mail);
            if (linq == null)
                return true;
            return false;
        }
        // xóa thành viên
        public bool XoaThanhVien(string maSo)
        {
            try
            {
                var xoa = (from tv in db.tbl_thanhvien where tv.MaGiangVien == maSo select tv).Single();
                db.tbl_thanhvien.Remove(xoa);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}