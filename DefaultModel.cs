using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThiOnline.Common;

namespace ThiOnline.Models
{
    public class DefaultModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        // kiểm tra đăng nhập
        public bool KiemTra_GV(string maSo, string matKhau)
        {
            var model = db.tbl_thanhvien.FirstOrDefault(gv => gv.MaGiangVien == maSo && gv.MatKhau == matKhau);
            if (model != null)
                return true;
            return false;
        }
        public bool KiemTra_SV(string maSo, string matKhau)
        {
            var model = db.tbl_sinhvien.FirstOrDefault(sv=>sv.MaSinhVien == maSo && sv.MatKhau == matKhau);
            if (model != null)
                return true;
            return false;
        }
        // lấy thông tin người dùng
        public List<NguoiDung> ThongTin_GV(string maSo)
        {
            var model = from gv in db.tbl_thanhvien
                        where gv.MaGiangVien == maSo
                        select new NguoiDung
                        {
                            maSo = gv.MaGiangVien,
                            hoTen = gv.HoTen,
                            hinhAnh = gv.HinhAnh,
                            matKhau = gv.MatKhau,
                            mail = gv.Email
                        };
            return model.ToList();
        }
        public List<NguoiDung> ThongTin_SV(string maSo)
        {
            var model = from sv in db.tbl_sinhvien
                        where sv.MaSinhVien == maSo
                        select new NguoiDung
                        {
                            maSo = sv.MaSinhVien,
                            hoTen = sv.HoTen,
                            hinhAnh = sv.HinhAnh,
                            matKhau = sv.MatKhau,
                            mail = sv.Email
                        };
            return model.ToList();
        }
        public void CapNhatNguoiDung(string maSo, string hoTen,string ngaySinh, string hinhAnh)
        {
            var result = from u in db.tbl_thanhvien
                         where u.MaGiangVien == maSo
                         select u;
            foreach (var r in result)
            {
                r.HoTen = hoTen;
                r.NgaySinh = ngaySinh;
                r.HinhAnh = hinhAnh;
            }
            db.SaveChanges();
        }
        public void DoiMatKhau(string maSo, string matKhau)
        {
            var result = from u in db.tbl_thanhvien
                         where u.MaGiangVien == maSo
                         select u;
            foreach (var r in result)
            {
                r.MatKhau = matKhau;
            }
            db.SaveChanges();
        }
    }
}