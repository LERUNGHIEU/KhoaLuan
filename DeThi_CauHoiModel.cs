using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DeThi_CauHoiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public bool ThemDeThi_CauHoi(string deThi, int cauHoi)
        {
            var dtch = new tbl_dethi_cauhoi();
            dtch.MaDeThi = deThi;
            dtch.MaCauHoi = cauHoi;
            try
            {
                db.tbl_dethi_cauhoi.Add(dtch);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // lấy số câu hỏi theo mức độ
        public int SoMucDo(string maDe, int md)
        {
            return db.tbl_dethi_cauhoi.Where(x => x.MaDeThi == maDe && x.tbl_cauhoi.MaMucDoCauHoi == md).Count();
        }
        // Danh sách mã đề thi câu hỏi
        public List<tbl_dethi_cauhoi> DanhSach_MaDTCH(string maDe)
        {
            return db.tbl_dethi_cauhoi.Where(x => x.MaDeThi == maDe).ToList();
        }
        // trộn câu hỏi
        public List<tbl_dethi_cauhoi> DanhSachCauHoiTron(string maDe)
        {
            return db.tbl_dethi_cauhoi.Where(x => x.MaDeThi == maDe).OrderBy(x => Guid.NewGuid()).ToList();
        }
        // bài thi theo đề
        public List<tbl_dethi_cauhoi> BaiThi(string maDe)
        {

            return db.tbl_dethi_cauhoi.Where(x => x.MaDeThi == maDe).ToList();
            //OrderBy(x => Guid.NewGuid())
        }
        // tìm câu hỏi theo mã câu hỏi
        public List<tbl_dethi_cauhoi> CauHoi(int maCH)
        {
            return db.tbl_dethi_cauhoi.Where(x => x.MaCauHoi == maCH).Take(1).ToList();
        }
        // lấy mức độ câu hỏi
        public int MaMucDo(int maCH)
        {
           var linq  = db.tbl_cauhoi.FirstOrDefault(x => x.MaCauHoi == maCH);
           return linq.MaMucDoCauHoi;
        }
        // lấy kết quả học tập
        public string KetQuaHocTap(int maCH)
        {
            var linq = db.tbl_cauhoi.FirstOrDefault(x => x.MaCauHoi == maCH);
            return linq.MaKQHT;
        }
        // tìm câu hỏi theo đề
        public List<tbl_cauhoi> LayMaTru_CauHoi(string maDe, int maCH)
        {
            var cauHoi = db.tbl_cauhoi.Where(x => x.MaCauHoi == maCH).ToList();
            string kqht = KetQuaHocTap(maCH);
            int mucDo = MaMucDo(maCH);
            var linq = from ch in db.tbl_cauhoi
                       from dtch in db.tbl_dethi_cauhoi
                       where ch.MaCauHoi != dtch.MaCauHoi && dtch.MaDeThi == maDe
                       && ch.MaMucDoCauHoi == mucDo && ch.MaKQHT == kqht
                       select ch;
            var sql = linq.ToList().OrderBy(x => Guid.NewGuid()).Take(1);
            return sql.ToList();
        }
        // cập nhật câu hỏi cho đề thi
        public void ThayTheCauHoi(string maDe, int maCH1, int maCH2)
        {

            var result = from p in db.tbl_dethi_cauhoi
                         where p.MaDeThi.Contains(maDe) && p.MaCauHoi == maCH1
                         select p;
            foreach (var i in result)
            {
                i.MaCauHoi = maCH2;
            }
            db.SaveChanges();
        }
        // câu hỏi thuộc đề thi
        public List<tbl_dethi_cauhoi> CauHoiThuocDeThi(string maDe, int maCH)
        {
            var linq = from dtch in db.tbl_dethi_cauhoi
                       where dtch.MaDeThi == maDe && dtch.MaCauHoi == maCH
                       select dtch;
            return linq.Take(1).ToList();
        }
        // xóa đề thi câu hỏi
        public bool XoaDeThiCauHoi(string maDT)
        {
            string[] ma = maDT.Trim().Split('_');
            string maDeThi = ma[0];
            try
            {
                var xoaDTCH = from md in db.tbl_dethi_cauhoi
                              where md.MaDeThi.Contains(maDeThi)
                              select md;

                db.tbl_dethi_cauhoi.RemoveRange(xoaDTCH);
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