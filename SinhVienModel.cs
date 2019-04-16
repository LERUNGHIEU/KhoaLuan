using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThiOnline.Common;

namespace ThiOnline.Models
{
    public class SinhVienModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        MD5 md5 = new MD5();
        public bool KiemTra_SinhVien(string maSV)
        {
            var linq = db.tbl_sinhvien.FirstOrDefault(x => x.MaSinhVien == maSV);
            if (linq == null)
                return true;
            return false;
        }
        public bool ThemSinhVien(string maSo, string hoTen, string ngaySinh, string gioiTinh, int khoa)
        {
            string emial = maSo + "@sv.tvu.edu.vn";
            string hinhAnh = "";
            var sv = new tbl_sinhvien();
            sv.MaSinhVien = maSo;
            sv.HoTen = hoTen;
            sv.NgaySinh = ngaySinh;
            sv.Phai = gioiTinh;
            sv.Email = emial;
            sv.MatKhau = md5.MaHoa(ngaySinh.Replace("/","")).ToString();
            sv.HinhAnh = hinhAnh;
            sv.Khoa = khoa;
            try
            {
                db.tbl_sinhvien.Add(sv);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // xóa sinh viên
        public bool Xoa_SinhVien(string maSV)
        {
            try
            {
                var xoaSV = (from sv in db.tbl_sinhvien where sv.MaSinhVien == maSV select sv).Single();
                db.tbl_sinhvien.Remove(xoaSV);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // lấy trạng thái 
        public int TrangThai_SinhVien(string maSV)
        {
            var linq = db.tbl_sinhvien.FirstOrDefault(x => x.MaSinhVien == maSV);
            return linq.Khoa;
        }
        // cập nhật trạng thái
        public void CapNhat_TrangThai(string maSV, int trangThai)
        {
            var result = from sv in db.tbl_sinhvien
                         where sv.MaSinhVien == maSV
                         select sv;
            foreach (var s in result)
            {
                s.Khoa = trangThai;
            }
            db.SaveChanges();
        }
        // cập nhật sinh viên
        public void CapNhat_SinhVien(string maSo, string hoTen, string ngaySinh, string gioiTinh)
        {
            var result = from sv in db.tbl_sinhvien
                         where sv.MaSinhVien == maSo
                         select sv;
            foreach (var s in result)
            {
                s.HoTen = hoTen;
                s.NgaySinh = ngaySinh;
                s.MatKhau = md5.MaHoa(ngaySinh.Replace("/", "")).ToString();
                s.Phai = gioiTinh;
            }
            db.SaveChanges();
        }

    }
}