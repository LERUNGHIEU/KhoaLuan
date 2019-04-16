using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class PhuongAnModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        // lấy phương án theo mã câu hỏi
        public List<tbl_phuongan> PhuongAn_CauHoi(int maCH)
        {
            return db.tbl_phuongan.Where(pa => pa.MaCauHoi == maCH).ToList();
        }
        // thêm phương án
        public bool Them_PhuongAn(string phuongAn, string hinhAnh, int trangThai, int maCH)
        {
            var ph = new tbl_phuongan();
            ph.PhuongAn = phuongAn;
            ph.HinhAnh = hinhAnh;
            ph.TrangThai = trangThai;
            ph.MaCauHoi = maCH;
            try
            {
                db.tbl_phuongan.Add(ph);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // xóa phương án
        public bool Xoa_PhuongAn(int maCH)
        {
            try
            {
                var xoaPA = (from pa in db.tbl_phuongan where pa.MaCauHoi == maCH select pa).ToList();

                db.tbl_phuongan.RemoveRange(xoaPA);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // sửa phương án
        public void Sua_PhuongAn(int maPhuongAn, int maCH, string phuongAn, string hinhAnh, int trangThai)
        {
            var result = from p in db.tbl_phuongan
                         where p.MaCauHoi == maCH && p.MaPhuongAn == maPhuongAn
                         select p;
            foreach (var ch in result)
            {
                ch.PhuongAn = phuongAn;
                ch.HinhAnh = hinhAnh;
                ch.TrangThai = trangThai;
            }
            db.SaveChanges();
        }
    }
}