using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ThiOnline.Models;


namespace ThiOnline.Controllers
{
    public class SinhVienController : Controller
    {
        // model
        CauHoiModel m_CauHoi = new CauHoiModel();
        MucDoModel m_MucDo = new MucDoModel();
        KQHTModel m_Kqht = new KQHTModel();
        DeThi_CauHoiModel m_DT_CH = new DeThi_CauHoiModel();
        DeThiModel m_DeThi = new DeThiModel();
        PhieuLamBaiModel m_Phieu = new PhieuLamBaiModel();
        ChiTiet_PhieuLamBaiModel m_ChiTietPhieu = new ChiTiet_PhieuLamBaiModel();
        // GET: SinhVien
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MucDoCauHoi()
        {
            string id = Request.QueryString["idMucDo"];
            int maMD = Convert.ToInt32(id);
            Session["ten"] = m_MucDo.TenMD(maMD);
            var model = m_CauHoi.MucDo(maMD);
            return View("MucDoCauHoi", model);
        }
        [HttpGet]
        public ActionResult KetQuaHocTap()
        {
            string id = Request.QueryString["idKqht"];
            Session["ten"] = m_Kqht.TenKQ(id);
            var model = m_CauHoi.KetQua(id);
            return View("KetQuaHocTap", model);
        }
        [HttpGet]
        public ActionResult LuyenThi()
        {
            string maDe = Request.QueryString["maDe"];
            Session["maDe"] = maDe;
            string maSV = Session["maSo"].ToString();
            bool ktPhieu = m_Phieu.KiemTra_Phieu (maSV, 1);
            if (ktPhieu) // không có phiếu 1
            {
                bool ktPhieu_ = m_Phieu.KiemTra_Phieu(maSV, 0);
                if (ktPhieu_ == false)
                {
                    int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 0));
                    bool xoaCT = m_ChiTietPhieu.XoaChiTietPhieu(maPhieu);                   
                    if (xoaCT)
                    {                                               
                        var danhsachMa = m_DT_CH.DanhSach_MaDTCH(maDe);
                        bool themCT_Phieu;
                        foreach (var ds in danhsachMa)
                        {
                            themCT_Phieu = m_ChiTietPhieu.Them_ChiTietPhieu(ds.MaDeThiCauHoi, maPhieu, 0);
                        }
                    }
                }
                else
                {
                    bool themPhieu = m_Phieu.Them_PhieuLamBai(0, 0, maSV);
                    if (themPhieu)
                    {
                        int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 0));
                        var danhsachMa = m_DT_CH.DanhSach_MaDTCH(maDe);
                        bool themCT_Phieu;
                        foreach (var ds in danhsachMa)
                        {
                            themCT_Phieu = m_ChiTietPhieu.Them_ChiTietPhieu(ds.MaDeThiCauHoi, maPhieu, 0);
                        }
                    }
                }          
            }
            else
            {              
                int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV,1));
                bool xoaCT = m_ChiTietPhieu.XoaChiTietPhieu(maPhieu);
                if(xoaCT)
                {
                    bool themPhieu = m_Phieu.Them_PhieuLamBai(0, 0, maSV);
                    if (themPhieu)
                    {
                        int maPhieu_ = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 0));
                        var danhsachMa = m_DT_CH.DanhSach_MaDTCH(maDe);
                        bool themCT_Phieu;
                        foreach (var ds in danhsachMa)
                        {
                            themCT_Phieu = m_ChiTietPhieu.Them_ChiTietPhieu(ds.MaDeThiCauHoi, maPhieu_, 0);
                        }

                    }
                      
                }              
            }
            var model = m_DT_CH.BaiThi(maDe);           
            return View("LuyenThi", model);
        }
        [HttpPost]
        public void CapNhat_PhuongAn(string maDe, int maDTCH, int maPA)
        {
            string maSV = Session["maSo"].ToString();
            int maDeThiCH = Convert.ToInt32(maDTCH);
            int phuongAn = Convert.ToInt32(maPA);
            int maPhieu = m_Phieu.Phieu_SinhVien(maSV, 0);

            m_ChiTietPhieu.PhuongAnChon(maDeThiCH, maPhieu, phuongAn);           
        }
        public ActionResult ChamDiem()
        {
            string maSV = Session["maSo"].ToString();
            int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 0));
            ViewData["Diem"] = m_ChiTietPhieu.ChamDiem(maPhieu) * 0.25;
            double diem = Convert.ToInt32(m_ChiTietPhieu.ChamDiem(maPhieu)) * 0.25;
            m_Phieu.CapNhat_Phieu(maPhieu, diem, 1, maSV);
            var model = m_ChiTietPhieu.BaiDaCham(maSV);
            return View("KetQua", model);

        }
        // chờ thi        
        public ActionResult ChoThi()
        {
            string maSV = Session["maSo"].ToString();
            var linq = m_DeThi.DeThi(maSV);
            string maDe = "";
            foreach(var l in linq)
            {
                ViewBag.MaVaoPhong = l.MaCaThi;
                ViewBag.ThoiGian = l.NgayThi.ToString("dd/MM/yyyy HH:mm:ss");
                maDe = l.MaDeThi;
            }
            Session["maDeThi"] = maDe;
            return View("ChoThi");
        }
        // cập nhật phương án
        [HttpPost]
        public void CapNhat_PhuongAnThi(string maDe, int maDTCH, int maPA)
        {
            string maSV = Session["maSo"].ToString();
            int maDeThiCH = Convert.ToInt32(maDTCH);
            int phuongAn = Convert.ToInt32(maPA);
            int maPhieu = m_Phieu.Phieu_SinhVien(maSV, 2);

            m_ChiTietPhieu.PhuongAnChon(maDeThiCH, maPhieu, phuongAn);
        }
        // chấm điểm thi
        public void ChamDiemThi()
        {
            string maSV = Session["maSo"].ToString();
            int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 2));           
            double diem = Convert.ToInt32(m_ChiTietPhieu.ChamDiem(maPhieu)) * 0.25;
            m_Phieu.CapNhat_Phieu(maPhieu, diem, 2, maSV);           

        }
        public ActionResult Thi()
        {
            string maDe = Session["maDeThi"].ToString();
            string maSV = Session["maSo"].ToString();
            bool themPhieu = m_Phieu.Them_PhieuLamBai(0, 2, maSV);
            if (themPhieu)
            {
                int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 2));
                var danhsachMa = m_DT_CH.DanhSach_MaDTCH(maDe);
                bool themCT_Phieu;
                foreach (var ds in danhsachMa)
                {
                    themCT_Phieu = m_ChiTietPhieu.Them_ChiTietPhieu(ds.MaDeThiCauHoi, maPhieu, 0);
                }
            }
            var model = m_DT_CH.BaiThi(maDe);
            return View("Thi", model);
        }
        public ActionResult ThongBao()
        {
            string maSV = Session["maSo"].ToString();
            int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 2));            
            ViewBag.SoCauDung = m_ChiTietPhieu.ChamDiem(maPhieu);
            ViewBag.Diem = m_ChiTietPhieu.ChamDiem(maPhieu) * 0.25;
            return View("ThongBao");
        }   
        public ActionResult SendMail()
        {
            string maSV = Session["maSo"].ToString();
            int maPhieu = Convert.ToInt32(m_Phieu.Phieu_SinhVien(maSV, 2));
            int soCau = m_ChiTietPhieu.ChamDiem(maPhieu);
            double diem = m_ChiTietPhieu.ChamDiem(maPhieu) * 0.25;
            string body = "<h3>Xin chào "+Session["hoTen"].ToString()+"</h3><br>";
            body += "<p>Bạn vừa kết thúc bài thi môn tin học ứng dụng cơ bản. Số câu đúng: <b>" + soCau.ToString() + "</b> và số điểm đạt được là: <b>" + diem.ToString() + "</b></p>";
            using (MailMessage mm = new MailMessage("leetrunghieeus1996@gmail.com","110115108@sv.tvu.edu.vn"))
            {
                mm.Subject = "Thi kết thúc môn";
                mm.Body = body;                
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("leetrunghieeus1996@gmail.com", "31021996");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                ViewBag.Message = "Đã gửi";
            }
            return ThongBao();
        }
    

    }
}
