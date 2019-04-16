using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class ChiTiet_PhieuLamBaiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public bool Them_ChiTietPhieu(int maDTCH, int maPhieu, int phuongAn)
        {
            var ct = new tbl_chitiet_phieulambai();
            ct.MaDeThiCauHoi = maDTCH;
            ct.MaPhieu = maPhieu;
            ct.PhuongAnChon = phuongAn;
            try
            {
                db.tbl_chitiet_phieulambai.Add(ct);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public void CapNhat_ChiTietPhieu(int maDTCH1, int maDTCH2, int maPhieu, int phuongAn)
        {
            var result = from ct in db.tbl_chitiet_phieulambai
                         where ct.MaPhieu == maPhieu && ct.MaDeThiCauHoi== maDTCH1
                         select ct;
            foreach (var p in result)
            {
                p.MaDeThiCauHoi = maDTCH2;           
                p.PhuongAnChon = phuongAn;
            }
            db.SaveChanges();
        }
        // xóa chi tiết phiếu làm bài
        public bool XoaChiTietPhieu(int maPhieu)
        {
            try
            {
                var xoa = (from ct in db.tbl_chitiet_phieulambai where ct.MaPhieu == maPhieu select ct);
                db.tbl_chitiet_phieulambai.RemoveRange(xoa);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public void PhuongAnChon(int maDTCH, int maPhieu, int phuongAn)
        {
            var result = from ct in db.tbl_chitiet_phieulambai
                         where ct.MaPhieu == maPhieu && ct.MaDeThiCauHoi == maDTCH
                         select ct;
            foreach (var p in result)
            {               
                p.PhuongAnChon = phuongAn;
            }
            db.SaveChanges();
        }
        public int ChamDiem(int maPhieu)
        {
            var linq = from ctphieu in db.tbl_chitiet_phieulambai                      
                       where ctphieu.MaPhieu == maPhieu 
                       select ctphieu.PhuongAnChon;
           int diem = 0;  
           foreach( var l in linq.ToList() )
            {
                var result = from p in db.tbl_phuongan
                             where p.MaPhuongAn == l
                             select p.TrangThai;

                foreach(var r in result)
                {
                    if (r == 1)
                        diem++;
                }
            }

            return diem;         
        }
        public List<tbl_chitiet_phieulambai> BaiDaCham(string maSV)
        {
            var linq = from ctp in db.tbl_chitiet_phieulambai
                       from ph in db.tbl_phieulambai
                       where ctp.MaPhieu == ph.MaPhieu && ph.TrangThai == 1 && ph.MaNguoiDung == maSV
                       select ctp;                      
                       
            return linq.ToList();

        }
    }
}