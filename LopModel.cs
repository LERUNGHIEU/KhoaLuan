using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class LopModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public int SoLuong()
        {
            return db.tbl_lop.Count();
        }
        // lấy danh sách lớp
        public List<tbl_lop> DanhSach_Lop()
        {
            return db.tbl_lop.ToList();
        }
        // kiểm tra mã lớp
        public bool KiemTra_MaLop(string maLop)
        {
            var linq = db.tbl_lop.FirstOrDefault(l => l.MaLop == maLop);
            if (linq == null)
                return true;
            return false;
        }
        // thêm mới lớp
        public bool Them_Lop(string maLop, string tenLop)
        {
            var lop = new tbl_lop();
            lop.MaLop = maLop;
            lop.TenLop = tenLop;
            try
            {
                db.tbl_lop.Add(lop);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // sửa lớp
        public void SuaLop(string maLop, string tenLop)
        {
            var result = (from l in db.tbl_lop
                          where l.MaLop == maLop
                          select l);
            foreach (var r in result)
            {
                r.TenLop = tenLop;     
            }
            db.SaveChanges();
        }
        // xóa lớp
        public bool XoaLop(string maLop)
        {
            try
            {
                var xoaLop = (from l in db.tbl_lop where l.MaLop == maLop select l).Single();
                db.tbl_lop.Remove(xoaLop);
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