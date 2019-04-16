using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class PhieuLamBaiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        // kiểm tra phiếu theo sinh viên
        public bool KiemTra_Phieu(string maSV, int trangThai)
        {
            var linq = db.tbl_phieulambai.FirstOrDefault(x => x.MaNguoiDung == maSV && x.TrangThai == trangThai);
            if (linq == null)
                return true;
            return false;
        }
        // lấy phiếu từ mã sinh viên
        public int Phieu_SinhVien(string maSV, int trangThai)
        {
            var linq = db.tbl_phieulambai.OrderByDescending(x=>x.MaPhieu).Where(x => x.MaNguoiDung == maSV && x.TrangThai==trangThai).Take(1).ToList();
            int phieu=0;
            foreach(var l in linq)
            {
                phieu = l.MaPhieu;
            }

            return phieu;
        }        
        // thêm phiếu làm bài
        public bool Them_PhieuLamBai(double diem, int trangThai, string maSV)
        {
            var ph = new tbl_phieulambai();
            ph.Diem = diem;
            ph.TrangThai = trangThai;
            ph.MaNguoiDung = maSV;
            try
            {
                db.tbl_phieulambai.Add(ph);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // cập nhật phiếu làm bài
        public void CapNhat_Phieu(int maPhieu, double diem, int trangThai, string maSV)
        {
            var result = from ph in db.tbl_phieulambai
                         where ph.MaNguoiDung == maSV && ph.MaPhieu==maPhieu
                         select ph;
            foreach (var p in result)
            {
                p.Diem = diem;
                p.TrangThai = trangThai;                
            }
            db.SaveChanges();
        }
        // bảng xếp hạng 
        public List<BangXepHang> BangXepHang()
        {
            var linq = from ph in db.tbl_phieulambai
                       from sv in db.tbl_sinhvien
                       where ph.MaNguoiDung == sv.MaSinhVien && ph.TrangThai == 1
                       orderby ph.Diem descending
                       select new BangXepHang
                       {
                           tenSV = sv.HoTen,
                           diem = ph.Diem
                       };

            return linq.Take(3).ToList();       
        }
        public List<BangXepHang> DanhSach_KetQua(string dotThi)
        {
            var linq = from ph in db.tbl_phieulambai
                       from sv in db.tbl_sinhvien
                       from ds in db.tbl_danhsachthi
                       where ph.MaNguoiDung == ds.MaSinhVien && ds.MaSinhVien == sv.MaSinhVien
                       && ds.MaDotThi == dotThi && ph.TrangThai == 2
                       select new BangXepHang
                       {
                           tenSV = sv.HoTen,
                           diem = ph.Diem
                       };
            return linq.ToList();
        }
        public double Diem(string maSV)
        {
            double diem = 0;
            var linq = db.tbl_phieulambai.Where(ph => ph.MaNguoiDung == maSV && ph.TrangThai == 2).ToList();
            foreach(var l in linq)
            {
                diem = l.Diem;
            }
            return diem;
        }
        // thống kê 
        public int ThongKe(double n, double m, string dotThi)
        {
            int dem = 0;
            var linq = from ph in db.tbl_phieulambai
                       from sv in db.tbl_sinhvien
                       from ds in db.tbl_danhsachthi
                       where ph.MaNguoiDung == ds.MaSinhVien && ds.MaSinhVien == sv.MaSinhVien
                       && ds.MaDotThi == dotThi && ph.TrangThai == 2
                       select ph;
            foreach (var l in linq.ToList())
            {
                if (l.Diem >= n && l.Diem <= m)
                    dem++;
            }
            return dem;
        }
    }
}