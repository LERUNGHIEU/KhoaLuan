using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DotThi_SinhVienModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();

        public List<DanhSachThi> DanhSach_SinhVien(string maDT)
        {
            var linq = from ct in db.tbl_danhsachthi
                       from sv in db.tbl_sinhvien
                       where ct.MaDotThi == maDT && ct.MaSinhVien == sv.MaSinhVien
                       select new DanhSachThi
                       {
                           maSo = sv.MaSinhVien,
                           hoTen = sv.HoTen,
                           ngaySinh = sv.NgaySinh,
                           phai = sv.Phai,
                           mail = sv.Email,
                           trangThai = sv.Khoa
                       };
            return linq.ToList();
        }        
        public bool Them_SinhVien(string maDT, string maSV)
        {
            var ds = new tbl_danhsachthi();
            ds.MaDotThi = maDT;
            ds.MaSinhVien = maSV;
            try
            {
                db.tbl_danhsachthi.Add(ds);
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
        public bool Xoa_DT_SV(string maSV)
        {
            try
            {
                var xoaSV = (from sv in db.tbl_danhsachthi where sv.MaSinhVien == maSV select sv).Single();
                db.tbl_danhsachthi.Remove(xoaSV);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public List<tbl_danhsachthi> DanhSach_MaSV(string maDotThi)
        {
            return db.tbl_danhsachthi.Where(ds => ds.MaDotThi == maDotThi).ToList();
        }
        public bool Xoa_DanhSachThi(string maDotThi)
        {
            try
            {
                var xoa = (from ct in db.tbl_danhsachthi where ct.MaDotThi == maDotThi select ct);
                db.tbl_danhsachthi.RemoveRange(xoa);
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