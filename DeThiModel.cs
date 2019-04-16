using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DeThiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public List<tbl_dethi> DanhSach_DeThi()
        {
            return db.tbl_dethi.ToList();
        }
        // kiêm tra đề thi
        // kiểm tra đợt thi
        public bool KiemTra_DotThi(string maDT)
        {
            var linq = db.tbl_dethi.FirstOrDefault(x => x.MaDotThi == maDT);
            if (linq == null)
                return true;
            return false;
        }
        // lấy mã vào phòng
        public List<tbl_dethi> DeThi(string maSV)
        {
           
            var linq = from ds in db.tbl_danhsachthi
                       from dt in db.tbl_dotthi
                       from de in db.tbl_dethi
                       where de.MaDotThi == dt.MaDotThi && dt.MaDotThi == ds.MaDotThi && ds.MaSinhVien == maSV
                       select de;
            return linq.OrderBy(x => Guid.NewGuid()).Take(1).ToList();
        }
        
        // thêm đề thi
        public bool ThemDeThi(string maDe, string tenDe, string maCaThi, DateTime ngayTao, DateTime ngayThi, int trangThai, string maGV, string maDotThi)
        {
            var dt = new tbl_dethi();
            dt.MaDeThi = maDe;
            dt.TenDeThi = tenDe;
            dt.MaCaThi = maCaThi;
            dt.NgayTao = ngayTao;
            dt.NgayThi = ngayThi;            
            dt.TrangThai = trangThai;
            dt.MaGiangVien = maGV;
            dt.MaDotThi = maDotThi;
            try
            {
                db.tbl_dethi.Add(dt);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // cập nhật thời gian thi
        public void CapNhat(string maDT, DateTime tgian)
        {
            string []ma = maDT.Trim().Split('_');
            string maDotThi = ma[0];
            var linq = db.tbl_dethi.Where(x => x.MaDeThi.Contains(maDotThi)).ToList();
            foreach(var l in linq)
            {
                l.NgayThi = tgian;
            }
            db.SaveChanges();

        }
        // kiểm tra trạng thái
        public bool TrangThai(string maDT)
        {
            var linq = db.tbl_dethi.Where(x => x.MaDeThi == maDT && x.TrangThai == 0).ToList();
            if (linq != null)
                return true;
            return false;
        }
        //xóa bộ đề thi
        public bool XoaBoDe(string maDT)
        {
            string[] ma = maDT.Trim().Split('_');
            string maDeThi = ma[0];
            try
            {
                var xoaDT = from md in db.tbl_dethi
                            where md.MaDeThi.Contains(maDeThi)
                            select md;

                db.tbl_dethi.RemoveRange(xoaDT);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        
        // lấy mã đề thi theo mã sinh viên
        public string MaDeThi(string maSV)
        {
            string maDe = "";
            var linq = from ds in db.tbl_danhsachthi
                       from dt in db.tbl_dotthi
                       from dethi in db.tbl_dethi
                       where ds.MaSinhVien == maSV && ds.MaDotThi == dt.MaDotThi
                       && dt.MaDotThi == dethi.MaDeThi
                       select dethi;
            foreach(var l in linq.ToList())
            {
                maDe = l.MaDeThi;
            }
          return maDe;
        }
    }
}