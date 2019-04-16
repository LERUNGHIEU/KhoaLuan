using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DotThiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public int SoLuong()
        {
            return db.tbl_dotthi.Count();
        }
        // danh sách đợt thi
        public List<tbl_dotthi> DanhSach_DotThi()
        {
            return db.tbl_dotthi.OrderByDescending(x=>x.MaDotThi).ToList();
        }
        
        // tạo mã đợt thi
        public string TaoMaDotThi(string ma)
        {
            var linq = db.tbl_dotthi.OrderByDescending(ch => ch.NgayTao).FirstOrDefault(x => x.TrangThai != 2);
            if (linq != null)
            {
                int l = (linq.MaDotThi.Trim()).Length;
                string so = linq.MaDotThi.Substring(ma.Length, l - ma.Length);
                int maMoi = (Convert.ToInt32(so.ToString())+1);
                return (ma + maMoi.ToString()).ToString();
            }

            return (ma + "1").ToString();
        }
        // tạo mã luyện thi
        public string TaoMaLuyenThi(string ma)
        {
            var linq = db.tbl_dotthi.OrderByDescending(ch => ch.NgayTao).FirstOrDefault(x => x.TrangThai == 2);
            if (linq != null)
            {
                int l = (linq.MaDotThi.Trim()).Length;
                string so = linq.MaDotThi.Substring(ma.Length, l - ma.Length);
                int maMoi = (Convert.ToInt32(so.ToString())+1);
                return (ma + maMoi.ToString()).ToString();
            }

            return (ma + "1").ToString();
        }
        // kiểm tra nhóm lớp
        public bool KiemTra_NhomLop(int nhom, string maLop)
        {
            var linq = db.tbl_dotthi.FirstOrDefault(x => x.MaLop == maLop && x.Nhom == nhom);
            if (linq == null)
                return true;
            return false;
        }
        // lấy nhóm và mã lớp
        public List<tbl_dotthi> NhomLop(string maDT)
        {
           return db.tbl_dotthi.Where(x => x.MaDotThi == maDT).ToList();
            
        }
        // kiểm tra mã lớp
        public bool KiemTra_MaLop(string maLop)
        {
            var linq = db.tbl_dotthi.FirstOrDefault(dt => dt.MaLop == maLop);
            if (linq == null)
                return true;
            return false;
        }

        // thêm đợt thi
        public bool Them_DotThi(string maDT, string tenDT, DateTime ngayTao, int nhom, string maLop, string moTa, int trangThai)
        {
            var dt = new tbl_dotthi();
            dt.MaDotThi = maDT;
            dt.TenDotThi = tenDT;
            dt.NgayTao = ngayTao;
            dt.Nhom = nhom;
            dt.MaLop = maLop;
            dt.MoTa = moTa;
            dt.TrangThai = trangThai;
            try
            {
                db.tbl_dotthi.Add(dt);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // xóa đợt thi
        public bool XoaDotThi(string maDotThi)
        {
            try
            {
                var xoa = (from dt in db.tbl_dotthi where dt.MaDotThi == maDotThi select dt).Single();
                db.tbl_dotthi.Remove(xoa);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public void CapNhat_DotThi(string maDotThi, string tenDotThi, int nhom, string maLop, string moTa)
        {
            var result = from dt in db.tbl_dotthi
                         where dt.MaDotThi == maDotThi 
                         select dt;
            foreach (var ch in result)
            {
                ch.TenDotThi = tenDotThi;
                ch.Nhom = nhom;
                ch.MaLop = maLop;
                ch.MoTa = moTa;
            }
            db.SaveChanges();
        }
       
        public List<DotThi> ChiTiet_DotThi()
        {
            var linq = from dt in db.tbl_dotthi
                       where dt.TrangThai == 1
                       select new DotThi
                       {
                           maDotThi = dt.MaDotThi,
                           nhom = dt.Nhom,
                           lop = dt.tbl_lop.TenLop,
                           siSo = dt.tbl_danhsachthi.Count(),
                           tiLe = (from ds in db.tbl_danhsachthi
                                   from ph in db.tbl_phieulambai
                                   where ds.MaSinhVien == ph.MaNguoiDung && ds.MaDotThi == dt.MaDotThi && ph.TrangThai == 2
                                   select ph).Count(x => x.Diem >= 5)
                       };

            return linq.ToList();
        }
       
    }
}