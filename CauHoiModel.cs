using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class CauHoiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        // danh sách câu hỏi
        public int SoLuong()
        {
            return db.tbl_cauhoi.Count();
        }
        public List<tbl_cauhoi> DanhSach_CauHoi()
        {
            return db.tbl_cauhoi.OrderByDescending(x => x.MaCauHoi).ToList();
        }
        // mức độ câu hỏi
        public List<tbl_cauhoi> MucDo(int maMD)
        {
            return db.tbl_cauhoi.Where(x => x.MaMucDoCauHoi == maMD).ToList();
        }
        // lọc theo mức độ
        public List<tbl_cauhoi> CauHoi_MucDo(int mucDo)
        {
            return db.tbl_cauhoi.Where(ch => ch.MaMucDoCauHoi == mucDo).ToList();
        }
        // kết quả học tập
        public List<tbl_cauhoi> KetQua(string maKQ)
        {
            return db.tbl_cauhoi.Where(x => x.MaKQHT == maKQ).ToList();
        }
        // lọc theo mức độ
        public List<tbl_cauhoi> CauHoi_KetQua(string kqHT)
        {
            return db.tbl_cauhoi.Where(ch => ch.MaKQHT == kqHT).ToList();
        }
        // tìm câu hỏi max
        public int CauHoi_Max()
        {
            var linq = db.tbl_cauhoi.OrderByDescending(ch => ch.MaCauHoi).FirstOrDefault();
            return linq.MaCauHoi;
        }
        
        // xử lý chuỗi
        public string Xuly(string nd)
        {
            string s = nd.Replace(".", "");
            string s1 = s.Replace("?", "");            
            return s1;
        }
        // kiểm tra câu hỏi
        public bool KiemTra_CauHoi(string noiDung)
        {
            var linq = from ch in db.tbl_cauhoi
                       where ch.CauHoi.Contains(noiDung)
                       select ch;            
            string nd = "";
            foreach (var l in linq.Take(1).ToList())
            {
                nd = l.CauHoi;
            }
            if (Xuly(noiDung) == Xuly(nd))
                return false;
            else
                return true;
        }
        // thêm câu hỏi
        public bool Them_CauHoi(string cauHoi, string hinhAnh, string ghiChu, string kqht, int mucDo)
        {
            var ch = new tbl_cauhoi();
            ch.CauHoi = cauHoi;
            ch.HinhAnh = hinhAnh;
            ch.GhiChu = ghiChu;
            ch.MaKQHT = kqht;
            ch.MaMucDoCauHoi = mucDo;
            try
            {
                db.tbl_cauhoi.Add(ch);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        //xóa câu hỏi
        public bool Xoa_CauHoi(int maCH)
        {
            try
            {
                var xoaCH = (from ch in db.tbl_cauhoi where ch.MaCauHoi == maCH select ch).Single();
                db.tbl_cauhoi.Remove(xoaCH);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }
        // tìm câu hỏi theo mã câu hỏi
        public List<tbl_cauhoi> Tim_CauHoi(int maCH)
        {
            return db.tbl_cauhoi.Where(x => x.MaCauHoi == maCH).ToList();
        }
        // tìm câu hỏi trong đề
        public bool TimCauHoi(int maCH)
        {
            var linq = db.tbl_dethi_cauhoi.Count(x => x.MaCauHoi == maCH);

            if (linq > 0)
                return true;
            else
                return false;
        }
        // sửa câu hỏi
        public void Sua_CauHoi(int maCH, string cauHoi, string hinhAnh, string ghiChu, string kqht, int mucDo)
        {
            var result = (from ch in db.tbl_cauhoi
                         where ch.MaCauHoi == maCH
                         select ch);
            foreach (var ch in result)
            {
                ch.CauHoi = cauHoi;
                ch.HinhAnh = hinhAnh;
                ch.GhiChu = ghiChu;
                ch.MaKQHT = kqht;
                ch.MaMucDoCauHoi = mucDo;
            }
            db.SaveChanges();
        }
        // lấy số lượng câu hỏi
        public List<tbl_cauhoi> LayMa_CauHoi(string kqht, int mucDo, int soCau)
        {
            var linq = db.tbl_cauhoi.Where(x => x.MaKQHT == kqht && x.MaMucDoCauHoi == mucDo).OrderBy(x => Guid.NewGuid()).Take(soCau);
            return linq.ToList();
        }
       
    }
}